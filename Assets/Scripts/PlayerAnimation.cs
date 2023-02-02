using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private Animator animator;

    private float dashAnimationTimerMax = 0.3f;
    private float dashAnimationTimer = 0;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerController.OnDash += PlayerController_OnDashChanged;
        playerController.OnAttack += PlayerController_OnAttack;

        playerController.GetComponent<HealthSystem>().OnDamaged += HealthSystem_OnDamaged;
        playerController.GetComponent<HealthSystem>().OnDead += HealthSystem_OnDead;
    }

    private void Update()
    {
        DashAnimation();

        RunningAnimation();

        AttackAnimation();

        JumpingAnimation();

        ClimbAnimation();
    }

    private void RunningAnimation()
    {
        if (playerController.IsGrounded())
        {
            animator.SetBool("falling", false);
            if (playerController.GetRigidbody2D().velocity.x == 0)
            {
                animator.SetBool("run", false);
            }
            if (playerController.GetRigidbody2D().velocity.x > 0 && playerController.GetMoveDir().x > 0)
            {
                animator.SetBool("run", true);
                playerController.transform.localScale = new Vector3(1, 1, 1);
            }
            if (playerController.GetRigidbody2D().velocity.x < 0 && playerController.GetMoveDir().x < 0)
            {
                animator.SetBool("run", true);
                playerController.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void JumpingAnimation()
    {
        if (!playerController.IsGrounded())
        {
            animator.SetFloat("verticalVelocity", playerController.GetRigidbody2D().velocity.y);
            if (playerController.GetRigidbody2D().velocity.y < -0.1f)
            {
                animator.SetBool("falling", true);
            }
        }
    }

    private void ClimbAnimation()
    {
        if (playerController.IsCloseToClimbableWallLeft() || playerController.IsCloseToClimbableWallRight())
        {
            if (!playerController.IsGrounded() && playerController.GetClimbTimer() != 0)
            {
                animator.SetBool("climb", true);
            }
        }
        else
        {
            animator.SetBool("climb", false);
        }
    }

    private void DashAnimation()
    {
        dashAnimationTimer -= Time.deltaTime;
        if (dashAnimationTimer < 0)
        {
            dashAnimationTimer = 0;
            animator.SetBool("dash", false);
        }
    }

    private void AttackAnimation()
    {
        if (!playerController.GetAlreadyAttacked())
        {
            animator.SetBool("attack", false);
        }
    }

    private void PlayerController_OnAttack(object sender, EventArgs e)
    {
        animator.SetBool("attack", true);
    }

    private void PlayerController_OnDashChanged(object sender, bool alreadyDashed)
    {
        if (alreadyDashed)
        {
            animator.SetBool("dash", true);
            dashAnimationTimer = dashAnimationTimerMax;
        }
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        animator.SetTrigger("hurt");
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        animator.SetTrigger("death");
    }
}
