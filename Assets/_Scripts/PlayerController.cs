using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;

    // ������
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �����ƶ�
        float moveInput = Input.GetAxis("Horizontal"); // A/D �� ��/��
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ��Ծ
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded) // �ո��
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}