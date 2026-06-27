using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float minX = -94f;
    public float maxX = 120.2f;
    private float blockSize = 11.25f;
    private float targetY;
    void Start()
    {
        transform.position = new Vector3(
            player.position.x + offset.x,
            transform.position.y,
            transform.position.z
        );
        targetY = transform.position.y;
    }

    void LateUpdate()
    {
        float targetX = player.position.x + offset.x;
        targetX = Mathf.Clamp(targetX, minX, maxX);

        transform.position = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );

        float cameraHalfHeight = Camera.main.orthographicSize;

        if (player.position.y > targetY + cameraHalfHeight)
        {
            targetY += blockSize;
        }

        if(player.position.y < targetY - cameraHalfHeight)
        {
            targetY -= blockSize;
        }

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(transform.position.y, targetY, Time.deltaTime * 5f),
            transform.position.z
        );

        //Debug.Log(Camera.main.orthographicSize * 2);
    }
}