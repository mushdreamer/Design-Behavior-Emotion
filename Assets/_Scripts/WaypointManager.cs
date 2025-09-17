// WaypointManager.cs
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; private set; }

    private List<Waypoint> allWaypoints;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // 找到场景中所有的路径点
            allWaypoints = FindObjectsOfType<Waypoint>().ToList();
        }
    }

    // A* 寻路算法的公开接口
    public List<Waypoint> FindPath(Vector3 startPos, Vector3 endPos)
    {
        Waypoint startNode = FindNearestWaypoint(startPos);
        Waypoint endNode = FindNearestWaypoint(endPos);

        if (startNode == null || endNode == null)
        {
            Debug.LogWarning("Could not find start or end waypoint.");
            return null;
        }

        List<Waypoint> openSet = new List<Waypoint>();
        HashSet<Waypoint> closedSet = new HashSet<Waypoint>();
        openSet.Add(startNode);

        Dictionary<Waypoint, Waypoint> cameFrom = new Dictionary<Waypoint, Waypoint>();
        Dictionary<Waypoint, float> gScore = new Dictionary<Waypoint, float>();
        gScore[startNode] = 0;

        Dictionary<Waypoint, float> fScore = new Dictionary<Waypoint, float>();
        fScore[startNode] = Vector3.Distance(startNode.transform.position, endNode.transform.position);

        while (openSet.Count > 0)
        {
            Waypoint current = openSet.OrderBy(node => fScore.ContainsKey(node) ? fScore[node] : float.MaxValue).First();

            if (current == endNode)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var link in current.neighbors)
            {
                Waypoint neighbor = link.neighbor;
                if (neighbor == null || closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + Vector3.Distance(current.transform.position, neighbor.transform.position);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= (gScore.ContainsKey(neighbor) ? gScore[neighbor] : float.MaxValue))
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Vector3.Distance(neighbor.transform.position, endNode.transform.position);
            }
        }

        return null; // Path not found
    }

    private List<Waypoint> ReconstructPath(Dictionary<Waypoint, Waypoint> cameFrom, Waypoint current)
    {
        List<Waypoint> totalPath = new List<Waypoint> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }

    public Waypoint FindNearestWaypoint(Vector3 position)
    {
        if (allWaypoints == null || allWaypoints.Count == 0) return null;
        return allWaypoints.OrderBy(waypoint => Vector3.Distance(position, waypoint.transform.position)).FirstOrDefault();
    }
}