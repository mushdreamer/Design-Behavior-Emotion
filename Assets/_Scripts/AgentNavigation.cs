using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class AgentNavigation : MonoBehaviour
{
    private PlayerController playerController;

    [Header("Sensor Offsets")]
    public Vector2 wallCheckOffset = new Vector2(0.5f, 0f);
    public Vector2 groundCheckOffset = new Vector2(0.5f, -0.5f);
    public Vector2 ceilingCheckOffset = new Vector2(0f, 0.5f);

    [Header("Sensor Distances")]
    public float wallCheckDistance = 0.5f;
    public float gapCheckDistance = 0.7f;
    public float ceilingCheckDistance = 1.0f;
    public float targetHeightThreshold = 1.5f;
    public LayerMask groundLayer;

    // --- 【核心修正】新增跳跃状态变量 ---
    private bool isExecutingLedgeJump = false;
    private float ledgeJumpTimer = 0f;
    public float ledgeJumpDuration = 0.5f; // 跳上平台时，强制向前移动的持续时间
    private float lastDirection = 1f;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        // 每帧减少跳跃计时器
        if (ledgeJumpTimer > 0)
        {
            ledgeJumpTimer -= Time.deltaTime;
            if (ledgeJumpTimer <= 0)
            {
                isExecutingLedgeJump = false;
            }
        }
    }

    public void NavigateTowards(GameObject target)
    {
        if (target == null)
        {
            playerController.Move(0);
            return;
        }

        float direction = Mathf.Sign(target.transform.position.x - transform.position.x);
        if (Mathf.Approximately(direction, 0)) direction = lastDirection;
        else lastDirection = direction;

        // 如果正在执行跳 ledge，则强制向前移动
        if (isExecutingLedgeJump)
        {
            playerController.Move(lastDirection);
            return; // 跳过本帧的其他导航逻辑
        }

        Vector2 wallOrigin = (Vector2)transform.position + new Vector2(wallCheckOffset.x * direction, wallCheckOffset.y);
        Vector2 groundOrigin = (Vector2)transform.position + new Vector2(groundCheckOffset.x * direction, groundCheckOffset.y);
        Vector2 ceilingOrigin = (Vector2)transform.position + ceilingCheckOffset;

        bool wallInFront = Physics2D.Raycast(wallOrigin, new Vector2(direction, 0), wallCheckDistance, groundLayer);
        bool gapInFront = !Physics2D.Raycast(groundOrigin, Vector2.down, gapCheckDistance, groundLayer);
        bool ceilingAbove = Physics2D.Raycast(ceilingOrigin, Vector2.up, ceilingCheckDistance, groundLayer);
        bool targetIsAbove = target.transform.position.y - transform.position.y > targetHeightThreshold;

        if ((wallInFront || targetIsAbove) && !ceilingAbove)
        {
            playerController.Jump();
            // 【核心修正】触发 ledge jump 状态
            if (wallInFront) // 只在面前有墙时才触发，避免原地跳也向前冲
            {
                isExecutingLedgeJump = true;
                ledgeJumpTimer = ledgeJumpDuration;
            }
        }
        else if (gapInFront && !ceilingAbove)
        {
            playerController.Jump();
        }

        if (Mathf.Abs(target.transform.position.x - transform.position.x) > 0.5f)
        {
            playerController.Move(direction);
        }
        else
        {
            playerController.Move(0);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Gizmos现在也会根据最后的方向动态绘制
        Vector2 wallOrigin = (Vector2)transform.position + new Vector2(wallCheckOffset.x * lastDirection, wallCheckOffset.y);
        Vector2 groundOrigin = (Vector2)transform.position + new Vector2(groundCheckOffset.x * lastDirection, groundCheckOffset.y);
        Vector2 ceilingOrigin = (Vector2)transform.position + ceilingCheckOffset;

        // Wall Check (Blue)
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallOrigin, wallOrigin + new Vector2(wallCheckDistance * lastDirection, 0));

        // Gap Check (Green)
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundOrigin, groundOrigin + new Vector2(0, -gapCheckDistance));

        // Ceiling Check (Magenta)
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(ceilingOrigin, ceilingOrigin + new Vector2(0, ceilingCheckDistance));
    }
}