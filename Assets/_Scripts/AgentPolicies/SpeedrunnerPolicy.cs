using UnityEngine;
using System.Linq;

public class SpeedrunnerPolicy : MonoBehaviour, IAgentPolicy
{
    public GameObject DecideTarget(PerceptionData data)
    {
        // 1. ����Ѿ���Կ��
        if (data.hasKey)
        {
            // ����ܿ����ţ�Ŀ�������
            if (data.visibleDoor != null)
            {
                return data.visibleDoor.gameObject;
            }
            // ����������������������ţ�����һ���ٵġ�̽��Ŀ�ꡱ
            // ���Ŀ��λ��Agent�Ҳ��Զ�ĵط�������������̽��
            else
            {
                // ����һ����ʱ��GameObject������̽������
                GameObject searchTarget = new GameObject("SearchTarget");
                searchTarget.transform.position = transform.position + Vector3.right * 100f;
                // ��һ֡�������������ⳡ������������
                Destroy(searchTarget, Time.deltaTime);
                return searchTarget;
            }
        }
        // 2. ����Ŀ�����Կ��
        else
        {
            Collectible key = data.visibleCollectibles.FirstOrDefault(c => c.type == Collectible.CollectibleType.Key);
            return (key != null) ? key.gameObject : null;
        }
    }
}