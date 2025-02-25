using System.Collections;
using System.Collections.Generic;
using Editor.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

[InitializeOnLoad]
public class TimescaleController
{
    private static float timeScaleValue = 1;
    private static bool resetOnExit = true;

    static TimescaleController()
    {
        ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        EditorPlaystateEvent.OnPlayModeExit += OnSceneClosed;
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();

        GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
        labelStyle.alignment = TextAnchor.MiddleCenter;
        labelStyle.fontSize = 12;
        labelStyle.fontStyle = FontStyle.Bold;

        GUILayout.BeginHorizontal();

        // Create a smaller horizontal section for the reset label and toggle
        GUILayout.BeginHorizontal(GUILayout.Width(50));
        GUILayout.Label("Reset", GUILayout.Width(40));  // Adjust width as needed
        resetOnExit = EditorGUILayout.Toggle(resetOnExit, GUILayout.Width(20));
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Label("Time", labelStyle, GUILayout.Height(16));
        GUILayout.EndVertical();

        GUILayout.Space(10);

        GUILayout.BeginVertical();
        timeScaleValue = EditorGUILayout.Slider(timeScaleValue, 0, 10, GUILayout.Width(180));
        GUILayout.EndVertical();

        GUILayout.Space(10);

        GUILayout.EndHorizontal();

        Time.timeScale = timeScaleValue;
    }


    static void OnSceneClosed()
    {
        // Reset values when the scene is closed
        if (resetOnExit)
        {
            timeScaleValue = 1f;
            Time.timeScale = timeScaleValue; // Apply reset to Time.timeScale
            QualitySettings.vSyncCount = 1;
        }
    
        // Unsubscribe from the event to avoid multiple subscriptions
        EditorPlaystateEvent.OnPlayModeExit -= OnSceneClosed;
    }


    // Static property to access and modify the timeScaleValue
    public static float TimeScaleValue
    {
        get { return timeScaleValue; }
        set
        {
            timeScaleValue = Mathf.Clamp(value, 0f, 10f); // Clamp between 0 and 10
            Time.timeScale = timeScaleValue; // Apply to the time scale
        }
    }
}