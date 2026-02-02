using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WolfChase : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpDistance = 2f;
    public float jumpCooldown = 2f;
    public Tilemap groundTilemap;
    public Tilemap waterTilemap;



    private Transform player;
    private Rigidbody2D rb;
    private float lastJumpTime;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Beaver").transform;
        rb = GetComponent<Rigidbody2D>();
        lastJumpTime = Time.time;

    }

    void FixedUpdate()
    {
        if (!player) return;

        //Vector2 direction = (player.position - transform.position).normalized;
        //rb.linearVelocity = direction * moveSpeed;

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

        // Only move if tile exists on Ground layer
        //if (groundTilemap.HasTile(cell))
        //{
        //    rb.MovePosition(targetPos);
        //}
        //else
        //{
        //    rb.linearVelocity = Vector2.zero;
        //}



    }

    void Update()
    {
        //TryJump();

    }


    void TryJump()
    {


        if (Time.time < lastJumpTime + jumpCooldown)
            return;




        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


        //if (distanceToPlayer > 1.5f && distanceToPlayer < jumpDistance)
        //{
        //    Vector2 jumpDir = (player.position - transform.position).normalized;
        //    rb.AddForce(jumpDir * 5f, ForceMode2D.Impulse);
        //    lastJumpTime = Time.time;
        //    JumpVisual();
        //    Debug.Log("Jump");
        //}
    }


    //IEnumerator JumpVisual()
    //{
    //    //Vector3 originalScale = transform.localScale;
    //    //transform.localScale = originalScale * 5f;
    //    //yield return new WaitForSeconds(0.15f);
    //    //transform.localScale = originalScale;
    //}




}
