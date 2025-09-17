using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;

    // 地面检测
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 左右移动
        float moveInput = Input.GetAxis("Horizontal"); // A/D 或 ←/→
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // 跳跃
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded) // 空格键
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}