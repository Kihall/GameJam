using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;

    [SerializeField] private int health = 100;
    private int healthMax;
    private bool dead = false;

    private void Awake()
    {
        healthMax = health;
    }

    private void Update()
    {
        Debug.Log(health);
    }

    public void Damage(int damageAmount)
    {
        if (IsDead()) return;

        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health == 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;

        if (health > healthMax)
        {
            health = healthMax;
        }

        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
        dead = true;
    }

    public bool IsDead()
    {
        return dead;
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
