using Lofelt.NiceVibrations;
using UnityEngine;
namespace Game.GameLogic.Managers.UISystems.SettingsPanel
{
    public class ToggleVibrationsButton : ToggleableButton
    {

        protected override void OnEnable()
        {
            isOn = PlayerPrefs.GetInt("HapticsEnabled", 1) == 1;
            OffSprite.enabled = !isOn;
            base.OnEnable();
        }

        protected override void OnButtonClicked()
        {
            base.OnButtonClicked();
            HapticController.hapticsEnabled = isOn;
            PlayerPrefs.SetInt("HapticsEnabled", isOn ? 1 : 0);
        }
    }
}