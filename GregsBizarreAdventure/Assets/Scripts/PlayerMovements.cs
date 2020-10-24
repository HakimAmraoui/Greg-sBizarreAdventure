using System;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [SerializeField] private float moveSpeed, jumpForce;

    [SerializeField] private bool isJumping, isGrounded, isSliding;

    // Those variables are used to check is the player is currently on the ground, the 'transform' is the center of the
    // circle attached to the player, the 'radius' is the radius of the circle
    // and the 'collisionLayers' is Filter to check objects only on specific layers 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask collisionLayers;

    // The animator is used to switch player's animations, as when he jump, or run
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;

    private Vector3 velocity = Vector3.zero;
    private float horizontalMovement;

    public static PlayerMovements instance;


    private void Awake()
    {
        // Just to be sure there is not multiple instance of the same object
        // it is especially used when you switch scene 
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of PlayerMovements in the scene.");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        // Assign all the variables
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = 300;
        jumpForce = 250;
    }

    private void Update()
    {
        // We calculate the speed and the direction for the horizontal movement
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;

        // If we press "Space" AND the player is on the Ground
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        //    animator.SetBool("isJumping", true);
        }

        /*if (Input.GetButtonDown("Crouch"))
        {
            isSliding = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            isSliding = false;
        }*/

        // If the player go to the left, the sprite should flip
        FlipX(rb.velocity.x);

        float characterVelocity = Mathf.Abs(rb.velocity.x);
        animator.SetFloat("Speed", Math.Abs(characterVelocity));
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isGrounded", isGrounded);
    }
/*
    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    } */
    

    void FixedUpdate()
    {
        // To prevent multiple jump, we have to check if the player is on the ground and not in the air,
        // To do that, we use Physics2D.OverlapCircle who checks if a colliders falls within a circlar area
        // The circle is defined by its centre coordinate in world space and by its radius.
        // The optional layerMask allows the test to check only for objects on specific layers

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionLayers);
        MovePlayer(horizontalMovement);
    }

    void MovePlayer(float _horizontalMovement)
    {
        // On calcul la velocité, la direction et à quel endroit le joueur doit faire son deplacement
        // On lui envoie la même force sur les abcisses qu'il a actuellement 
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);

        // On applique le mouvement sur le rigidbody du joueur
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);


        if (isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }
    }

    // This method is used to flip on X the player's sprite if he is turn to the left
    void FlipX(float _velocity)
    {
        if (_velocity < -0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (_velocity > 0.1f)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}