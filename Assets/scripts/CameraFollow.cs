using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float minX = -94f;
    public float maxX = 120.2f;

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
        targetX = Mathf.Clamp(targetX, minX, maxX);

        float smoothedX = Mathf.Lerp(
            transform.position.x,
            targetX,
            smoothSpeed
        );


        transform.position = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );
    }
}