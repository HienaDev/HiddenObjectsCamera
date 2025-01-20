using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine.Audio;

public class InCameraCheck : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Transform mainCamera;
    private Renderer[] meshRenderer;
    private Material[] defaultMaterials;
    private Plane[] cameraFrustum;
    private Collider col;
    private List<Material> objectMaterials;
    [SerializeField] private Material missingObjectMaterial;
    private Material[] missingObjectArray;

    [SerializeField] private bool wrongObject = false;
    [SerializeField] private bool door = false;

    [SerializeField] private HiddenObject objectDescription;
    private bool zooming;
    private bool destroyed;
    private bool completed;
    private bool wasBeingLookedAt;

    private Inventory inventory;

    [SerializeField] private Transform nextCamLocation;

    [SerializeField] private GameObject smokeParticles;
    [SerializeField] private GameObject itemParticles;
    private GameObject itemParticlesClone;
    private bool hasParticles = false;

    [Header("ANIMATION"), SerializeField] private float animationDuration = 10f;
    [SerializeField] private float maxTransparency = 0.5f;
    [SerializeField] private float initialDelayDuration = 3f;
    private Coroutine coroutine = null;

    private bool coroutineRunning = false;

    [SerializeField] private float popDuration = 2f;

    [SerializeField] private TakePhotos takePhotoScript;
    private float photoCooldown;
    private float justTookPhoto;

    [SerializeField] private GameObject placeSound;
    [SerializeField] private GameObject collectSound;

    [SerializeField] private AudioMixerGroup soundMixerGroup;

    void Start()
    {



        meshRenderer = GetComponentsInChildren<Renderer>();

        defaultMaterials = new Material[meshRenderer.Length];

        inventory = FindObjectOfType<Inventory>();
        takePhotoScript = FindObjectOfType<TakePhotos>();
        photoCooldown = takePhotoScript.PhotoCooldown;
        justTookPhoto = Time.time;

        mainCamera = Camera.main.transform;

        col = GetComponent<Collider>();

        for (int i = 0; i < meshRenderer.Length; i++)
        {
            defaultMaterials[i] = meshRenderer[i].material;
        }

        objectMaterials = new List<Material>();

        zooming = false;
        destroyed = false;
        wasBeingLookedAt = false;

        if (wrongObject)
        {
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].material = missingObjectMaterial;
                meshRenderer[i].enabled = false;
            }
        }


    }

    private void Update()
    {

        if(!DifficultyManager.Instance.GameStarted)
        {
            hasParticles = false;
        }

        if(!hasParticles && DifficultyManager.Instance.GameStarted)
        {
            Debug.Log("hi");

            if(itemParticlesClone != null)
            {
                Destroy(itemParticlesClone);
            }

            if (!DifficultyManager.Instance.isAnimated)
            {
                for (int i = 0; i < meshRenderer.Length; i++)
                {

                    meshRenderer[i].enabled = true;
                }
            }
            itemParticlesClone = Instantiate(DifficultyManager.Instance.currParticle, transform.position, Quaternion.identity);
            hasParticles = true;
        }

        if (!completed && DifficultyManager.Instance.GameStarted)
        {
            CheckIfInbounds();
        }
    }

    private void CheckIfInbounds()
    {
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);

        if ((!GeometryUtility.TestPlanesAABB(cameraFrustum, col.bounds)) || destroyed)
        {
            if (wrongObject && DifficultyManager.Instance.isAnimated)
            {
                for (int i = 0; i < meshRenderer.Length; i++)
                {
                    meshRenderer[i].enabled = false;
                }

                if (coroutineRunning)
                {
                    StopCoroutine(coroutine);
                    coroutineRunning = false;
                }
            }

            if (zooming)
            {
                cam.gameObject.GetComponentInParent<Zoom>().zooming = false;
                zooming = false;
            }

            wasBeingLookedAt = false;
        }
        else if (GeometryUtility.TestPlanesAABB(cameraFrustum, col.bounds))
        {
            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position, (transform.position - cam.transform.position), out hit, Mathf.Infinity))
            {
                if (hit.transform == transform)
                {
                    wasBeingLookedAt = true;

                    if (wrongObject)
                    {
                        if (DifficultyManager.Instance.isAnimated)
                        {
                            for (int i = 0; i < meshRenderer.Length; i++)
                            {
                                meshRenderer[i].enabled = true;
                            }
                        }


                        if (!coroutineRunning && DifficultyManager.Instance.isAnimated)
                        {
                            for (int i = 0; i < meshRenderer.Length; i++)
                            {
                                meshRenderer[i].material.color =
                                new Color(meshRenderer[i].material.color.r, meshRenderer[i].material.color.g, meshRenderer[i].material.color.b, 0f);

                            }


                            coroutine = StartCoroutine(TransparencyAnimation());
                        }

                        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) && Time.time - justTookPhoto > photoCooldown)
                        {
                            justTookPhoto = Time.time;
                            if (inventory.GetInventoryList().Contains(objectDescription))
                            {
                                inventory.RemoveObject(objectDescription);
                                for (int i = 0; i < meshRenderer.Length; i++)
                                {
                                    meshRenderer[i].material = defaultMaterials[i];
                                }

                                Instantiate(smokeParticles, transform.position, Quaternion.identity);
                                GameObject sound = Instantiate(collectSound, transform.position, Quaternion.identity);
                                AudioSource audioSource = sound.GetComponent<AudioSource>();
                                if (audioSource != null)
                                {
                                    audioSource.outputAudioMixerGroup = soundMixerGroup;
                                    audioSource.pitch = Random.Range(0.9f, 1.1f);
                                }

                                if (coroutineRunning && DifficultyManager.Instance.isAnimated)
                                {
                                    StopCoroutine(coroutine);
                                    coroutineRunning = false;
                                }

                                Destroy(itemParticlesClone);

                                StartCoroutine(Popped());

                                completed = true;
                            }
                        }
                    }
                    else if (!door)
                    {
                        cam.gameObject.GetComponentInParent<Zoom>().zooming = true;
                        zooming = true;

                        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) && !inventory.IsInventoryFull() && Time.time - justTookPhoto > photoCooldown)
                        {
                            justTookPhoto = Time.time;
                            cam.gameObject.GetComponentInParent<Inventory>().AddObject(objectDescription);
                            cam.gameObject.GetComponentInParent<Zoom>().zooming = false;
                            col.enabled = false;
                            destroyed = true;
                            Instantiate(smokeParticles, transform.position, Quaternion.identity);
                            GameObject sound = Instantiate(placeSound, transform.position, Quaternion.identity);
                            AudioSource audioSource = sound.GetComponent<AudioSource>();
                            if (audioSource != null)
                            {
                                audioSource.outputAudioMixerGroup = soundMixerGroup;
                                audioSource.pitch = Random.Range(0.9f, 1.1f);
                            }
                            Destroy(itemParticlesClone);
                            Destroy(gameObject, 0.1f);
                        }
                    }
                    else if (door)
                    {
                        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) && Time.time - justTookPhoto > photoCooldown)
                        {
                            justTookPhoto = Time.time;
                            mainCamera.position = nextCamLocation.position;
                            mainCamera.rotation = nextCamLocation.rotation;
                        }
                    }
                }
                else
                {
                    wasBeingLookedAt = false;
                }
            }
            else
            {
                wasBeingLookedAt = false;
            }

            if (!wasBeingLookedAt)
            {
                cam.gameObject.GetComponentInParent<Zoom>().zooming = false;
                zooming = false;
            }
        }

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) && Time.time - justTookPhoto > photoCooldown)
            justTookPhoto = Time.time;
    }

    private IEnumerator TransparencyAnimation()
    {

        coroutineRunning = true;

        yield return new WaitForSeconds(DifficultyManager.Instance.initalDelay);

        float lerpValue = 0;

        while (lerpValue <= maxTransparency)
        {
            lerpValue += Time.deltaTime / DifficultyManager.Instance.animationDuration * maxTransparency;
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].material.color =
                    new Color(meshRenderer[i].material.color.r, meshRenderer[i].material.color.g, meshRenderer[i].material.color.b, lerpValue);

            }
            yield return null;
        }

    }

    private IEnumerator Popped()
    {
        float lerpValue = 0;
        Vector3 scale = Vector3.zero;
        Vector3 finalScale = transform.localScale;

        while (lerpValue <= 1)
        {
            lerpValue += Time.deltaTime / popDuration;
            transform.localScale = Vector3.Lerp(scale, finalScale, lerpValue);
            yield return null;
        }
    }
}