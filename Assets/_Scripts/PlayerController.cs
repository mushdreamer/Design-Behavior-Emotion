using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Movement Stats")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    // --- ���� ---
    // Agent����ͨ����������������Ƿ���AI�ӹ�
    public bool isAgentControlled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �������ҿ��ƣ�����ռ�������
        if (!isAgentControlled)
        {
            HandlePlayerInput();
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // ����������߼���װ��һ��������
    private void HandlePlayerInput()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Move(moveInput);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    // --- �����޸ģ����ƶ�����Ծ��Ϊ�������� ---
    // ����Agent�Ϳ��������ң����һ����������

    /// <summary>
    /// ���ƽ�ɫ�ƶ�
    /// </summary>
    /// <param name="direction">-1 for left, 1 for right, 0 for stop</param>
    public void Move(float direction)
    {
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    /// <summary>
    /// ���ƽ�ɫ��Ծ
    /// </summary>
    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}