using UnityEngine;
namespace Game.GameLogic.Managers.UISystems.SettingsPanel
{
    public class StartGameButton : ButtonBase
    {
        protected override void OnClick()
        {
            GameEvents.Game.StartGame();
            base.OnClick();
        }
    }
}