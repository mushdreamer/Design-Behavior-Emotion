using UnityEngine;
using System.Linq;

public class SpeedrunnerPolicy : MonoBehaviour, IAgentPolicy
{
    /// <summary>
    /// AI的核心决策函数
    /// </summary>
    /// <param name="data">AI感知到的所有环境数据</param>
    /// <returns>当前应该前往的目标GameObject</returns>
    public GameObject DecideTarget(PerceptionData data)
    {
        // 核心优先级判断：是否已经持有钥匙？
        if (data.hasKey)
        {
            // --- 阶段2：已持有钥匙，目标是门 ---
            Debug.Log("[SpeedrunnerPolicy] 决策：已持有钥匙，正在寻找门...");

            // 如果能看见门，目标就是门
            if (data.visibleDoor != null)
            {
                Debug.Log("[SpeedrunnerPolicy] 决策：门在视野内，设为目标。");
                return data.visibleDoor.gameObject;
            }
            // 如果看不见门，则向右探索
            else
            {
                Debug.Log("[SpeedrunnerPolicy] 决策：门不在视野内，向右探索。");
                return CreateSearchTarget("SearchTarget_Door");
            }
        }
        else
        {
            // --- 阶段1：没有钥匙，目标是钥匙 ---
            Debug.Log("[SpeedrunnerPolicy] 决策：没有钥匙，正在寻找钥匙...");

            Collectible key = data.visibleCollectibles.FirstOrDefault(c => c.type == Collectible.CollectibleType.Key);

            // 如果能看见钥匙，目标就是钥匙
            if (key != null)
            {
                Debug.Log("[SpeedrunnerPolicy] 决策：钥匙在视野内，设为目标。");
                return key.gameObject;
            }
            // 如果看不见钥匙，则向右探索
            else
            {
                Debug.Log("[SpeedrunnerPolicy] 决策：钥匙不在视野内，向右探索。");
                return CreateSearchTarget("SearchTarget_Key");
            }
        }
    }

    /// <summary>
    /// 创建一个临时的探索目标
    /// </summary>
    /// <param name="name">探索目标的名称</param>
    /// <returns>一个位于Agent右侧远方的GameObject</returns>
    private GameObject CreateSearchTarget(string name)
    {
        GameObject searchTarget = new GameObject(name);
        // 将目标设置在Agent当前位置的右边很远的地方，驱动它前进
        searchTarget.transform.position = transform.position + Vector3.right * 100f;
        // 立即销毁这个临时对象，避免在场景中留下垃圾
        Destroy(searchTarget, Time.deltaTime);
        return searchTarget;
    }
}