using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class BeaverMovement : MonoBehaviour
{
    public float moveSpeed = 3f;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;
    Vector2 movement;

    string currentState;

    const string IDLE = "Beaver_Idle";
    const string WALK_SIDE = "Beaver_Walk_Side";
    const string WALK_UP = "Beaver_Walk_Up";

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
            movement = movement.normalized;

        UpdateAnimationAndFacing();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);


    }

    void UpdateAnimationAndFacing()
    {
        // IDLE
        if (movement == Vector2.zero)
        {
            ChangeState(IDLE);

            // IMPORTANT: reset flips on idle
            sr.flipY = false;

            return;
        }

        // Decide dominant direction
        if (Mathf.Abs(movement.x) >= Mathf.Abs(movement.y))
        {
            // SIDE WALK
            ChangeState(WALK_SIDE);
            sr.flipX = movement.x < 0;
            sr.flipY = false;
        }
        else
        {
            // UP / DOWN (reuse up)
            ChangeState(WALK_UP);
            sr.flipX = false;
            sr.flipY = movement.y < 0; // down = flipped
        }
    }

    void ChangeState(string newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        animator.Play(newState);
    }
}
