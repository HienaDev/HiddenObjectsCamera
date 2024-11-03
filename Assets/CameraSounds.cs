using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSounds : MonoBehaviour
{

    [SerializeField] private AudioClip[] placeObjectSounds;
    private AudioSource placeObjectSource;
    [SerializeField] private float placeObjectVolume;

    [SerializeField] private AudioClip[] collectObjectSounds;
    private AudioSource collectObjectSoundsSource;
    [SerializeField] private float collectObjectVolume;

    // Start is called before the first frame update
    void Start()
    {
        placeObjectSource = gameObject.AddComponent<AudioSource>();
        placeObjectSource.volume = placeObjectVolume;
        //placeObjectSource


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
