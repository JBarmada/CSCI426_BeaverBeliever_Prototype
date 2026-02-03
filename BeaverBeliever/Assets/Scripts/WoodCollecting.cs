using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class WoodCollecting : MonoBehaviour
{
    [Header("Wood Prefab")]
    public GameObject woodPrefab;

    [Header("Offsets")]
    public Vector2 carryOffset = new Vector2(0f, 1f);
    public Vector2 dropOffset = new Vector2(0f, 1f);

    [Header("Tags")]
    public string treeTag = "Trunk";
    public string woodTag = "Wood";


    GameObject carriedWood;
    Collision2D currentCollision;
    bool touchingTree;
    bool touchingWood;


    void Update()
    {
        if (Mouse.current == null)
            return;

        // CHOP TREE
        if (touchingTree &&
            Mouse.current.leftButton.wasReleasedThisFrame &&
            carriedWood == null)
        {
            TryChopTree();
        }else if (touchingWood &&
            Mouse.current.leftButton.wasReleasedThisFrame &&
            carriedWood == null)
        {
            Destroy(currentCollision.gameObject);
            PickUpWood();
            touchingWood = false;
            currentCollision = null;
        }
        // DROP WOOD
        else if (Mouse.current.leftButton.wasReleasedThisFrame &&
                 carriedWood != null)
        {
            DropWood();
        }
    }


    

    void TryChopTree()
    {
        if (currentCollision == null)
            return;

        Tree tree = currentCollision.transform.GetComponentInParent<Tree>();
        if (tree == null)
            return;

        bool destroyed = tree.Chop();

        if (destroyed)
        {
            Destroy(tree.gameObject);
            PickUpWood();
            touchingTree = false;
            currentCollision = null;
        }
    }

    void PickUpWood()
    {
        carriedWood = Instantiate(woodPrefab, transform);
        carriedWood.transform.localPosition = carryOffset;

        if (carriedWood.TryGetComponent<Rigidbody2D>(out var rb))
            rb.simulated = false;

        if (carriedWood.TryGetComponent<Collider2D>(out var col))
            col.enabled = false;
    }

    void DropWood()
    {
        carriedWood.transform.parent = null;
        carriedWood.transform.position = transform.position + (Vector3)dropOffset;

        if (carriedWood.TryGetComponent<Rigidbody2D>(out var rb))
            rb.simulated = true;

        if (carriedWood.TryGetComponent<Collider2D>(out var col))
            col.enabled = true;

        carriedWood = null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(treeTag))
        {
            touchingTree = true;
            currentCollision = collision;
        }else if (collision.gameObject.CompareTag(woodTag))
        {
            touchingWood = true;
            currentCollision = collision;

        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(treeTag))
        {
            touchingTree = false;
            currentCollision = null;
        }
        else if (collision.gameObject.CompareTag(woodTag))
        {
            touchingWood = true;
            currentCollision = collision;

        }
    }
}
