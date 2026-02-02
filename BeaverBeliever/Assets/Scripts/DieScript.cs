using UnityEngine;

public class DieScript : MonoBehaviour
{

    public bool ActuallyDie = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wolf"))
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died");

        if (ActuallyDie)
        {
            Destroy(gameObject);

        }
    }
}
