using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameManager gameManager = (GameManager)target;
        if(GUILayout.Button("save data default"))
        {
            Debug.Log("save default");
            gameManager.SaveFileDefault();
        }
    }
}
