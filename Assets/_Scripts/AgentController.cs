using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerController), typeof(EntityStats), typeof(AgentNavigation))]
public class AgentController : MonoBehaviour
{
    private AgentNavigation navigationModule;
    private EntityStats playerStats;
    private PerceptionData perceptionData = new PerceptionData();

    [Header("Policy")]
    public MonoBehaviour policyScript;
    private IAgentPolicy currentPolicy;

    [Header("Perception")]
    public float perceptionRadius = 15f;

    private List<Waypoint> currentPath;
    private int currentPathIndex;
    private GameObject finalTarget; // Agent当前需要完成的最终目标
    public float waypointReachedThreshold = 1f;

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

        // --- 1. 感知环境 ---
        PerceiveEnvironment();

        // --- 2. 决策：从策略脚本获取“理想”目标 ---
        // 这一步是AI的“思考”，它根据当前情况（是否持有钥匙等）决定最应该做什么。
        GameObject idealTarget = currentPolicy.DecideTarget(perceptionData);

        // --- 3. 目标管理与路径更新 ---
        // 检查是否需要更新主目标和路径。
        // 触发更新的情况有两种：
        // A) 策略给出了一个全新的目标 (比如拿完钥匙后，理想目标从“钥匙”变为“门”)
        // B) 当前的主目标被销毁了 (比如钥匙被收集后，finalTarget 自动变成了 null)
        if (idealTarget != finalTarget)
        {
            Debug.Log($"[AgentController] Target has changed. Old: {(finalTarget != null ? finalTarget.name : "COMPLETED/NULL")}, New: {(idealTarget != null ? idealTarget.name : "null")}. Planning new path.");
            finalTarget = idealTarget; // 更新主目标

            if (finalTarget != null)
            {
                // 为新的主目标规划路径
                currentPath = WaypointManager.Instance.FindPath(transform.position, finalTarget.transform.position);
                currentPathIndex = 0;
            }
            else
            {
                // 如果没有理想目标，则没有路径
                currentPath = null;
            }
        }

        // --- 4. 导航执行 ---
        // 如果当前有有效路径，则沿着路径移动
        if (currentPath != null && currentPath.Count > 0)
        {
            Waypoint currentWaypoint = currentPath[currentPathIndex];

            // 增加防御性检查：如果路径中的某个点也被意外销毁，作废路径以防崩溃
            if (currentWaypoint == null)
            {
                Debug.LogWarning("[AgentController] A waypoint in the current path was destroyed. Invalidating path.");
                currentPath = null;
                finalTarget = null; // 强制重新决策
                return;
            }

            // 指挥导航模块向当前路径点移动
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

    private void PerceiveEnvironment()
    {
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