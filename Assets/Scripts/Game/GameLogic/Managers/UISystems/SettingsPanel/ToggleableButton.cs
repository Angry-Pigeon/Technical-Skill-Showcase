using System;
using UnityEngine;
using UnityEngine.UI;
namespace Game.GameLogic.Managers.UISystems.SettingsPanel
{
    public class ToggleableButton : MonoBehaviour
    {
        [field: SerializeField]
        public Sprite OnSprite { get; private set; }

        [field: SerializeField]
        public Sprite OffSprite { get; private set; }

        protected Button button;
        
        protected bool isOn = true;

        private void OnEnable()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
        
        protected virtual void OnButtonClicked()
        {
            isOn = !isOn;
            button.image.sprite = isOn ? OnSprite : OffSprite;
        }

    }
}