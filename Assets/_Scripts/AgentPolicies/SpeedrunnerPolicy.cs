using UnityEngine;
using System.Linq;

public class SpeedrunnerPolicy : MonoBehaviour, IAgentPolicy
{
    /// <summary>
    /// AI�ĺ��ľ��ߺ���
    /// </summary>
    /// <param name="data">AI��֪�������л�������</param>
    /// <returns>��ǰӦ��ǰ����Ŀ��GameObject</returns>
    public GameObject DecideTarget(PerceptionData data)
    {
        // �������ȼ��жϣ��Ƿ��Ѿ�����Կ�ף�
        if (data.hasKey)
        {
            // --- �׶�2���ѳ���Կ�ף�Ŀ������ ---
            Debug.Log("[SpeedrunnerPolicy] ���ߣ��ѳ���Կ�ף�����Ѱ����...");

            // ����ܿ����ţ�Ŀ�������
            if (data.visibleDoor != null)
            {
                Debug.Log("[SpeedrunnerPolicy] ���ߣ�������Ұ�ڣ���ΪĿ�ꡣ");
                return data.visibleDoor.gameObject;
            }
            // ����������ţ�������̽��
            else
            {
                Debug.Log("[SpeedrunnerPolicy] ���ߣ��Ų�����Ұ�ڣ�����̽����");
                return CreateSearchTarget("SearchTarget_Door");
            }
        }
        else
        {
            // --- �׶�1��û��Կ�ף�Ŀ����Կ�� ---
            Debug.Log("[SpeedrunnerPolicy] ���ߣ�û��Կ�ף�����Ѱ��Կ��...");

            Collectible key = data.visibleCollectibles.FirstOrDefault(c => c.type == Collectible.CollectibleType.Key);

            // ����ܿ���Կ�ף�Ŀ�����Կ��
            if (key != null)
            {
                Debug.Log("[SpeedrunnerPolicy] ���ߣ�Կ������Ұ�ڣ���ΪĿ�ꡣ");
                return key.gameObject;
            }
            // ���������Կ�ף�������̽��
            else
            {
                Debug.Log("[SpeedrunnerPolicy] ���ߣ�Կ�ײ�����Ұ�ڣ�����̽����");
                return CreateSearchTarget("SearchTarget_Key");
            }
        }
    }

    /// <summary>
    /// ����һ����ʱ��̽��Ŀ��
    /// </summary>
    /// <param name="name">̽��Ŀ�������</param>
    /// <returns>һ��λ��Agent�Ҳ�Զ����GameObject</returns>
    private GameObject CreateSearchTarget(string name)
    {
        GameObject searchTarget = new GameObject(name);
        // ��Ŀ��������Agent��ǰλ�õ��ұߺ�Զ�ĵط���������ǰ��
        searchTarget.transform.position = transform.position + Vector3.right * 100f;
        // �������������ʱ���󣬱����ڳ�������������
        Destroy(searchTarget, Time.deltaTime);
        return searchTarget;
    }
}