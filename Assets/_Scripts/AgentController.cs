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
    private GameObject finalTarget; // Agent��ǰ��Ҫ��ɵ�����Ŀ��
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

        // --- 1. ��֪���� ---
        PerceiveEnvironment();

        // --- 2. ���ߣ��Ӳ��Խű���ȡ�����롱Ŀ�� ---
        // ��һ����AI�ġ�˼�����������ݵ�ǰ������Ƿ����Կ�׵ȣ�������Ӧ����ʲô��
        GameObject idealTarget = currentPolicy.DecideTarget(perceptionData);

        // --- 3. Ŀ�������·������ ---
        // ����Ƿ���Ҫ������Ŀ���·����
        // �������µ���������֣�
        // A) ���Ը�����һ��ȫ�µ�Ŀ�� (��������Կ�׺�����Ŀ��ӡ�Կ�ס���Ϊ���š�)
        // B) ��ǰ����Ŀ�걻������ (����Կ�ױ��ռ���finalTarget �Զ������ null)
        if (idealTarget != finalTarget)
        {
            Debug.Log($"[AgentController] Target has changed. Old: {(finalTarget != null ? finalTarget.name : "COMPLETED/NULL")}, New: {(idealTarget != null ? idealTarget.name : "null")}. Planning new path.");
            finalTarget = idealTarget; // ������Ŀ��

            if (finalTarget != null)
            {
                // Ϊ�µ���Ŀ��滮·��
                currentPath = WaypointManager.Instance.FindPath(transform.position, finalTarget.transform.position);
                currentPathIndex = 0;
            }
            else
            {
                // ���û������Ŀ�꣬��û��·��
                currentPath = null;
            }
        }

        // --- 4. ����ִ�� ---
        // �����ǰ����Ч·����������·���ƶ�
        if (currentPath != null && currentPath.Count > 0)
        {
            Waypoint currentWaypoint = currentPath[currentPathIndex];

            // ���ӷ����Լ�飺���·���е�ĳ����Ҳ���������٣�����·���Է�����
            if (currentWaypoint == null)
            {
                Debug.LogWarning("[AgentController] A waypoint in the current path was destroyed. Invalidating path.");
                currentPath = null;
                finalTarget = null; // ǿ�����¾���
                return;
            }

            // ָ�ӵ���ģ����ǰ·�����ƶ�
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