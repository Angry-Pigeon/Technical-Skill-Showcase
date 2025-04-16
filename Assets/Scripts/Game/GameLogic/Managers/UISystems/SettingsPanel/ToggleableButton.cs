using System;
using UnityEngine;
using UnityEngine.UI;
namespace Game.GameLogic.Managers.UISystems.SettingsPanel
{
    public class ToggleableButton : MonoBehaviour
    {

        [field: SerializeField]
        public Image OffSprite { get; private set; }

        protected Button button;
        
        protected bool isOn = true;

        protected virtual void OnEnable()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
            OffSprite.enabled = !isOn;
        }

        protected virtual void OnDisable()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
        
        protected virtual void OnButtonClicked()
        {
            isOn = !isOn;
            OffSprite.enabled = !isOn;
        }

    }
}