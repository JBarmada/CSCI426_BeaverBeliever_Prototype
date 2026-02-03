using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.Windows;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class BetterBeaverControls : MonoBehaviour
{
    public float landSpeed = 3f;
    public float waterSpeed = 5f;


    public Tilemap waterTilemap;

    private Rigidbody2D rb;



    public float waterAcceleration = 5f;
    public float waterDrag = 1.5f;


    private Vector2 velocity;
    private Vector2 input;



    Animator animator;
    SpriteRenderer sr;
    const string IDLE = "Beaver_Idle";
    const string WALK_SIDE = "Beaver_Walk_Side";
    const string WALK_UP = "Beaver_Walk_Up";
    string currentState;

    public bool beaverMove = true;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        velocity = new Vector2(0f, 0f);
        input = new Vector2(0f, 0f);

        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();


    }


    void Update()
    {
        input = new Vector2(0f, 0f);

        if (Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed)
        {
            input.y = 1;
        }
        else if (Keyboard.current.sKey.isPressed && !Keyboard.current.wKey.isPressed)
        {
            input.y = -1;

        }

        if (Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed)
        {
            input.x = -1;
        }
        else if (Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed)
        {
            input.x = 1;

        }

        if(input != Vector2.zero)
            input = input.normalized;

        UpdateAnimationAndFacing();


    }

    void FixedUpdate()
    {

        if (beaverMove)
        {
            bool onWater = IsOnWater();

            if (onWater)
            {
                HandleWaterMovement();

            }
            else
            {
                HandleGroundMovement();
            }

        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

      
    }


    void UpdateAnimationAndFacing()
    {
        // IDLE
        if (input == Vector2.zero)
        {
            ChangeState(IDLE);

            // IMPORTANT: reset flips on idle
            sr.flipY = false;

            return;
        }

        // Decide dominant direction
        if (Mathf.Abs(input.x) >= Mathf.Abs(input.y))
        {
            // SIDE WALK
            ChangeState(WALK_SIDE);
            sr.flipX = input.x < 0;
            sr.flipY = false;
        }
        else
        {
            // UP / DOWN (reuse up)
            ChangeState(WALK_UP);
            sr.flipX = false;
            sr.flipY = input.y < 0; // down = flipped
        }
    }

    void HandleGroundMovement()
    {
        rb.MovePosition(rb.position + input * landSpeed * Time.fixedDeltaTime);


    }

    void HandleWaterMovement()
    {

        // Gradually build momentum
        velocity = rb.linearVelocity;
        velocity += input * waterAcceleration;
        velocity = Vector2.ClampMagnitude(velocity, waterSpeed);

        velocity *= 1f - waterDrag * Time.deltaTime;

        rb.linearVelocity = velocity;
    }

    void ChangeState(string newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        animator.Play(newState);
    }




    bool IsOnWater()
    {
        Vector3 worldPos = transform.position;
        Vector3Int cellPos = waterTilemap.WorldToCell(worldPos);

        return waterTilemap.HasTile(cellPos);
    }
}
