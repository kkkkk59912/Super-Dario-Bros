using UnityEngine;
using UnityEngine.InputSystem;

public class MarioMovement : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float jumpForce = 12f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // je li Mario na podu?
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float direction = 0f;

        if (Keyboard.current.leftArrowKey.isPressed) direction = -1f;
        if (Keyboard.current.rightArrowKey.isPressed) direction = 1f;

        // hodanje (vertikalnu brzinu ostavljamo fizici)
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        // skok
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // ako pustiš tipku ranije, skok je niži (kao u pravom Mariju)
        if (Keyboard.current.spaceKey.wasReleasedThisFrame && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }

        // animacije
        animator.SetFloat("Speed", Mathf.Abs(direction));
        animator.SetBool("IsGrounded", isGrounded);

        // okreni sprite u smjeru hodanja (originalni sprite gleda ulijevo)
        if (direction < 0) spriteRenderer.flipX = false;
        else if (direction > 0) spriteRenderer.flipX = true;
    }
}