using System;
using System.Collections;
using System.Collections.Generic;
using Game.GameLogic.Managers;
using Game.GameLogic.Managers.SaveData;
using Game.GameLogic.Managers.UISystems;
using UnityEngine;
using Zenject;
namespace Game.GameLogic
{
    public class BootStrapper : MonoBehaviour
    {
        [Inject]
        private SaveSystemManager _saveSystemManager;
        [Inject]
        private LevelLoadingManager _sceneLoaderManager;
        [Inject]
        private UIManager _uiManager;
        
        [Inject]
        private BootStrapperContext bootStrapperContext;
        
        private List<Manager_Base> _managers = new List<Manager_Base>();

        private void Awake()
        {
            Managers.GameEvents.Game.Initialize(this);
        }

        private void Start()
        {
            StartCoroutine(IE_SimulateLoading());
        }

        private void OnDisable()
        {
            Dispose();
        }

        private IEnumerator IE_SimulateLoading()
        {
            yield return IE_Initialize();
            yield return IE_PostInitialize();
        }
        
        private IEnumerator IE_Initialize()
        {
            
            _managers = new List<Manager_Base>
            {
                _saveSystemManager,
                _sceneLoaderManager,
                _uiManager
            };
            _managers.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            
            yield return IE_InitializeManagers();
        }
        
        private IEnumerator IE_PostInitialize()
        {
            yield return IE_PostInitializeManagers();
            _uiManager.ShowPanel(UIPanelType.StartPanel);
        }
        
        private IEnumerator IE_Pause()
        {
            yield return IE_PauseManagers();
        }
        
        private IEnumerator IE_Resume()
        {
            yield return IE_ResumeManagers();
        }
        
        private IEnumerator IE_Tick()
        {
            yield return IE_TickManagers();
        }
        
        private IEnumerator IE_FixedTick()
        {
            yield return IE_FixedTickManagers();
        }
        
        private IEnumerator IE_LateTick()
        {
            yield return IE_LateTickManagers();
        }
        
        private void Dispose()
        {
            DisposeManagers();
        }
        
        private IEnumerator IE_InitializeManagers()
        {
            foreach (var manager in _managers)
            {
                yield return manager.Initialize();
            }
        }
        
        private IEnumerator IE_PostInitializeManagers()
        {
            foreach (var manager in _managers)
            {
                yield return manager.PostInitialize();
            }
        }
        
        private IEnumerator IE_PauseManagers()
        {
            yield return _saveSystemManager.Pause();
            yield return _sceneLoaderManager.Pause();
        }
        
        private IEnumerator IE_ResumeManagers()
        {
            yield return _saveSystemManager.Resume();
            yield return _sceneLoaderManager.Resume();
        }
        
        private IEnumerator IE_TickManagers()
        {
            yield return _saveSystemManager.Tick();
            yield return _sceneLoaderManager.Tick();
        }
        
        private IEnumerator IE_FixedTickManagers()
        {
            yield return _saveSystemManager.FixedTick();
            yield return _sceneLoaderManager.FixedTick();
        }
        
        private IEnumerator IE_LateTickManagers()
        {
            yield return _saveSystemManager.LateTick();
            yield return _sceneLoaderManager.LateTick();
        }
        
        private void DisposeManagers()
        {
            foreach (var manager in _managers)
            {
                manager.Dispose();
            }
        }
        
        public void StartGame()
        {
            StartCoroutine(IE_StartGame());
        }

        public IEnumerator IE_StartGame()
        {
            int currentLevel = _saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).Level;
            int previewLevel = _saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).LevelPreview;

            _uiManager.ShowPanel(UIPanelType.LoadingPanel);
            
            yield return _sceneLoaderManager.IE_LoadLevel(currentLevel);

            yield return new WaitForFixedUpdate();
            
            _uiManager.ShowPanel(UIPanelType.GamePanel);
            _uiManager.ShowPanel(UIPanelType.SettingsPanel, false);
            

        }

        public void NextLevel()
        {
            StartCoroutine(IE_NextLevel());
        }

        private IEnumerator IE_NextLevel()
        {
            _uiManager.ShowPanel(UIPanelType.LoadingPanel);
            
            yield return _sceneLoaderManager.IE_LoadNextLevel();

            yield return new WaitForFixedUpdate();
            
            _uiManager.ShowPanel(UIPanelType.GamePanel);
            _uiManager.ShowPanel(UIPanelType.SettingsPanel, false);
        }
        public void RestartLevel()
        {
            StartCoroutine(IE_RestartLevel());
        }
        
        private IEnumerator IE_RestartLevel()
        {
            int currentLevel = _saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).Level;
            int previewLevel = _saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).LevelPreview;

            _uiManager.ShowPanel(UIPanelType.LoadingPanel);
            
            yield return _sceneLoaderManager.IE_LoadLevel(currentLevel);

            yield return new WaitForFixedUpdate();
            
            _uiManager.ShowPanel(UIPanelType.GamePanel);
            _uiManager.ShowPanel(UIPanelType.SettingsPanel, false);
        }


        public void EndGame(bool win)
        {
            StartCoroutine(IE_EndGame(win));
        }
        
        private IEnumerator IE_EndGame(bool win)
        {
            if (win)
            {
                int currentLevel = _saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).Level;
                int previewLevel = _saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).LevelPreview;
                
                _saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).Level = currentLevel + 1;
                _saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).LevelPreview = previewLevel + 1;
                
                _uiManager.ShowPanel(UIPanelType.WinPanel);
                yield return null;
            }
            else
            {
                _uiManager.ShowPanel(UIPanelType.LosePanel);
                yield return null;
            }
        }


        public int GetPreviewLevel()
        {
            return _saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).LevelPreview;
        }
        
        public BootStrapperContext GetBootStrapperContext()
        {
            return bootStrapperContext;
        }
    }
}