using UnityEngine;
using System.Collections.Generic;

// ���� PerceptionData ������ա��ȫ�汾
public class PerceptionData
{
    // ͨ��������ʱֱ�ӳ�ʼ����ȷ����Щ�б���Զ������ null
    public List<Collectible> visibleCollectibles = new List<Collectible>();
    public List<Monster> visibleMonsters = new List<Monster>();

    public Door visibleDoor;
    public EntityStats playerStats;
    public bool hasKey;
}

// IAgentPolicy �ӿڱ��ֲ���
public interface IAgentPolicy
{
    GameObject DecideTarget(PerceptionData data);
}