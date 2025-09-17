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

    // --- ������������������Ծ״̬���� ---
    private bool isExecutingLedgeJump = false;
    private float ledgeJumpTimer = 0f;
    public float ledgeJumpDuration = 0.5f; // ����ƽ̨ʱ��ǿ����ǰ�ƶ��ĳ���ʱ��
    private float lastDirection = 1f;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        // ÿ֡������Ծ��ʱ��
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

        // �������ִ���� ledge����ǿ����ǰ�ƶ�
        if (isExecutingLedgeJump)
        {
            playerController.Move(lastDirection);
            return; // ������֡�����������߼�
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
            // ���������������� ledge jump ״̬
            if (wallInFront) // ֻ����ǰ��ǽʱ�Ŵ���������ԭ����Ҳ��ǰ��
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
        // Gizmos����Ҳ��������ķ���̬����
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