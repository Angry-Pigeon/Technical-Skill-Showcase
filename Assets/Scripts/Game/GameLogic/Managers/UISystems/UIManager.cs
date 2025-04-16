using System.Collections;
using System.Collections.Generic;
using Game.GameLogic.Managers.SaveData;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using Zenject;
namespace Game.GameLogic.Managers.UISystems
{
    public enum UIPanelType
    {
        StartPanel,
        GamePanel,
        SettingsPanel,
        WinPanel,
        LosePanel,
        LoadingPanel
    }
    public class UIManager : Manager_Base
    {
        public GameObject Canvas;
        public CanvasGroup StartPanel;
        public CanvasGroup GamePanel;
        public CanvasGroup SettingsPanel;
        public CanvasGroup WinPanel;
        public CanvasGroup LosePanel;
        public CanvasGroup LoadingPanel;
        
        public TMP_Text PreviewLevelText;
        
        private Dictionary<UIPanelType, CanvasGroup> _panels = new Dictionary<UIPanelType, CanvasGroup>();
        
        public override IEnumerator Initialize()
        {
            State = ManagerState.InitializationStarted;
            
            _panels = new Dictionary<UIPanelType, CanvasGroup>
            {
                {UIPanelType.StartPanel, StartPanel},
                {UIPanelType.GamePanel, GamePanel},
                {UIPanelType.SettingsPanel, SettingsPanel},
                {UIPanelType.WinPanel, WinPanel},
                {UIPanelType.LosePanel, LosePanel},
                {UIPanelType.LoadingPanel, LoadingPanel}
            };
            
            ShowPanel(UIPanelType.LoadingPanel);
            
            yield return null;
            State = ManagerState.Initialized;
        }
        public override IEnumerator PostInitialize()
        {
            State = ManagerState.PostInitializationStarted;
            yield return null;
            PreviewLevelText.text = $"Level :{GameEvents.Game.GetPreviewLevel()}";
            
            State = ManagerState.PostInitialized;
        }
        public override IEnumerator Pause()
        {
            State = ManagerState.PauseStarted;
            yield return null;
            State = ManagerState.Paused;
        }
        public override IEnumerator Resume()
        {
            State = ManagerState.ResumeStarted;
            yield return null;
            State = ManagerState.Resumed;
        }
        public override IEnumerator Tick()
        {
            yield return null;
        }
        public override IEnumerator FixedTick()
        {
            yield return null;
        }
        public override IEnumerator LateTick()
        {
            yield return null;
        }
        public override void Dispose()
        {
            State = ManagerState.Disposed;
        }
        
        public CanvasGroup GetPanel(UIPanelType panelType)
        {
            if (!_panels.ContainsKey(panelType))
            {
                Debug.LogError($"Panel with type {panelType} not found.");
                return null;
            }
            return _panels[panelType];
        }
        
        public void ShowPanel(UIPanelType panelType, bool hideOtherPanels = true)
        {
            if (hideOtherPanels)
            {
                _panels.ForEach( panel => panel.Value.SetActive(false));
            }
            var panel = GetPanel(panelType);
            if (panel == null) return;
            panel.SetActive(true);
        }
        
        public void HidePanel(UIPanelType panelType)
        {
            var panel = GetPanel(panelType);
            if (panel == null) return;
            panel.SetActive(false);
        }
    }
}