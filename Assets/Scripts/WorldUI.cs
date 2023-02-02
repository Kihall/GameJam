using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnDead += HealthSystem_OnDead;

        UpdateHealthBar();
    }

    private void Update()
    {
        if (healthSystem.transform.localScale.x == 1)
        {
            healthBarImage.fillOrigin = 0;
        }
        if (healthSystem.transform.localScale.x == -1)
        {
            healthBarImage.fillOrigin = 1;
        }
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void HealthSystem_OnHealed(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }
}
