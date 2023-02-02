using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDamage : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            HealthSystem healthSystem = playerController.GetComponent<HealthSystem>();
            healthSystem.Damage(damageAmount);
        }
    }
}
