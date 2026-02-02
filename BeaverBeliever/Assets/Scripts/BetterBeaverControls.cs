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
        bool onWater = IsOnWater();

        if (onWater)
        {
            HandleWaterMovement();
            Debug.Log("waters");

        }
        else
        {
            Debug.Log("Groud");
            HandleGroundMovement();
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

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //void FixedUpdate()
    //{
    //    bool onWater = IsOnWater();


    //    float horizotnalVelocity = rb.linearVelocity.x;
    //    float verticalVelocity = rb.linearVelocity.y;

    //    if (onWater)
    //    {


    //        if ((Keyboard.current.aKey.isPressed && Keyboard.current.dKey.isPressed) || (!Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed))
    //        {

    //            if (horizotnalVelocity != 0)
    //            {
    //                horizotnalVelocity += -horizontal * waterDeccel * Time.deltaTime;
    //            }
    //            if (horizontal == -1f)
    //            {
    //                if (horizotnalVelocity > 0f) horizotnalVelocity = 0f;

    //            }
    //            else
    //            {
    //                if (horizotnalVelocity < 0f) horizotnalVelocity = 0f;
    //            }
    //        }
    //        else
    //        {
    //            if (Keyboard.current.aKey.isPressed)
    //                horizontal = -1f;
    //            else if (Keyboard.current.dKey.isPressed)
    //                horizontal = 1f;

    //            horizotnalVelocity += horizontal * waterAccel * Time.deltaTime;
    //            horizotnalVelocity = Mathf.Clamp(horizotnalVelocity, -waterSpeed, waterSpeed);
    //        }

    //        if ((Keyboard.current.wKey.isPressed && Keyboard.current.sKey.isPressed) || (!Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed))
    //        {

    //            if (verticalVelocity != 0)
    //            {
    //                verticalVelocity += -vertical * waterDeccel * Time.deltaTime;
    //            }
    //            if (vertical == -1f)
    //            {
    //                if (verticalVelocity > 0f) verticalVelocity = 0f;

    //            }
    //            else
    //            {
    //                if (verticalVelocity < 0f) verticalVelocity = 0f;
    //            }
    //        }
    //        else
    //        {
    //            if (Keyboard.current.wKey.isPressed)
    //                vertical = -1f;
    //            else if (Keyboard.current.sKey.isPressed)
    //                vertical = 1f;

    //            verticalVelocity += vertical * waterAccel * Time.deltaTime;
    //            verticalVelocity = Mathf.Clamp(verticalVelocity, -waterSpeed, waterSpeed);
    //        }
    //    }
    //    else
    //    {
    //        if (Keyboard.current.wKey.isPressed && !Keyboard.current.sKey.isPressed)
    //        {
    //            movement.y = -landSpeed;
    //        } else if (Keyboard.current.sKey.isPressed && !Keyboard.current.wKey.isPressed)
    //        {
    //            movement.y = landSpeed;

    //        }

    //        if (Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed)
    //        {
    //            movement.x = -landSpeed;
    //        }
    //        else if (Keyboard.current.dKey.isPressed && !Keyboard.current.aKey.isPressed)
    //        {
    //            movement.x = landSpeed;

    //        }

    //        rb.MovePosition(rb.position + movement * waterSpeed * Time.fixedDeltaTime);


    //    }


    //    //rb.linearVelocity = new Vector2(horizotnalVelocity, verticalVelocity);


    //}



    bool IsOnWater()
    {
        Vector3 worldPos = transform.position;
        Vector3Int cellPos = waterTilemap.WorldToCell(worldPos);

        return waterTilemap.HasTile(cellPos);
    }
}
