﻿using UnityEngine;

namespace Juce.CoreUnity.Guizmos
{
    public static class GizmosUtils
    {
        public static void DrawLine(Vector3 from, Vector3 to, Color color)
        {
            Color lastColor = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(from, to);
            Gizmos.color = lastColor;
        }

        public static void DrawCircle(Vector3 center, Vector3 normal, float radius, Color color)
        {
            Color lastColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawWireDisc(center, normal, radius);
            UnityEditor.Handles.color = lastColor;
        }
    }
}