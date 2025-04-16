using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Base.Editor.WelcomePage.Pages.Features;

namespace Base.Editor.WelcomePage.Pages
{
    public class FirstPageDrawer : OdinValueDrawer<FirstShowcasePage>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 30,
                alignment = TextAnchor.MiddleCenter
            };
            
            GUILayout.Space(20);
            GUILayout.Label(ValueEntry.SmartValue.title, titleStyle);
            GUILayout.Space(20);
            
            GUILayout.Label(ValueEntry.SmartValue.explanation, EditorStyles.wordWrappedLabel);
            GUILayout.Space(20);
            
            GUILayout.Label("Project Features:", EditorStyles.boldLabel);
            
            foreach (var item in ValueEntry.SmartValue.ProjectFeaturesList)
            {
                DrawProjectFeature(item);
            }
            
            GUILayout.Label("Game Features:", EditorStyles.boldLabel);
            
            foreach (var item in ValueEntry.SmartValue.GameFeaturesList)
            {
                DrawProjectFeature(item);
            }
            
        }
        private static void DrawProjectFeature(ProjectFeatures item)
        {

            EditorGUILayout.BeginHorizontal();
                
            EditorGUILayout.Toggle(item.Completed, GUILayout.Width(20));
                
            GUIStyle titleButtonStyle = new GUIStyle(EditorStyles.label)
            {
                richText = true
            };
                
            Color normalColor = new Color(0.02f, 0.83f, 1f);
            Color hoverColor = Color.yellow; 
                
            string hexNormal = ColorUtility.ToHtmlStringRGB(normalColor);
            string hexHover = ColorUtility.ToHtmlStringRGB(hoverColor);
                
            string titleText = $"<color=#{hexNormal}><u>{item.Title}</u></color>";
                
            Rect buttonRect = GUILayoutUtility.GetRect(new GUIContent(titleText), titleButtonStyle);
                
            EditorGUIUtility.AddCursorRect(buttonRect, MouseCursor.Link);
                
            bool isHovered = buttonRect.Contains(Event.current.mousePosition);
                
            string hexColor = isHovered ? hexHover : hexNormal;
            string formattedTitle = $"<color=#{hexColor}>{item.Title}</color>";
                
            if (GUI.Button(buttonRect, formattedTitle, titleButtonStyle))
            {
                TechnicalShowcaseOdinWindow.OpenPage(item.Title);
            }
                
            EditorGUILayout.EndHorizontal();
        }
    }
}
