using UnityEngine;
using UnityEngine.Tilemaps; 

public class WolfChase : MonoBehaviour
{
    [Header("Settings")]
    public float normalSpeed = 3f;
    public float enragedSpeed = 5.5f; 
    public float attackRate = 1.0f;
    public int attackDamage = 1;
    
    [Header("Water Settings")]
    public float waterSpeedMultiplier = 0.5f;
    public Tilemap waterTilemap; 

    [Header("References")]
    public PlayerHide playerHide;
    public DamCollecting damTarget;

    private Rigidbody2D rb;
    private Animator animator;
    private float nextAttackTime = 0f;
    private bool isRetreating = false; 

    void Start()
    {
        if (!playerHide) playerHide = FindFirstObjectByType<PlayerHide>();
        if (!damTarget) damTarget = FindFirstObjectByType<DamCollecting>();
        
        if (waterTilemap == null) 
        {
            GameObject waterObj = GameObject.Find("Water"); 
            if (waterObj) waterTilemap = waterObj.GetComponent<Tilemap>();
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.gravityScale = 0f;
    }

    public void Retreat()
    {
        isRetreating = true;
        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;
    }

    void FixedUpdate()
    {
        if (isRetreating)
        {
            HandleRetreat();
            return; 
        }

        if (playerHide == null || damTarget == null) return;

        if (playerHide.hidden)
            MoveToAndAttack(damTarget.transform.position, true);
        else
            MoveToAndAttack(playerHide.transform.position, false);
    }

    void HandleRetreat()
    {
        Vector3 runDirection = (transform.position - playerHide.transform.position).normalized;
        rb.linearVelocity = runDirection * enragedSpeed;

        if(runDirection.x != 0) transform.localScale = new Vector3(runDirection.x < 0 ? -1 : 1, 1, 1);

        float distanceToPlayer = Vector2.Distance(transform.position, playerHide.transform.position);
        if (distanceToPlayer > 25f) Destroy(gameObject);
    }

    void MoveToAndAttack(Vector3 targetPos, bool isAttackingDam)
    {
        float dist = Vector2.Distance(transform.position, targetPos);
        
        // Stop and Attack if close
        if (dist < 1.2f)
        {
            rb.linearVelocity = Vector2.zero;
            if (Time.time >= nextAttackTime)
            {
                Attack(isAttackingDam);
                nextAttackTime = Time.time + attackRate;
            }
        }
        else
        {
            Vector2 dir = (targetPos - transform.position).normalized;
            float currentSpeed = (!isAttackingDam && damTarget.currentDamStrength <= 0) ? enragedSpeed : normalSpeed;
            if (IsOnWater()) currentSpeed *= waterSpeedMultiplier;

            rb.linearVelocity = dir * currentSpeed;
            if(dir.x != 0) transform.localScale = new Vector3(dir.x < 0 ? -1 : 1, 1, 1);
        }
    }

    bool IsOnWater()
    {
        if (waterTilemap == null) return false;
        Vector3Int cellPos = waterTilemap.WorldToCell(transform.position);
        return waterTilemap.HasTile(cellPos);
    }

    void Attack(bool isAttackingDam)
    {
        if(animator) animator.SetTrigger("Attack");

        if (isAttackingDam)
        {
            damTarget.TakeDamage(attackDamage);
        }
        else
        {
            // FIX: Manually kill the player instead of waiting for collision
            if (playerHide != null)
            {
                DieScript playerHealth = playerHide.GetComponent<DieScript>();
                if (playerHealth != null)
                {
                    playerHealth.ActuallyDie = true; // Force the death flag to true
                    playerHealth.SendMessage("Die"); // Execute death
                }
            }
        }
    }
}