using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Base.Editor.WelcomePage
{
    public class TechnicalShowcaseOdinWindow : OdinMenuEditorWindow
    {
        [MenuItem("Window/Technical Showcase")]
        public static void OpenWindow()
        {
            GetWindow<TechnicalShowcaseOdinWindow>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();

            FirstShowcasePage firstShowcasePage = new FirstShowcasePage();
            tree.Add(firstShowcasePage.title, new FirstShowcasePage());
            
            firstShowcasePage.ProjectFeaturesList.ForEach(feature =>
            {
                tree.Add($"Project Features/{feature.Title}", feature);
            });
            
            firstShowcasePage.GameFeaturesList.ForEach(feature =>
            {
                tree.Add($"Game Features Features/{feature.Title}", feature);
            });

            return tree;
        }
        
        public static void OpenPage(string pageName)
        {
            var window = GetWindow<TechnicalShowcaseOdinWindow>();

            OdinMenuItem found = null;
            foreach (var rootItem in window.MenuTree.MenuItems)
            {
                found = FindPageRecursive(rootItem, pageName);
                if (found != null)
                    break;
            }

            if (found != null)
            {
                window.MenuTree.Selection.Clear();
                window.MenuTree.Selection.Add(found);
                window.Repaint();
            }
            else
            {
                Debug.Log("Page not found: " + pageName);
            }
        }

        private static OdinMenuItem FindPageRecursive(OdinMenuItem item, string pageName)
        {
            if (item.Name == pageName)
            {
                return item;
            }

            if (item.ChildMenuItems != null)
            {
                foreach (var child in item.ChildMenuItems)
                {
                    OdinMenuItem found = FindPageRecursive(child, pageName);
                    if (found != null)
                        return found;
                }
            }

            return null;
        }


    }

    [InitializeOnLoad]
    public static class AutoShowTechnicalShowcase
    {
        static AutoShowTechnicalShowcase()
        {
            EditorApplication.delayCall += () =>
            {
                TechnicalShowcaseOdinWindow.OpenWindow();
            };
        }
    }
}
