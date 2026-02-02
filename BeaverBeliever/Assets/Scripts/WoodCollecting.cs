using UnityEngine;
using UnityEngine.Tilemaps;

public class WoodCollecting : MonoBehaviour
{
    public GameObject spritePrefab;   // Prefab with SpriteRenderer
    private bool touchingTree = false;
    public string tree = "Trunk";
    public Vector3 carryOffset = new Vector3(0f, 1f, 0f);
    public Vector3 carriedScale = new Vector3(0.5f, 0.5f, 1f);
    public Vector3 droppedScale = new Vector3(1f, 1f, 1f);



    private GameObject carriedWood = null;



    Collision2D currentCollision;

    void Update()
    {

        if (touchingTree && Input.GetMouseButtonDown(0) && carriedWood == null)
        {
            Vector3 spawnPos = currentCollision.transform.position;

            Transform parent = currentCollision.transform.parent;
            if (parent != null)
            {
                Destroy(parent.gameObject);
            }

            //Destroy(currentCollision.gameObject);

            carriedWood = Instantiate(spritePrefab, transform);
            carriedWood.transform.localPosition = carryOffset;

            carriedWood.transform.localScale = carriedScale;


            if (carriedWood.TryGetComponent<Collider2D>(out var col))
            {
                col.enabled = false;
            }
        }
        else if (Input.GetMouseButtonDown(0) && carriedWood != null)
        {
            carriedWood.transform.parent = null;

            Vector3 dropPosition = transform.position + Vector3.down * 0.2f;
            carriedWood.transform.position = dropPosition;
            carriedWood.transform.localScale = droppedScale;


            if (carriedWood.TryGetComponent<Collider2D>(out var col))
            {
                col.enabled = true;
            }
            carriedWood = null;
        }
       
    }

   

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tree))
        {
            touchingTree = true;
            currentCollision = collision;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(tree))
        {
            touchingTree = false;
            currentCollision = null;

        }
    }


}

