using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPickup : MonoBehaviour
{

    [SerializeField] private bool isDash;
    [SerializeField] private bool isDoubleJump;
    [SerializeField] private bool isClimb;
    [SerializeField] private bool isRestoreHealth;

    [SerializeField] private float doubleJumpTimer = 5f;
    [SerializeField] private float dashTimer = 5f;
    [SerializeField] private float climbTimer = 5f;
    [SerializeField] private int healAmount = 100;
    [SerializeField] private float respawnTime = 5f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other.gameObject);
        }
    }

    private void Pickup(GameObject subject)
    {
        if (isDoubleJump)
        {
            subject.GetComponent<PlayerController>().SetDoubleJumpTimer(doubleJumpTimer);
        }
        if (isDash)
        {
            subject.GetComponent<PlayerController>().SetDashTimer(dashTimer);
        }
        if (isClimb)
        {
            subject.GetComponent<PlayerController>().SetClimbTimer(climbTimer);
        }
        if (isRestoreHealth)
        {
            subject.GetComponent<HealthSystem>().Heal(healAmount);
        }
        StartCoroutine(HideForSeconds(respawnTime));
    }

    private IEnumerator HideForSeconds(float seconds)
    {
        ShowPickup(false);
        yield return new WaitForSeconds(seconds);
        ShowPickup(true);
    }

    private void ShowPickup(bool shouldShow)
    {
        GetComponent<Collider2D>().enabled = shouldShow;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(shouldShow);
        }
    }
}
