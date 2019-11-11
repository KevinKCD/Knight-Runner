using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public float runSpeed = 10f;
    private float JumpForce = 5f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    bool attack = false;
    private bool FacingRight = true;  // For determining which way the player is currently facing.

    private bool isGrounded;            // Whether or not the player is grounded.
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private int JumpCount = 0;
    private bool DoubleJump;
    Rigidbody2D rb;

  
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("Jumping", true);
            Jump();
        }
        if (Input.GetButtonUp("Jump"))
        {
            jump = false;
            animator.SetBool("Jumping", false);
        }
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            animator.SetBool("Crouching", true);

        }
        if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            animator.SetBool("Crouching", false);

        }
        if (Input.GetButtonDown("Attack"))
        {
            attack = true;
            animator.SetBool("Attacking", true);
        }
        if (Input.GetButtonUp("Attack"))
        {
            attack = false;
            animator.SetBool("Attacking", false);

        }
        rb.velocity = new Vector2(horizontalMove * runSpeed, rb.velocity.y);
        if (horizontalMove > 0 && !FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (horizontalMove < 0 && FacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }
     private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = Vector2.up * JumpForce;
            DoubleJump = true;
        }
        else
        {
            if (DoubleJump)
            {
                DoubleJump = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity = Vector2.up * JumpForce;
            }
        }
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight = !FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
