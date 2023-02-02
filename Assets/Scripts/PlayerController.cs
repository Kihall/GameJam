using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public event EventHandler<bool> OnDash;
    public event EventHandler OnAttack;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] private int playerDamage = 10;
    [SerializeField] private float dashAmount = 5f;
    [SerializeField] private float climbSpeed = 5f;

    [SerializeField] private float canNotDashForSecondsTimer = 1f;
    [SerializeField] private float attackTimerMax = 0.8f;
    [SerializeField] private float enemyDistance = 1f;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask climbableWallLayerMask;
    [SerializeField] private LayerMask enemyLayerMask;

    [SerializeField] private AIController enemy;

    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;

    private Vector3 moveDir;
    private Vector3 climbDir;
    private bool alreadyDashed = false;
    private bool alreadyAttacked = false;
    private float canNotDashForSeconds = 0f;
    private bool jumpedTwice;
    private float attackTimer = 0f;
    private float doubleJumpTimer = 0f;
    private float dashTimer = 0f;
    private float climbTimer = 0f;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Timers();

        if (attackTimer > 0) return;

        HandleMovement();

        HandleJump();

        HandleAttack();

        if (canNotDashForSeconds == 0)
        {
            if (dashTimer > 0)
            {
                Dash();
                Debug.Log("Dash!");
            }
        }
        if (doubleJumpTimer > 0)
        {
            DoubleJump();
            Debug.Log("DoubleJump!");
        }
        if (climbTimer > 0)
        {
            Climb();
            Debug.Log("Climb!");
        }
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rigidbody2D.velocity = new Vector2(-moveSpeed, rigidbody2D.velocity.y);
            moveDir = new Vector3(-moveSpeed, 0, 0).normalized;
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                rigidbody2D.velocity = new Vector2(+moveSpeed, rigidbody2D.velocity.y);
                moveDir = new Vector3(+moveSpeed, 0, 0).normalized;
            }
            else
            {
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            }
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidbody2D.velocity = Vector2.up * jumpVelocity;
            jumpedTwice = false;
        }
    }

    private void HandleAttack()
    {
        if (attackTimer == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                alreadyAttacked = true;
                attackTimer = attackTimerMax;
                OnAttack?.Invoke(this, EventArgs.Empty);
                if (moveDir.x < 0)
                {
                    if (LookForTargetLeft(out RaycastHit2D enemyRaycastHitLeft))
                    {
                        enemyRaycastHitLeft.collider.GetComponent<HealthSystem>().Damage(playerDamage);
                    }
                }
                if (moveDir.x > 0)
                {
                    if (LookForTargetRight(out RaycastHit2D enemyRaycastHitRight))
                    {
                        enemyRaycastHitRight.collider.GetComponent<HealthSystem>().Damage(playerDamage);
                    }
                }
            }
        }
    }

    private void DoubleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !jumpedTwice && !IsGrounded())
        {
            rigidbody2D.velocity = Vector2.up * jumpVelocity;
            jumpedTwice = true;
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rigidbody2D.MovePosition(transform.position + moveDir * dashAmount);
            alreadyDashed = true;
            canNotDashForSeconds = canNotDashForSecondsTimer;
            OnDash?.Invoke(this, alreadyDashed);
        }
    }

    private void Climb()
    {
        if (IsCloseToClimbableWallLeft() || IsCloseToClimbableWallRight())
        {
            rigidbody2D.velocity = new Vector3(rigidbody2D.velocity.x, 0, 0);
            if (Input.GetKey(KeyCode.W))
            {
                rigidbody2D.velocity = new Vector2(0, +climbSpeed);
            }
            else
            {
                if (Input.GetKey(KeyCode.S))
                {
                    rigidbody2D.velocity = new Vector2(0, -climbSpeed);
                }
            }
        }
    }

    private void Timers()
    {
        doubleJumpTimer -= Time.deltaTime;
        dashTimer -= Time.deltaTime;
        climbTimer -= Time.deltaTime;

        attackTimer -= Time.deltaTime;
        canNotDashForSeconds -= Time.deltaTime;

        if (doubleJumpTimer < 0)
        {
            doubleJumpTimer = 0;
        }
        if (dashTimer < 0)
        {
            dashTimer = 0;
        }
        if (climbTimer < 0)
        {
            climbTimer = 0;
        }

        if (attackTimer < 0)
        {
            attackTimer = 0;
            alreadyAttacked = false;
        }
        if (canNotDashForSeconds < 0)
        {
            canNotDashForSeconds = 0;
            alreadyDashed = false;
        }
    }

    private bool LookForTargetLeft(out RaycastHit2D enemyRaycastHitLeft)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(
            boxCollider2D.bounds.center, Vector2.left, boxCollider2D.bounds.extents.x + enemyDistance, enemyLayerMask);
        enemyRaycastHitLeft = raycastHit2D;
        return raycastHit2D.collider != null;
    }

    private bool LookForTargetRight(out RaycastHit2D enemyRaycastHitRight)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(
            boxCollider2D.bounds.center, Vector2.right, boxCollider2D.bounds.extents.x + enemyDistance, enemyLayerMask);
        enemyRaycastHitRight = raycastHit2D;
        return raycastHit2D.collider != null;
    }

    public bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(
            boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.1f, groundLayerMask);
        return raycastHit2D.collider != null;
    }

    public bool IsCloseToClimbableWallLeft()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(
            boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.left, 0.1f, climbableWallLayerMask);
        return raycastHit2D.collider != null;
    }

    public bool IsCloseToClimbableWallRight()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(
            boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.right, 0.1f, climbableWallLayerMask);
        return raycastHit2D.collider != null;
    }

    public Rigidbody2D GetRigidbody2D()
    {
        return rigidbody2D;
    }

    public void SetDoubleJumpTimer(float doubleJumpTimerMax)
    {
        doubleJumpTimer = doubleJumpTimerMax;
    }

    public void SetDashTimer(float dashTimerMax)
    {
        dashTimer = dashTimerMax;
    }

    public void SetClimbTimer(float climbTimerMax)
    {
        climbTimer = climbTimerMax;
    }

    public float GetClimbTimer()
    {
        return climbTimer;
    }

    public Vector3 GetMoveDir()
    {
        return moveDir;
    }
    public bool GetAlreadyAttacked()
    {
        return alreadyAttacked;
    }
}
