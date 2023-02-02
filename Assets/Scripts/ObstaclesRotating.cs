using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesRotating : MonoBehaviour
{
    [SerializeField] private float turningSpeed = 30f;


    private void Update()
    {
        transform.Rotate(Vector3.forward, turningSpeed * Time.deltaTime);
    }
}
