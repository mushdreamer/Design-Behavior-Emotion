using UnityEngine;
using System.Collections.Generic;

// 这是 PerceptionData 类的最终、最安全版本
public class PerceptionData
{
    // 通过在声明时直接初始化，确保这些列表永远不会是 null
    public List<Collectible> visibleCollectibles = new List<Collectible>();
    public List<Monster> visibleMonsters = new List<Monster>();

    public Door visibleDoor;
    public EntityStats playerStats;
    public bool hasKey;
}

// IAgentPolicy 接口保持不变
public interface IAgentPolicy
{
    GameObject DecideTarget(PerceptionData data);
}