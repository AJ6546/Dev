using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrolPath : MonoBehaviour
{
    [SerializeField] float waypointRadius = 0.5f;
    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextWaypoint(i);
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(transform.GetChild(i).position, waypointRadius);
            Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(j));
        }
    }

    public int GetNextWaypoint(int i)
    {
        if (i + 1 == transform.childCount)
        {
            return 0;
        }
        return i + 1;
    }
    public Vector3 GetWaypointPosition(int i)
    {
        return transform.GetChild(i).position;
    }
}
