using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 10f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 newPos = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            newPos,
            followSpeed * Time.deltaTime
        );
    }
}
