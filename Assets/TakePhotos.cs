using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePhotos : MonoBehaviour
{
    private RenderTextureCapture rtc;
    [SerializeField] private float photoCooldown = 1f;
    public float PhotoCooldown { get { return photoCooldown; } }
    private float justTookPhoto;
    // Start is called before the first frame update
    void Start()
    {
        rtc = FindAnyObjectByType<RenderTextureCapture>();
        justTookPhoto = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.time - justTookPhoto > photoCooldown)
        {
            justTookPhoto = Time.time;
            rtc.ExportPhoto("");
        }
    }
}
