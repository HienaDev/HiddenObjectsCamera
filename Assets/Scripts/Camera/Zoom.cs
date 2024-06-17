using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] private float zoomScale;
    [SerializeField] private float howFastZoom;

    private Camera cam;

    private float defaultZoom;
    private float currentZoom;

    private float zoomValue;
    private float lerpValue;

    [HideInInspector]public bool zooming;

    void Start()
    {
        cam = GetComponent<Camera>();
        defaultZoom = cam.fieldOfView;
        currentZoom = cam.fieldOfView;

        zoomValue = howFastZoom;

        lerpValue = 0;

        zooming = false;
    }

    void Update()
    {
        if (zooming)
        {
            howFastZoom = zoomValue;
        }
        else
        { 
            howFastZoom = -zoomValue; 
        }

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
