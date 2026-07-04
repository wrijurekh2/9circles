using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    #region Variables 
    [SerializeField] private float grappleLength;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LineRenderer rope;
    [SerializeField] private float pullForce = 40f;
    [SerializeField] private float throwSpeed = 40f;
    [SerializeField] private float arrivalThreshold = 3f;
    [SerializeField] private GameObject portalEffect;

    private Rigidbody2D rb;
    private InputSystem_Actions playerInput;
    public bool isGrappling = false;
    private Vector3 grapplePoint;
    private bool isThrowing = false;
    private Vector3 ropeEnd;
    private bool missed = false;
    private PlayerMovement playerMovement;
    public bool recentlyGrappled = false;
    private GameObject spawnedPortal;
    private Vector2 mousePos;
    private SpriteRenderer sr;
    private Color color;
    #endregion

    #region Unity Callbacks
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
        AnchorPoint target = FindClosestAnchor();
        mousePos = Mouse.current.position.ReadValue();

        foreach (var anchor in FindObjectsByType<AnchorPoint>()) 
        {
            anchor.GetComponentInChildren<Light2D>().intensity = 0f;
            sr = anchor.GetComponentInChildren<SpriteRenderer>();
            color = sr.color;
            color.a = 112.5f;
        }

        if (target != null ) 
        {
            target.GetComponentInChildren<Light2D>().intensity = 1f;
            sr = target.GetComponentInChildren<SpriteRenderer>();
            color = sr.color;
            color.a = 255;

        }

        if (playerInput.Player.Grapple.WasPressedThisFrame() && !isThrowing && !isGrappling)
        {

            if (target != null)
            {
                grapplePoint = target.transform.position;
                grapplePoint.z = 0;
                ropeEnd = transform.position;
                playerMovement.rb.linearVelocity = new Vector2(0, 0);
                isThrowing = true;
                rope.SetPosition(0, transform.position);
                rope.SetPosition(1, transform.position);
                rope.enabled = true;
                spawnedPortal = Instantiate(portalEffect, grapplePoint, Quaternion.identity);
                float throwDistance = Vector3.Distance(transform.position, grapplePoint);
                float travelTime = throwDistance / throwSpeed;
                Animator portalAnimator = spawnedPortal.GetComponent<Animator>();
                portalAnimator.speed = 1f / travelTime;
            }
        }

        if (isThrowing)
        {
            ropeEnd = Vector3.MoveTowards(ropeEnd, grapplePoint, throwSpeed * Time.deltaTime);
            rope.SetPosition(0, ropeEnd);

            if (Vector3.Distance(ropeEnd, grapplePoint) < 0.1f)
            {
                if (missed)
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
        if (isGrappling)
        {
            rope.SetPosition(0, grapplePoint);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"),
                                           LayerMask.NameToLayer("Ground"),
                                           true);
            if (playerInput.Player.Grapple.WasPressedThisFrame())
            {
                Detach();
            }
        }

        if (!isGrappling)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"),
                                           LayerMask.NameToLayer("Ground"),
                                           false);
        }

        if (rope.enabled)
        {
            rope.SetPosition(1, transform.position);
        }


        if (missed && Vector3.Distance(ropeEnd, transform.position) < 0.1f)
        {
            Detach();
        }
    }

    void FixedUpdate()
    {
        if (!isGrappling) return;

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
    #endregion

    #region Grapple Methods
    void Detach()
    {
        isGrappling = false;
        isThrowing = false;
        rope.enabled = false;
        rb.gravityScale = 2;
        recentlyGrappled = true;
        if (spawnedPortal != null)
        {
            Destroy(spawnedPortal);
        }
    }

    
    private AnchorPoint FindClosestAnchor()
    {
        AnchorPoint[] allAnchors = FindObjectsByType<AnchorPoint>();
        AnchorPoint closest = null;
        Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);
        float closestPoint = grappleLength;

        foreach (AnchorPoint anchor in allAnchors)
        {
            float distance = Vector2.Distance(mousePosWorld, anchor.transform.position);
            if (distance < closestPoint)
            {
                closestPoint = distance;
                closest = anchor;
            }
        }

        if (closest == null) return null;

        if (Vector2.Distance(transform.position, closest.transform.position) <= grappleLength)
        {
            return closest;
        }

        return null;
    }
    #endregion
}