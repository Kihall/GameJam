using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private AudioSource audioSource;
    private float volume = 0.5f;

    private void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void IncreaseVolume()
    {
        volume += 0.1f;
        volume = Mathf.Clamp01(volume);
    }

    public void DecreaseVolume()
    {
        volume -= 0.1f;
        volume = Mathf.Clamp01(volume);
    }
}
