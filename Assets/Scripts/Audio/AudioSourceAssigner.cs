using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceAssigner : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerGroup;

    private void Start()
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in audioSources)
        {
            source.outputAudioMixerGroup = mixerGroup;
        }

        
        // Mostra o nome de todos os AudioSources que foram alterados
        foreach (AudioSource source in audioSources)
        {
            Debug.Log(source.name);
        }
    }
}
