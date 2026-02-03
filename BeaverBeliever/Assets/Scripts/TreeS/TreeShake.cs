using UnityEngine;
using System.Collections;

public class TreeShake : MonoBehaviour
{
    public float shakeDuration = 0.15f;
    public float shakeStrength = 0.1f;

    Vector3 originalPos;
    Coroutine routine;

    void Awake()
    {
        originalPos = transform.localPosition;
    }

    public void Shake()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float t = 0f;
        while (t < shakeDuration)
        {
            t += Time.deltaTime;
            float offset = Random.Range(-shakeStrength, shakeStrength);
            transform.localPosition = originalPos + new Vector3(offset, 0, 0);
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
