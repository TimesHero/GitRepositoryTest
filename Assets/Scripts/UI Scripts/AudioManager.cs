using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioMixer audioMixer; 

    private AudioSource audioSource;
    public AudioMixerGroup sfxGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void PlaySound(AudioClip SFX)
    {
        audioSource.outputAudioMixerGroup = sfxGroup;
        if (SFX != null)
        {
            audioSource.PlayOneShot(SFX);
        }
        else
        {
            Debug.LogWarning("Sound not found: ");
        }
    }
}
