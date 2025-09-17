using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerController), typeof(EntityStats), typeof(AgentNavigation))]
public class AgentController : MonoBehaviour
{
    // --- 组件引用 ---
    private AgentNavigation navigationModule;
    private EntityStats playerStats; // 新增对 EntityStats 的引用
    private PerceptionData perceptionData = new PerceptionData();

    // --- 策略引用 ---
    [Header("Policy")]
    public MonoBehaviour policyScript;
    private IAgentPolicy currentPolicy;

    [Header("Perception")]
    public float perceptionRadius = 15f; // 感知半径移到这里，方便管理

    void Awake()
    {
        navigationModule = GetComponent<AgentNavigation>();
        playerStats = GetComponent<EntityStats>(); // 获取 EntityStats 组件
        currentPolicy = policyScript as IAgentPolicy;

        if (currentPolicy == null)
        {
            Debug.LogError("Policy script is either not assigned or does not implement IAgentPolicy!", this);
        }
    }

    void Start()
    {
        // Start 方法中设置 isAgentControlled
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
        GameObject target = currentPolicy.DecideTarget(perceptionData);

        // 3. 执行：将目标交给导航模块去处理
        navigationModule.NavigateTowards(target);
    }

    // --- 【核心修正】恢复 PerceiveEnvironment 方法的完整实现 ---
    private void PerceiveEnvironment()
    {
        // 清空上一帧的列表
        perceptionData.visibleCollectibles.Clear();
        perceptionData.visibleMonsters.Clear();
        perceptionData.visibleDoor = null;

        // 使用物理检测来填充列表
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

        // 填充Agent自身的状态信息
        perceptionData.playerStats = this.playerStats;
        perceptionData.hasKey = GameManager.Instance.hasKey;
    }
}