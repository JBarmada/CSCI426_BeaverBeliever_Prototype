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

    // NEW: Determines if the dam is safe enough to hide in
    private bool canHide = true; 

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(DamTag))
        {
            touchingDam = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(DamTag))
        {
            touchingDam = false;
        }
    }

    private void Update()
    {
        // Use 'wasPressedThisFrame' to handle the toggle cleanly without extra bools
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (hidden)
            {
                UnHide();
            }
            // Check 'canHide' here to ensure we don't hide if the dam is broken
            else if (canHide && touchingDam && damCollector.damFull)
            {
                Hide();
            }
        }
    }

    void Hide()
    {
        hidden = true;
        spriteRenderer.enabled = false;
        beaverMovement.beaverMove = false;
        collision.enabled = false;
        
        // Optional: Stop physics interactions so wolves can't push the invisible player
        if(rigidBody) rigidBody.simulated = false; 
    }

    void UnHide()
    {
        hidden = false;
        spriteRenderer.enabled = true;
        beaverMovement.beaverMove = true;
        collision.enabled = true;

        if(rigidBody) rigidBody.simulated = true;
    }

    // --- NEW METHODS ---

    // Called by DamCollecting when the wolves break the dam
    public void ForceUnhide()
    {
        UnHide();
        canHide = false; // Lock the player out of the dam
    }

    // Called by GameplayDirector at the start of Day 2
    public void ResetAbility()
    {
        canHide = true;  // Allow hiding again
    }
}