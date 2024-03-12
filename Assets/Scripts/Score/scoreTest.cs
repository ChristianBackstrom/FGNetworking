using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class scoreTest : MonoBehaviour
{

}
#if UNITY_EDITOR
[CustomEditor(typeof(scoreTest))]
public class scoreTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (var score in ScoreCounter.Scoreboard)
        {
            EditorGUILayout.TextField($"{SavedClientInformationManager.GetUserData(score.Key).userName} score: {score.Value}");
        }
    }
}
#endif