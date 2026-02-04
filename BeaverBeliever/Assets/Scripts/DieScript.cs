using UnityEngine;

public class DieScript : MonoBehaviour
{
    public bool ActuallyDie = false;
    public SlideUpPanel gameOverPanel;



    public AudioSource audioSource;
    public AudioClip deathClip;

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
            if (audioSource && deathClip) audioSource.PlayOneShot(deathClip);

            Debug.Log("ActuallyDie: " + ActuallyDie);
            if (gameOverPanel)
                gameOverPanel.Show();

            foreach (WolfChase wolf in FindObjectsByType<WolfChase>(FindObjectsSortMode.None))
            {
                wolf.enabled = false;
            }
            Destroy(gameObject);
        }
    }
}
