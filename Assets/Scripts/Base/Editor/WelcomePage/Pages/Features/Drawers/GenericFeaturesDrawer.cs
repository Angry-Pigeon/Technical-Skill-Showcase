using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Base.Editor.WelcomePage.Pages.Features;
using System.IO;

namespace Base.Editor.WelcomePage.Pages.Features.Drawers
{
    public class GenericFeaturesDrawer : OdinValueDrawer<ProjectFeatures>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            Sirenix.Utilities.Editor.SirenixEditorGUI.BeginBox();

            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 30
            };
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(ValueEntry.SmartValue.Title, titleStyle, GUILayout.Height(40));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUIStyle descriptionStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
            {
                alignment = TextAnchor.UpperLeft,
                fontSize = 14
            };
            EditorGUILayout.LabelField(ValueEntry.SmartValue.Description, descriptionStyle);

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            string completionSymbol = ValueEntry.SmartValue.Completed 
                ? "<color=green>✔</color>" 
                : "<color=red>✖</color>";
            GUIStyle symbolStyle = new GUIStyle(EditorStyles.label)
            {
                richText = true,
                alignment = TextAnchor.MiddleCenter,
                fontSize = 24
            };
            EditorGUILayout.LabelField(completionSymbol, symbolStyle, GUILayout.Width(50));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.LabelField("References:", EditorStyles.boldLabel);
            foreach (var reference in ValueEntry.SmartValue.ReferenceToClasses)
            {
                string className = Path.GetFileNameWithoutExtension(reference.ClassAddress);

                GUIStyle linkStyle = new GUIStyle(EditorStyles.label)
                {
                    richText = true,
                    fontSize = 12
                };

                Color normalColor = new Color(0.02f, 0.83f, 1f);
                Color hoverColor = Color.yellow; 
                string normalHex = ColorUtility.ToHtmlStringRGB(normalColor); 
                string hoverHex = ColorUtility.ToHtmlStringRGB(hoverColor); 

                Rect rect = GUILayoutUtility.GetRect(new GUIContent(className), linkStyle);
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

                bool isHovered = rect.Contains(Event.current.mousePosition);
                string hexColor = isHovered ? hoverHex : normalHex;

                string linkText = $"<color=#{hexColor}>{className}</color>";

                if (GUI.Button(rect, linkText, linkStyle))
                {
                    UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(reference.ClassAddress);
                    if (asset != null)
                    {
                        EditorGUIUtility.PingObject(asset);
                        Selection.activeObject = asset;
                    }
                    else
                    {
                        Debug.LogWarning("Asset not found at path: " + reference.ClassAddress);
                    }
                }
            }

            if (ValueEntry.SmartValue.Actions != null && ValueEntry.SmartValue.Actions.Count > 0)
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Actions:", EditorStyles.boldLabel);
                foreach (var featureAction in ValueEntry.SmartValue.Actions)
                {
                    if (GUILayout.Button(featureAction.ActionName, GUILayout.Height(25)))
                    {
                        featureAction.Action?.Invoke();
                    }
                }
            }

            Sirenix.Utilities.Editor.SirenixEditorGUI.EndBox();
        }
    }
}
