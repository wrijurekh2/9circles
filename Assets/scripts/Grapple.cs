using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private float grappleLength;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private float pullForce = 20f;
    [SerializeField] private float arrivalThreshold = 3f;
    private Rigidbody2D rb;
    private InputSystem_Actions playerInput;
    public bool isGrappling = false;
    private Vector3 grapplePoint;
    private bool isThrowing = false; 
    private Vector3 ropeEnd;
    private float throwSpeed = 20f; 
    private bool missed = false;
    private PlayerMovement playerMovement;
    public bool recentlyGrappled = false;

    void Start()
    {
        rope.enabled = false;
        playerInput = new InputSystem_Actions();
        playerInput.Enable();
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if(playerInput.Player.Grapple.WasPressedThisFrame() && !isThrowing && !isGrappling)
        {
            
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            
            Vector2 aimDirection = (mouseWorldPos - transform.position).normalized;
            
            RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            aimDirection, 
            grappleLength,
            grappleLayer);

            //Debug.Log("Hit: " + hit.collider + " Distance: " + hit.distance);
            //Debug.DrawRay(transform.position, aimDirection * grappleLength, Color.red, 2f);

            if(hit.collider !=null)
            {
                grapplePoint = hit.point;
                grapplePoint.z =0;
                ropeEnd = transform.position;
                isThrowing = true;
                rope.SetPosition(0, transform.position);
                rope.SetPosition(1, transform.position);
                rope.enabled = true;
            }
            else
            {
                grapplePoint = (Vector3)(Vector2)transform.position + (Vector3)aimDirection * grappleLength;
                grapplePoint.z = 0;
                ropeEnd = transform.position;
                isThrowing = true;
                missed = true; 
                rope.SetPosition(0, transform.position);
                rope.SetPosition(1, transform.position);
                rope.enabled = true;
            }
        }

        if(isThrowing)
        {
            ropeEnd = Vector3.MoveTowards(ropeEnd, grapplePoint, throwSpeed * Time.deltaTime);
            rope.SetPosition(0, ropeEnd);

            if(Vector3.Distance(ropeEnd, grapplePoint) < 0.1f)
            {
                if(missed)
                {
                    missed = false;
                    grapplePoint = transform.position;
                }

                else
                {
                    isThrowing = false;
                    isGrappling = true;
                }
            }
        }
        if(isGrappling)
        {
            rope.SetPosition(0, grapplePoint);
        }

        if(rope.enabled)
        {
            rope.SetPosition(1, transform.position);
        }

        /*if(playerInput.Player.Grapple.WasReleasedThisFrame())
        {
           Detach();
        }*/


        if(missed && Vector3.Distance(ropeEnd, transform.position) < 0.1f)
        {
            Detach();
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
        isThrowing = false;
        rope.enabled = false;
        rb.gravityScale = 2;
        recentlyGrappled = true;
    }
}