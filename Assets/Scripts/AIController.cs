using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public event EventHandler OnAttack;

    [SerializeField] private float attackTimerMax = 2f;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private int aIDamage = 10;
    [SerializeField] private float aggroDistance = 5f;

    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask climbableWallLayerMask;
    [SerializeField] private LayerMask playerLayerMask;

    private GameObject player;
    private HealthSystem healthSystem;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2D;


    private Vector3 targetPosition;
    private Vector3 moveDir;
    private float patrolTurnTimerMax = 1f;
    private float patrolTurnTimer = 0f;
    private float attackTimer = 0f;


    private void Awake()
    {
        player = GameObject.FindWithTag("Player");

        healthSystem = GetComponent<HealthSystem>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        moveDir = new Vector3(+patrolSpeed, 0, 0);
    }

    private void Update()
    {
        if (healthSystem.IsDead()) return;

        attackTimer -= Time.deltaTime;
        if (attackTimer < 0)
        {
            attackTimer = 0;
        }

        patrolTurnTimer -= Time.deltaTime;
        if (patrolTurnTimer < 0)
        {
            patrolTurnTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (healthSystem.IsDead()) return;

        if (attackTimer == 0)
        {
            HandleAttack();
        }
        else
        {
            return;
        }

        if (OnTheEdge() && DistanceToPlayer() < aggroDistance)
        {
            return;
        }

        Movement();

        HandlePatrol();
    }

    private void Movement()
    {
        if (DistanceToPlayer() < aggroDistance)
        {
            Vector3 moveDirToPlayer = (player.transform.position - transform.position).normalized;
            Vector3 moveDirVector3 = moveDirToPlayer * patrolSpeed;
            moveDir.x = moveDirVector3.x;
        }

        rigidbody2D.velocity = moveDir;
    }

    private void HandlePatrol()
    {
        if (patrolTurnTimer == 0)
        {
            if (!IsGrounded() || OnTheEdge() || HitTheLeftWall() || HitTheRightWall())
            {
                moveDir *= -1;
                patrolTurnTimer = patrolTurnTimerMax;
            }
        }
    }

    private void HandleAttack()
    {
        if (moveDir.x < 0)
        {
            if (LookForTargetLeft(out RaycastHit2D playerRaycastHitLeft))
            {
                OnAttack?.Invoke(this, EventArgs.Empty);
                attackTimer = attackTimerMax;
                playerRaycastHitLeft.collider.GetComponent<HealthSystem>().Damage(aIDamage);
            }
        }
        if (moveDir.x > 0)
        {
            if (LookForTargetRight(out RaycastHit2D playerRaycastHitRight))
            {
                OnAttack?.Invoke(this, EventArgs.Empty);
                attackTimer = attackTimerMax;
                playerRaycastHitRight.collider.GetComponent<HealthSystem>().Damage(aIDamage);
            }
        }
    }

    private float DistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        return distanceToPlayer;
    }

    private bool LookForTargetLeft(out RaycastHit2D playerRaycastHitLeft)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(
            boxCollider2D.bounds.center, Vector2.left, boxCollider2D.bounds.extents.x + 1f, playerLayerMask);
        playerRaycastHitLeft = raycastHit2D;
        return raycastHit2D.collider != null;
    }

    private bool LookForTargetRight(out RaycastHit2D playerRaycastHitRight)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(
            boxCollider2D.bounds.center, Vector2.right, boxCollider2D.bounds.extents.x + 1f, playerLayerMask);
        playerRaycastHitRight = raycastHit2D;
        return raycastHit2D.collider != null;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(
            boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + 0.1f, groundLayerMask);
        return raycastHit2D.collider != null;
    }

    private bool OnTheEdge()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(
            boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, 0.5f, climbableWallLayerMask);
        return raycastHit2D.collider != null;
    }

    private bool HitTheLeftWall()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(
            boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.left, 1f, climbableWallLayerMask);
        return raycastHit2D.collider != null;
    }

    private bool HitTheRightWall()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(
            boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.right, 1f, climbableWallLayerMask);
        return raycastHit2D.collider != null;
    }

    public float GetHorizontalVelocity()
    {
        return rigidbody2D.velocity.x;
    }
}
