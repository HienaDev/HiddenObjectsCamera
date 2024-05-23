using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InCameraCheck : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private MeshRenderer meshRenderer;
    private Color defaultColor;
    private Plane[] cameraFrustum;
    private Collider col;
    private List<Material> objectMaterials;
    [SerializeField] private Material missingObjectMaterial;
    private Material[] missingObjectArray;

    [SerializeField] private bool wrongObject = false;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponent<Collider>();
            
        defaultColor = meshRenderer.material.color;

        objectMaterials = new List<Material>();
        

        foreach (Material mat in meshRenderer.materials)
        {
            objectMaterials.Add(mat);
        }

        missingObjectArray = new Material[meshRenderer.materials.Length];

        for (int i = 0; i < meshRenderer.materials.Length; i++)
        {
            missingObjectArray[i] = missingObjectMaterial;
        }

        if(wrongObject)
            meshRenderer.materials = missingObjectArray;

    }



    private void FixedUpdate()
    {

            CheckIfInbounds();

    }

    private void CheckIfInbounds()
    {
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);

        if(wrongObject)
        {
            if (GeometryUtility.TestPlanesAABB(cameraFrustum, col.bounds))
            {
                //meshRenderer.materials = missingObjectAr
                Debug.Log("zoom");
                cam.gameObject.GetComponentInParent<Zoom>().zooming = true;
            }
            else
            {
                //meshRenderer.materials = objectMaterials.ToArray();
                
                cam.gameObject.GetComponentInParent<Zoom>().zooming = false;
            }
        }
        
    }
}
