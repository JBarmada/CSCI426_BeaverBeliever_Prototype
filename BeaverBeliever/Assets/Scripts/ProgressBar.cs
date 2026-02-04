using UnityEngine;

public class ProgressBar : MonoBehaviour
{



    private DamCollecting damCollector;

    float maxScale;
    float yScale;
    public float currentScale = 0;

    Vector3 startLoc;


    void Awake()
    {
        damCollector = GetComponentInParent<DamCollecting>();
        maxScale = transform.localScale.x;
        yScale = transform.localScale.y;
        startLoc =transform.localPosition;

    }
    void Update()
    {
        float percent = (float)damCollector.currentDamStrength / damCollector.totalDamStrength;
        currentScale = percent * maxScale;
        transform.localScale = new Vector3(currentScale, yScale, 0f);

        float offsetX = (1f - percent) * maxScale * 0.5f;
        transform.localPosition = startLoc + Vector3.left * offsetX;

    }
}

