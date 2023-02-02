using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToSecondScene : MonoBehaviour
{
    private AudioSource audioSource;

    private float laughterTimer = 0.5f;
    private float laughterTimerMax = 6f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (laughterTimer > 1)
        {
            laughterTimer -= Time.deltaTime;
        }
        else
        {
            return;
        }

        if (laughterTimer < 1.2f)
        {
            laughterTimer = 1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        audioSource.Play();
        laughterTimer = laughterTimerMax;
    }
}
