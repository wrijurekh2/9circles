using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void Start()
    {
        transform.position = new Vector3(
            player.position.x + offset.x,
            transform.position.y,
            transform.position.z
        );
    }

    void LateUpdate()
    {
        float targetX = player.position.x + offset.x;

        float smoothedX = Mathf.Lerp(
            transform.position.x,
            targetX,
            smoothSpeed
        );

        // Only follow X, keep Y and Z fixed
        transform.position = new Vector3(
            smoothedX,
            transform.position.y,
            transform.position.z
        );
    }
}