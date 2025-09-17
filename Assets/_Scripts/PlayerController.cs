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

    // --- 新增 ---
    // Agent可以通过这个变量来控制是否由AI接管
    public bool isAgentControlled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 如果是玩家控制，则接收键盘输入
        if (!isAgentControlled)
        {
            HandlePlayerInput();
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // 将玩家输入逻辑封装到一个方法里
    private void HandlePlayerInput()
    {
        float moveInput = Input.GetAxis("Horizontal");
        Move(moveInput);

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    // --- 核心修改：将移动和跳跃变为公共方法 ---
    // 这样Agent就可以像调用遥控器一样调用它们

    /// <summary>
    /// 控制角色移动
    /// </summary>
    /// <param name="direction">-1 for left, 1 for right, 0 for stop</param>
    public void Move(float direction)
    {
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    /// <summary>
    /// 控制角色跳跃
    /// </summary>
    public void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}