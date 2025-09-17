using UnityEngine;
using System.Linq;

public class SpeedrunnerPolicy : MonoBehaviour, IAgentPolicy
{
    public GameObject DecideTarget(PerceptionData data)
    {
        // 1. 如果已经有钥匙
        if (data.hasKey)
        {
            // 如果能看见门，目标就是门
            if (data.visibleDoor != null)
            {
                return data.visibleDoor.gameObject;
            }
            // 【核心修正】如果看不见门，返回一个假的“探索目标”
            // 这个目标位于Agent右侧很远的地方，驱动它向右探索
            else
            {
                // 创建一个临时的GameObject来代表探索方向
                GameObject searchTarget = new GameObject("SearchTarget");
                searchTarget.transform.position = transform.position + Vector3.right * 100f;
                // 在一帧后销毁它，避免场景中留下垃圾
                Destroy(searchTarget, Time.deltaTime);
                return searchTarget;
            }
        }
        // 2. 否则，目标就是钥匙
        else
        {
            Collectible key = data.visibleCollectibles.FirstOrDefault(c => c.type == Collectible.CollectibleType.Key);
            return (key != null) ? key.gameObject : null;
        }
    }
}