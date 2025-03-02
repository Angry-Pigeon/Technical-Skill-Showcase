using System.Collections;
using UnityEngine;
namespace Game.GameLogic.Managers.UISystems
{
    public enum UIPanelType
    {
        StartPanel,
        GamePanel,
        SettingsPanel,
        WinPanel,
        LosePanel
    }
    public class UIManager : Manager_Base
    {
        public GameObject Canvas;
        public CanvasGroup StartPanel;
        public CanvasGroup GamePanel;
        public CanvasGroup SettingsPanel;
        public CanvasGroup WinPanel;
        public CanvasGroup LosePanel;
        
        public override IEnumerator Initialize()
        {
            State = ManagerState.InitializationStarted;
            yield return null;
            State = ManagerState.Initialized;
        }
        public override IEnumerator PostInitialize()
        {
            State = ManagerState.PostInitializationStarted;
            yield return null;
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
    }
}