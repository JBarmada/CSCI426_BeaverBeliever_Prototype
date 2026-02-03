using UnityEngine;

public class Tree : MonoBehaviour
{
    [Header("Chopping")]
    public int hitsToChop = 3;

    TreeShake shaker;

    void Awake()
    {
        shaker = GetComponent<TreeShake>();
    }

    public bool Chop()
    {
        // Visual feedback
        if (shaker != null)
            shaker.Shake();

        hitsToChop--;

        // Return true if tree should be destroyed
        return hitsToChop <= 0;
    }
}
