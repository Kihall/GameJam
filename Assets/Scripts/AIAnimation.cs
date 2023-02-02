using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimation : MonoBehaviour
{
    [SerializeField] private AIController aiController;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        aiController.GetComponent<HealthSystem>().OnDamaged += HealthSystem_OnDamaged;
        aiController.GetComponent<HealthSystem>().OnDead += HealthSystem_OnDead;

        aiController.OnAttack += AIController_OnAttack;
    }

    private void Update()
    {
        MovementAnimation();
    }

    private void AttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    private void MovementAnimation()
    {
        animator.SetFloat("Speed", Mathf.Abs(aiController.GetHorizontalVelocity()));

        if (aiController.GetHorizontalVelocity() > 0)
        {
            aiController.transform.localScale = new Vector3(1, 1, 1);
        }
        if (aiController.GetHorizontalVelocity() < 0)
        {
            aiController.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void AIController_OnAttack(object sender, EventArgs e)
    {
        AttackAnimation();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        animator.SetTrigger("Hurt");
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        animator.SetBool("death", true);
        aiController.GetComponent<BoxCollider2D>().enabled = false;
        aiController.GetComponent<Rigidbody2D>().gravityScale = 0;
    }
}
