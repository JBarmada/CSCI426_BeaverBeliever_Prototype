using UnityEngine;
using UnityEngine.Tilemaps;

public class WolfChase : MonoBehaviour
{
    public float moveSpeed = 3f;

    public Tilemap groundTilemap;
    public Tilemap waterTilemap;

    public PlayerHide playerhide;

    Transform player;
    Rigidbody2D rb;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Beaver");
        if (p != null)
            player = p.transform;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        transform.parent = null;
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (player.position - transform.position);
        direction = direction.normalized;

        TryMove(direction);
    }

    void TryMove(Vector2 dir)
    {

        if (playerhide.hidden)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            Vector2 nextPos = rb.position + dir * moveSpeed * Time.fixedDeltaTime;

            if (!IsWater(nextPos))
            {
                rb.linearVelocity = dir * moveSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
       

    bool IsWater(Vector2 worldPos)
    {   
        Vector3Int cellPos = waterTilemap.WorldToCell(worldPos);
        return waterTilemap.HasTile(cellPos);
    }
}
