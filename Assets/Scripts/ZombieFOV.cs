#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ZombieAI))]
public class ZombieFOV : Editor
{
    private void OnSceneGUI()
    {
        ZombieAI fov = (ZombieAI)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewDistance);
        Vector3 viewAngleA = fov.dirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.dirFromAngle(+fov.viewAngle / 2, false);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewDistance);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewDistance);
    }
}
#endif