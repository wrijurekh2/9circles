using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float minX = -94f;
    public float maxX = 120.2f;
    private float targetY;
    void Start()
    {
        transform.position = new Vector3(
            player.position.x + offset.x,
            player.position.y + offset.y,
            transform.position.z
        );
    }

    void LateUpdate()
    {
        float targetX = player.position.x + offset.x;
        float targetY = player.position.y + offset.y;
        targetX = Mathf.Clamp(targetX, minX, maxX);
        //targetY = Mathf.Clamp(targetY, minY, maxY); use if needed later

        transform.position = new Vector3(
            targetX,
            targetY,
            transform.position.z
        );

  
    }
}