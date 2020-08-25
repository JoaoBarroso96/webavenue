using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayClip(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/SFX/" + name);
        audioSource.PlayOneShot(clip);
    }
}
