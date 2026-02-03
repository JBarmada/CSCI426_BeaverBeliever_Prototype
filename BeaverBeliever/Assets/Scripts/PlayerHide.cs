using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHide : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidBody;
    public Collider2D collision;


    public DamCollecting damCollector;
    public BetterBeaverControls beaverMovement;
    public string DamTag = "Dam";
    private bool touchingDam = false;

    public bool hidden = false;
    bool spacePressed = false;



    void Awake()
    {
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(DamTag))
        {
            touchingDam = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(DamTag))
        {
            touchingDam = false;
        }

    }

  
    

    private void Update()
    {
        if (touchingDam && !hidden && Keyboard.current.spaceKey.IsPressed() && damCollector.damFull && !spacePressed)
        {
            Hide();
        }else if(hidden && Keyboard.current.spaceKey.IsPressed() && !spacePressed)
        {
            UnHide();
        }

        spacePressed = Keyboard.current.spaceKey.IsPressed();
    }

    void Hide()
    {
        hidden = true;
        spriteRenderer.enabled = false;
        beaverMovement.beaverMove = false;
        collision.enabled = false;



    }

    void UnHide()
    {
        hidden = false;
        spriteRenderer.enabled = true;
        beaverMovement.beaverMove = true;
        collision.enabled = true;

    }




}
