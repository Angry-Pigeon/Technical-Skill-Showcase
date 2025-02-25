using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
namespace Editor.Utility
{
    public class PlayerPrefsWindow : OdinMenuEditorWindow
    {
        
        [MenuItem("Tools/PlayerPrefs Editor")]
        public static void OpenWindow()
        {
            var window = GetWindow<PlayerPrefsWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
            tree.Config.DrawSearchToolbar = true;
            tree.Add("PlayerPrefsEditor", new PlayerPrefsEditor());
            tree.SortMenuItemsByName();
            return tree;
        }
    }
}