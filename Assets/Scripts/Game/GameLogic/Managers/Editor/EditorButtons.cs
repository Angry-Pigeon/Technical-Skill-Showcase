using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;
namespace Game.GameLogic.Managers.Editor
{
    [InitializeOnLoad]
    public class EditorButtons
    {
        static EditorButtons()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }
        
        static void OnToolbarGUI()
        {
            if(!Application.isPlaying) return;
            GUILayout.FlexibleSpace();

            GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 12;
            labelStyle.fontStyle = FontStyle.Bold;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("W", labelStyle, GUILayout.Width(30), GUILayout.Height(30)))
            {
                GameEvents.Game.EndGame(true);
            }
            if (GUILayout.Button("L", labelStyle, GUILayout.Width(30), GUILayout.Height(30)))
            {
                GameEvents.Game.EndGame(false);
            }
            
            GUILayout.EndHorizontal();
        }
    }
}