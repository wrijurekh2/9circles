using UnityEngine;

public class BackgroundFit : MonoBehaviour
{
    void Start()
    {
        Camera cam = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float CamHeight = cam.orthographicSize * 2f;
        float spriteHeight = sr.sprite.bounds.size.y;
        float scale = CamHeight / spriteHeight;
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
