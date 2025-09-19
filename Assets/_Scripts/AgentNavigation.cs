using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class AgentNavigation : MonoBehaviour
{
    private PlayerController playerController;

    [Header("Sensor Offsets")]
    public Vector2 wallCheckOffset = new Vector2(0.5f, 0f);
    public Vector2 groundCheckOffset = new Vector2(0.5f, -0.5f);
    public Vector2 ceilingCheckOffset = new Vector2(0f, 0.5f);
    // --- 【新增】 ledge (平台边缘) 检测的偏移 ---
    public Vector2 ledgeCheckOffset = new Vector2(0.5f, 0.8f);

    [Header("Sensor Distances")]
    public float wallCheckDistance = 0.5f;
    // --- 【新增】 ledge 检测的距离 ---
    public float ledgeCheckDistance = 0.5f;
    public float gapCheckDistance = 0.7f;
    public float ceilingCheckDistance = 1.0f;
    public float targetHeightThreshold = 1.5f;
    public LayerMask groundLayer;

    private bool isExecutingLedgeJump = false;
    private float ledgeJumpTimer = 0f;
    public float ledgeJumpDuration = 0.5f;
    private float lastDirection = 1f;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
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

        if (isExecutingLedgeJump)
        {
            playerController.Move(lastDirection);
            return;
        }

        Vector2 wallOrigin = (Vector2)transform.position + new Vector2(wallCheckOffset.x * direction, wallCheckOffset.y);
        Vector2 groundOrigin = (Vector2)transform.position + new Vector2(groundCheckOffset.x * direction, groundCheckOffset.y);
        Vector2 ceilingOrigin = (Vector2)transform.position + ceilingCheckOffset;
        // --- 【新增】 ledge 检测射线的起点 ---
        Vector2 ledgeOrigin = (Vector2)transform.position + new Vector2(ledgeCheckOffset.x * direction, ledgeCheckOffset.y);


        bool wallInFront = Physics2D.Raycast(wallOrigin, new Vector2(direction, 0), wallCheckDistance, groundLayer);
        // --- 【新增】 发射 ledge 检测射线 ---
        bool ledgeInFront = Physics2D.Raycast(ledgeOrigin, new Vector2(direction, 0), ledgeCheckDistance, groundLayer);
        bool gapInFront = !Physics2D.Raycast(groundOrigin, Vector2.down, gapCheckDistance, groundLayer);
        bool ceilingAbove = Physics2D.Raycast(ceilingOrigin, Vector2.up, ceilingCheckDistance, groundLayer);
        bool targetIsAbove = target.transform.position.y - transform.position.y > targetHeightThreshold;

        // --- 【核心修复】将 ledgeInFront 加入跳跃判断 ---
        if ((wallInFront || ledgeInFront || targetIsAbove) && !ceilingAbove)
        {
            playerController.Jump();

            isExecutingLedgeJump = true;
            ledgeJumpTimer = ledgeJumpDuration;
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

        // --- 【新增】在编辑器中绘制新的 ledge check 射线 (红色) ---
        Vector2 ledgeOrigin = (Vector2)transform.position + new Vector2(ledgeCheckOffset.x * lastDirection, ledgeCheckOffset.y);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ledgeOrigin, ledgeOrigin + new Vector2(ledgeCheckDistance * lastDirection, 0));
    }
}