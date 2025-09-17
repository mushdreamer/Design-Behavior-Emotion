using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerController), typeof(EntityStats), typeof(AgentNavigation))]
public class AgentController : MonoBehaviour
{
    // --- ������� ---
    private AgentNavigation navigationModule;
    private EntityStats playerStats; // ������ EntityStats ������
    private PerceptionData perceptionData = new PerceptionData();

    // --- �������� ---
    [Header("Policy")]
    public MonoBehaviour policyScript;
    private IAgentPolicy currentPolicy;

    [Header("Perception")]
    public float perceptionRadius = 15f; // ��֪�뾶�Ƶ�����������

    void Awake()
    {
        navigationModule = GetComponent<AgentNavigation>();
        playerStats = GetComponent<EntityStats>(); // ��ȡ EntityStats ���
        currentPolicy = policyScript as IAgentPolicy;

        if (currentPolicy == null)
        {
            Debug.LogError("Policy script is either not assigned or does not implement IAgentPolicy!", this);
        }
    }

    void Start()
    {
        // Start ���������� isAgentControlled
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
        GameObject target = currentPolicy.DecideTarget(perceptionData);

        // 3. ִ�У���Ŀ�꽻������ģ��ȥ����
        navigationModule.NavigateTowards(target);
    }

    // --- �������������ָ� PerceiveEnvironment ����������ʵ�� ---
    private void PerceiveEnvironment()
    {
        // �����һ֡���б�
        perceptionData.visibleCollectibles.Clear();
        perceptionData.visibleMonsters.Clear();
        perceptionData.visibleDoor = null;

        // ʹ��������������б�
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

        // ���Agent�����״̬��Ϣ
        perceptionData.playerStats = this.playerStats;
        perceptionData.hasKey = GameManager.Instance.hasKey;
    }
}