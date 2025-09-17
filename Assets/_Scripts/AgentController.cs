// AgentController.cs (Modified)
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerController), typeof(EntityStats), typeof(AgentNavigation))]
public class AgentController : MonoBehaviour
{
    // --- 组件引用 (不变) ---
    private AgentNavigation navigationModule;
    private EntityStats playerStats;
    private PerceptionData perceptionData = new PerceptionData();

    // --- 策略引用 (不变) ---
    [Header("Policy")]
    public MonoBehaviour policyScript;
    private IAgentPolicy currentPolicy;

    [Header("Perception")]
    public float perceptionRadius = 15f;

    // --- 【新增】路径管理 ---
    private List<Waypoint> currentPath;
    private int currentPathIndex;
    private GameObject finalTarget; // 存储策略给出的最终目标
    public float waypointReachedThreshold = 1f; // 到达路径点的距离阈值

    void Awake()
    {
        navigationModule = GetComponent<AgentNavigation>();
        playerStats = GetComponent<EntityStats>();
        currentPolicy = policyScript as IAgentPolicy;

        if (currentPolicy == null)
        {
            Debug.LogError("Policy script is either not assigned or does not implement IAgentPolicy!", this);
        }
    }

    void Start()
    {
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.isAgentControlled = true;
        }
    }

    void Update()
    {
        if (currentPolicy == null) return;

        // 1. 感知
        PerceiveEnvironment();

        // 2. 决策：从策略脚本获取高级目标
        GameObject newTarget = currentPolicy.DecideTarget(perceptionData);

        // 3. 【核心修改】路径规划与执行
        // 如果目标发生变化，则重新规划路径
        if (newTarget != finalTarget)
        {
            finalTarget = newTarget;
            if (finalTarget != null)
            {
                // 请求路径管理器规划路径
                currentPath = WaypointManager.Instance.FindPath(transform.position, finalTarget.transform.position);
                currentPathIndex = 0;
            }
            else
            {
                currentPath = null; // 没有目标，就没有路径
            }
        }

        // 如果有路径，则沿着路径移动
        if (currentPath != null && currentPath.Count > 0)
        {
            // 获取当前要移动向的路径点
            Waypoint currentWaypoint = currentPath[currentPathIndex];

            // 将当前路径点作为“微观操作”的目标，交给导航模块
            navigationModule.NavigateTowards(currentWaypoint.gameObject);

            // 检查是否已到达当前路径点
            if (Vector3.Distance(transform.position, currentWaypoint.transform.position) < waypointReachedThreshold)
            {
                // 如果到达，则将目标设为路径中的下一个点
                if (currentPathIndex < currentPath.Count - 1)
                {
                    currentPathIndex++;
                }
            }
        }
        else
        {
            // 如果没有路径（因为没有目标或找不到路），则停止移动
            navigationModule.NavigateTowards(null);
        }
    }

    // PerceiveEnvironment 方法保持不变...
    private void PerceiveEnvironment()
    {
        // ... (你的代码)
        perceptionData.visibleCollectibles.Clear();
        perceptionData.visibleMonsters.Clear();
        perceptionData.visibleDoor = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, perceptionRadius);
        foreach (Collider2D col in colliders)
        {
            if (col.TryGetComponent<Collectible>(out var collectible))
            {
                perceptionData.visibleCollectibles.Add(collectible);
            }
            else if (col.TryGetComponent<Monster>(out var monster))
            {
                perceptionData.visibleMonsters.Add(monster);
            }
            else if (col.TryGetComponent<Door>(out var door))
            {
                perceptionData.visibleDoor = door;
            }
        }
        perceptionData.playerStats = this.playerStats;
        perceptionData.hasKey = GameManager.Instance.hasKey;
    }
}