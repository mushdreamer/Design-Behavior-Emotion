// Waypoint.cs
using UnityEngine;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour
{
    // �����������ͣ�����AI֪������δ�һ�����ƶ�����һ����
    public enum LinkType { Walk, Jump }

    [System.Serializable]
    public struct WaypointLink
    {
        public Waypoint neighbor;
        public LinkType linkType;
    }

    public List<WaypointLink> neighbors = new List<WaypointLink>();

    // �ڱ༭���л��������ߣ�����ؿ����
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.3f);

        foreach (var link in neighbors)
        {
            if (link.neighbor == null) continue;

            // �����������ͻ��Ʋ�ͬ��ɫ����
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