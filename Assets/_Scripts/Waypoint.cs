// Waypoint.cs
using UnityEngine;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour
{
    // 定义连接类型，方便AI知道该如何从一个点移动到另一个点
    public enum LinkType { Walk, Jump }

    [System.Serializable]
    public struct WaypointLink
    {
        public Waypoint neighbor;
        public LinkType linkType;
    }

    public List<WaypointLink> neighbors = new List<WaypointLink>();

    // 在编辑器中绘制连接线，方便关卡设计
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.3f);

        foreach (var link in neighbors)
        {
            if (link.neighbor == null) continue;

            // 根据连接类型绘制不同颜色的线
            switch (link.linkType)
            {
                case LinkType.Walk:
                    Gizmos.color = Color.green;
                    break;
                case LinkType.Jump:
                    Gizmos.color = Color.cyan;
                    break;
            }
            Gizmos.DrawLine(transform.position, link.neighbor.transform.position);
        }
    }
}