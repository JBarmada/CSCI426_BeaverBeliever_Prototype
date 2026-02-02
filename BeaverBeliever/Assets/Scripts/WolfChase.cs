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

        transform.parent = null;

    }


    void FixedUpdate()
    {
        Vector3 direction = (player.position - transform.position);
       // direction = GetCardinalDirection(direction);
       direction = direction.normalized;


        Vector3 worldPos = transform.position;
        worldPos.z = 0;
        Vector3Int cellPos = waterTilemap.WorldToCell(worldPos);

        Debug.Log(" X: " + cellPos.x + " Y: " + cellPos.y);

        if (waterTilemap.HasTile(cellPos))
        {
            Debug.Log("current on water");
        }
        TryMove(direction);
    }

    // Restrict to up/down/left/right
    Vector2 GetCardinalDirection(Vector2 dir)
    {
        return Mathf.Abs(dir.x) > Mathf.Abs(dir.y)
            ? new Vector2(Mathf.Sign(dir.x), 0)
            : new Vector2(0, Mathf.Sign(dir.y));
    }


    void TryMove(Vector3 dir)
    {
        // Vector3Int nextCell = WorldToCell(transform.position + (Vector3)dir);

        Vector3 newPos = transform.position + dir * moveSpeed * Time.deltaTime;
        // Ground → move normally

        //Debug.Log(" X: " + newPos.x + " Y: " + newPos.y);


        



        if (!IsWater(newPos))
        {
            rb.linearVelocity = dir * moveSpeed;
            return;
        }
        else
        {
            Debug.Log("water");

            //TryJumpOverWater(dir);
            //return;

        }

        // Blocked
        rb.linearVelocity = Vector2.zero;
    }

    void TryJumpOverWater(Vector2 dir)
    {
        // Check up to 2 water tiles ahead
        for (int i = 1; i <= 2; i++)
        {
            Vector3Int checkCell = WorldToCell(
                transform.position + (Vector3)(dir * i)
            );

            //if (IsWater(checkCell))
            //    continue;

            if (!IsWater(checkCell))
            {
                // Jump destination
                Vector2 jumpTarget = CellToWorldCenter(checkCell);
                Vector2 jumpDir = (jumpTarget - rb.position).normalized;

                rb.linearVelocity = jumpDir * jumpSpeed;
                return;
            }

            break;
        }

        // Can't jump
        rb.linearVelocity = Vector2.zero;
    }

   

    bool IsWater(Vector3 pos)
    {

        Vector3Int cellPos = waterTilemap.WorldToCell(pos);
        return waterTilemap.HasTile(cellPos);
        //return waterTilemap.HasTile(cell);
    }

    Vector3Int WorldToCell(Vector3 worldPos)
    {
        return groundTilemap.WorldToCell(worldPos);
    }

    Vector2 CellToWorldCenter(Vector3Int cell)
    {
        return groundTilemap.GetCellCenterWorld(cell);
    }






    //void FixedUpdate()
    //{
    //    if (!player) return;



    //    Vector2 direction = (player.position - transform.position).normalized;
    //    Vector2 targetPos = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

    //    Vector3Int cell = waterTilemap.WorldToCell(targetPos);


    //    if (!waterTilemap.HasTile(cell))
    //    {
    //        rb.MovePosition(targetPos);

    //    }
    //    else
    //    {
    //        rb.linearVelocity = Vector2.zero;
    //    }





    //}

    void Update()
    {

    }


    void TryJump()
    {


       


     
    }


   




}
