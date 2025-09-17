// AgentController.cs (Modified)
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerController), typeof(EntityStats), typeof(AgentNavigation))]
public class AgentController : MonoBehaviour
{
    // --- ������� (����) ---
    private AgentNavigation navigationModule;
    private EntityStats playerStats;
    private PerceptionData perceptionData = new PerceptionData();

    // --- �������� (����) ---
    [Header("Policy")]
    public MonoBehaviour policyScript;
    private IAgentPolicy currentPolicy;

    [Header("Perception")]
    public float perceptionRadius = 15f;

    // --- ��������·������ ---
    private List<Waypoint> currentPath;
    private int currentPathIndex;
    private GameObject finalTarget; // �洢���Ը���������Ŀ��
    public float waypointReachedThreshold = 1f; // ����·����ľ�����ֵ

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

        // 1. ��֪
        PerceiveEnvironment();

        // 2. ���ߣ��Ӳ��Խű���ȡ�߼�Ŀ��
        GameObject newTarget = currentPolicy.DecideTarget(perceptionData);

        // 3. �������޸ġ�·���滮��ִ��
        // ���Ŀ�귢���仯�������¹滮·��
        if (newTarget != finalTarget)
        {
            finalTarget = newTarget;
            if (finalTarget != null)
            {
                // ����·���������滮·��
                currentPath = WaypointManager.Instance.FindPath(transform.position, finalTarget.transform.position);
                currentPathIndex = 0;
            }
            else
            {
                currentPath = null; // û��Ŀ�꣬��û��·��
            }
        }

        // �����·����������·���ƶ�
        if (currentPath != null && currentPath.Count > 0)
        {
            // ��ȡ��ǰҪ�ƶ����·����
            Waypoint currentWaypoint = currentPath[currentPathIndex];

            // ����ǰ·������Ϊ��΢�۲�������Ŀ�꣬��������ģ��
            navigationModule.NavigateTowards(currentWaypoint.gameObject);

            // ����Ƿ��ѵ��ﵱǰ·����
            if (Vector3.Distance(transform.position, currentWaypoint.transform.position) < waypointReachedThreshold)
            {
                // ��������Ŀ����Ϊ·���е���һ����
                if (currentPathIndex < currentPath.Count - 1)
                {
                    currentPathIndex++;
                }
            }
        }
        else
        {
            // ���û��·������Ϊû��Ŀ����Ҳ���·������ֹͣ�ƶ�
            navigationModule.NavigateTowards(null);
        }
    }

    // PerceiveEnvironment �������ֲ���...
    private void PerceiveEnvironment()
    {
        // ... (��Ĵ���)
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