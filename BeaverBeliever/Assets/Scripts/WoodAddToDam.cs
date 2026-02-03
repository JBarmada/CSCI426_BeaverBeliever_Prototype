
using UnityEngine;

public class WoodAddToDam : MonoBehaviour
{

    // Call this when the object is dropped
    public void EnableCollection()
    {
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dam"))
        {
            DamCollecting collector = other.GetComponent<DamCollecting>();

            if (collector != null)
            {
                collector.Collect();
                Destroy(gameObject);
            }
        }
    }

}
