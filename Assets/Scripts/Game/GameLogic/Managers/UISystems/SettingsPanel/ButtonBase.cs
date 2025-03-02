using System;
using UnityEngine;
using UnityEngine.UI;
namespace Game.GameLogic.Managers.UISystems.SettingsPanel
{
    public class ButtonBase : MonoBehaviour
    {
        protected Button Button;

        protected void OnEnable()
        {
            Button = GetComponent<Button>();
            Button.onClick.AddListener(OnClick);
        }
        
        protected void OnDisable()
        {
            Button.onClick.RemoveListener(OnClick);
        }
        
        protected virtual void OnClick()
        {
            Debug.Log($"Button {Button.gameObject.name} clicked.");
        }
    }
}