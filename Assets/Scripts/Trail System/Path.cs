﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Path : MonoBehaviour
{

    private Transform[] nodes;

    private void Start()
    {
        nodes = GetComponentsInChildren<Transform>();
    }
    public Vector3 LinearPosition(int segment, float ratio)
    {
        Vector3 p1 = nodes[segment].position;
        Vector3 p2 = nodes[segment + 1].position;

        return Vector3.Lerp(p1, p2, ratio);
    }

    public Vector3 LinearPositionBack(int segment, float ratio)
    {
        Vector3 p1 = nodes[segment + 1].position;
        Vector3 p2 = nodes[segment].position;

        return Vector3.Lerp(p1, p2,ratio);
    }

    public Quaternion Orientation (int segment, float ratio)
    {
        Quaternion q1 = nodes[segment].rotation;
        Quaternion q2 = nodes[segment + 1].rotation;

        return Quaternion.Lerp(q1, q2, ratio);
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i <= nodes.Length; i++)
        {
            if (i < nodes.Length - 1)
            Handles.DrawDottedLine(nodes[i].position, nodes[i + 1].position,3.0f);

            if (i == nodes.Length -1)
                Handles.DrawDottedLine(nodes[i].position, nodes[0].position, 3.0f);
        }
    }
    public static string GetFileName (string path)
    {
        return path;
    }
}
