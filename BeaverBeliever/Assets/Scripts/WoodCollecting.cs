using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class WoodCollecting : MonoBehaviour
{
    public GameObject spritePrefab;   // Prefab with SpriteRenderer
    private bool touchingTree = false;
    public string tree = "Trunk";
    public Vector3 carryOffset = new Vector3(0f, 1f, 0f);
    public Vector3 dropOffset = new Vector3(0f, 3f, 0f);

    public Vector3 carriedScale = new Vector3(0.5f, 0.5f, 1f);
    public Vector3 droppedScale = new Vector3(1f, 1f, 1f);



    private GameObject carriedWood = null;




    Collision2D currentCollision;



    private void Start()
    {
       
        
    }

    void Update()
    {

        if (touchingTree && Mouse.current.leftButton.wasReleasedThisFrame && carriedWood == null)
        {
            Transform parent = currentCollision.transform.parent;
            if (parent != null)
            {
                Destroy(parent.gameObject);

            }
            else
            {
                Destroy(currentCollision.gameObject);


            }

            carriedWood = Instantiate(spritePrefab, transform);

            carriedWood.transform.localPosition = carryOffset;

            carriedWood.transform.localScale = carriedScale;


            if (carriedWood.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.simulated = false;
            }

            if (carriedWood.TryGetComponent<Collider2D>(out var col))
            {
                col.enabled = false;
            }
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame && carriedWood != null)
        {

            if (carriedWood.TryGetComponent<Collider2D>(out var col))
            {
                col.enabled = true;
            }

            if (carriedWood.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.simulated = true;
            }

            carriedWood.transform.parent = null;

            Vector3 dropPosition = transform.position + dropOffset;
            carriedWood.transform.position = dropPosition;
            carriedWood.transform.localScale = droppedScale;


       
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

