using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

public class InCameraCheck : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Transform mainCamera;
    private MeshRenderer[] meshRenderer;
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

    [Header("ANIMATION"), SerializeField] private float animationDuration = 10f;
    [SerializeField] private float maxTransparency = 0.5f;
    [SerializeField] private float initialDelayDuration = 3f;
    private Coroutine coroutine = null;

    private bool coroutineRunning = false;

    void Start()
    {
        meshRenderer = GetComponentsInChildren<MeshRenderer>();

        defaultMaterials = new Material[meshRenderer.Length];

        inventory = FindObjectOfType<Inventory>();

        mainCamera = Camera.main.transform;

        col = GetComponent<Collider>();

        for(int i = 0; i < meshRenderer.Length; i++)
        {
            defaultMaterials[i]= meshRenderer[i].material;
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
        if (!completed)
        {
            CheckIfInbounds();
        }
    }

    private void CheckIfInbounds()
    {
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);

        if ((!GeometryUtility.TestPlanesAABB(cameraFrustum, col.bounds)) || destroyed)
        {
            if (wrongObject)
            {
                for (int i = 0; i < meshRenderer.Length; i++)
                {
                    meshRenderer[i].enabled = false;
                }

                if (coroutineRunning)
                {
                    Debug.Log("coroutineRunning on leave bounds: " + coroutineRunning); 
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
                        for (int i = 0; i < meshRenderer.Length; i++)
                        {
                            meshRenderer[i].enabled = true;
                        }

                        if(!coroutineRunning)
                        {
                            for (int i = 0; i < meshRenderer.Length; i++)
                            {
                                meshRenderer[i].material.color =
                                new Color(meshRenderer[i].material.color.r, meshRenderer[i].material.color.g, meshRenderer[i].material.color.b, 0f);

                            }

                            Debug.Log("coroutineRunning on enter bounds: " + coroutineRunning);

                            coroutine = StartCoroutine(TransparencyAnimation());
                        }

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            if (inventory.GetInventoryList().Contains(objectDescription))
                            {
                                inventory.RemoveObject(objectDescription);
                                for (int i = 0; i < meshRenderer.Length; i++)
                                {
                                    meshRenderer[i].material = defaultMaterials[i];
                                }
                                Instantiate(smokeParticles, transform.position, Quaternion.identity);
                                completed = true;
                            }
                        }
                    }
                    else if (!door)
                    {
                        Debug.Log("zoom because of" + hit.transform.name);
                        cam.gameObject.GetComponentInParent<Zoom>().zooming = true;
                        zooming = true;

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            cam.gameObject.GetComponentInParent<Inventory>().AddObject(objectDescription);
                            cam.gameObject.GetComponentInParent<Zoom>().zooming = false;
                            col.enabled = false;
                            destroyed = true;
                            Instantiate(smokeParticles, transform.position, Quaternion.identity);
                            Destroy(gameObject, 0.1f);
                        }
                    }
                    else if (door)
                    {
                        if (Input.GetKeyDown(KeyCode.E))
                        {
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
    }

    private IEnumerator TransparencyAnimation()
    {

        Debug.Log("start coroutine");

        coroutineRunning = true;

        yield return new WaitForSeconds(initialDelayDuration);

        float lerpValue = 0;

        while (lerpValue <= maxTransparency)
        {
            lerpValue += Time.deltaTime / animationDuration * maxTransparency;
            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i].material.color = 
                    new Color(meshRenderer[i].material.color.r, meshRenderer[i].material.color.g, meshRenderer[i].material.color.b, lerpValue);

            }
              yield return null;
        }



        Debug.Log("stop coroutine");
    }
}