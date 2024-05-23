using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float zoomScale;
    private float defaultZoom;
    private float currentZoom;

    [SerializeField] private float howFastZoom;
    private float zoomValue;
    private float lerpValue;

    [HideInInspector]public bool zooming;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        defaultZoom = cam.fieldOfView;
        currentZoom = cam.fieldOfView;

        zoomValue = howFastZoom;

        lerpValue = 0;

        zooming = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (zooming)
        {
            howFastZoom = zoomValue;
        }
        else
        { howFastZoom = -zoomValue; }


        ZoomIn();
    }


    private void ZoomIn()
    {

            lerpValue += howFastZoom * Time.deltaTime;
            lerpValue = Mathf.Clamp01(lerpValue);
            currentZoom = Mathf.Lerp(defaultZoom, zoomScale, lerpValue);
            cam.fieldOfView = currentZoom;

    }

    private void ZoomOut()
    {
        if (lerpValue < 1)
        {
            lerpValue += howFastZoom * Time.deltaTime;
            currentZoom = Mathf.Lerp(zoomScale, defaultZoom, lerpValue);
            cam.fieldOfView = currentZoom;
        }
    }
}
