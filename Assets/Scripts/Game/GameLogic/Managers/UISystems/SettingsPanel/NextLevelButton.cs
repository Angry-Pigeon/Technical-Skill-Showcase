using UnityEngine;
using UnityEngine.UI;
namespace Game.GameLogic.Managers.UISystems.SettingsPanel
{
    public class NextLevelButton : ButtonBase
    {
        protected override void OnClick()
        {
            GameEvents.Game.NextLevel();
            base.OnClick();
        }
    }
}