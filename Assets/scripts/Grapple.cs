using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleLength;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private float pullForce = 20f;
    [SerializeField] private float arrivalThreshold = 1f;
    private Rigidbody2D rb;
    private InputSystem_Actions playerInput;
    public bool isGrappling = false;

    private Vector3 grapplePoint;

    void Start()
    {
        rope.enabled = false;
        playerInput = new InputSystem_Actions();
        playerInput.Enable();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(playerInput.Player.Grapple.WasPressedThisFrame())
        {
            RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Input.mousePosition), 
            Vector2.zero, 
            Mathf.Infinity,
            grappleLayer);

            if(hit.collider !=null)
            {
                grapplePoint = hit.point;
                grapplePoint.z =0;
                isGrappling = true;
                rope.SetPosition(0, grapplePoint);
                rope.enabled = true;
            }
        }

        if(playerInput.Player.Grapple.WasReleasedThisFrame())
        {
           Detach();
        }

        if(rope.enabled)
        {
            rope.SetPosition(1, transform.position);
        }
    }

    void FixedUpdate()
    {
        if(!isGrappling) return;
        
        Vector2 direction = (grapplePoint - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, grapplePoint);

        rb.gravityScale = 0;
        rb.AddForce(direction * pullForce);

        // detach once close enough
        if (distance < arrivalThreshold)
        {
            Detach();
        }
            
        
    }

    void Detach()
    {
        isGrappling = false;
        rope.enabled = false;
        rb.gravityScale = 2;
    }
}