using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor (typeof(EnemyAI))]
public class FOVEditor : Editor
{

    private void OnSceneGUI()
    {
        EnemyAI fov = (EnemyAI)target;
        Handles.color = Color.white;

        Vector3 viewangleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewangleB = fov.DirFromAngle(fov.viewAngle / 2, false);
        Handles.DrawWireArc(fov.transform.position, Vector3.up, viewangleA, fov.viewAngle, fov.viewRadius);

        Handles.DrawLine(fov.transform.position, fov.transform.position + viewangleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewangleB * fov.viewRadius);


    }

}
