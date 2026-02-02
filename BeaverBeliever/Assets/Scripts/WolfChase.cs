using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WolfChase : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpSpeed = 6f;

    public Tilemap groundTilemap;
    public Tilemap waterTilemap;



    private Transform player;
    private Rigidbody2D rb;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Beaver").transform;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;


    }

    void FixedUpdate()
    {
        if (!player) return;

    

        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 targetPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

        Vector3Int cell = waterTilemap.WorldToCell(targetPos);


        if (!waterTilemap.HasTile(cell))
        {
            rb.MovePosition(targetPos);

        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

   



    }

    void Update()
    {

    }


    void TryJump()
    {


        if (Time.time < lastJumpTime + jumpCooldown)
            return;




        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


     
    }


   




}
