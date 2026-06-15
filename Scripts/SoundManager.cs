using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip lightCough;   // 軽い咳
    public AudioClip mediumCough;  // 中程度の咳
    public AudioClip heavyCough;   // 激しい咳

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCough(int durability)
    {
        if (durability >= 3)
        {
            audioSource.PlayOneShot(heavyCough);
        }
        else if (durability == 2)
        {
            audioSource.PlayOneShot(mediumCough);
        }
        else
        {
            audioSource.PlayOneShot(lightCough);
        }
    }
}