using UnityEngine;
namespace Game.GameLogic.Managers.UISystems.SettingsPanel
{
    public class RestartLevelButton : ButtonBase
    {
        protected override void OnClick()
        {
            GameEvents.Game.RestartLevel();
            base.OnClick();
        }
    }
}