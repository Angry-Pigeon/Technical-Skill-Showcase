using System.Collections;
using System.Collections.Generic;
using Game.GameLogic.Managers;
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
        
        private List<Manager_Base> _managers = new List<Manager_Base>();
        
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
                _sceneLoaderManager
            };
            _managers.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            
            yield return IE_InitializeManagers();
        }
        
        private IEnumerator IE_PostInitialize()
        {
            yield return IE_PostInitializeManagers();
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
        
    }
}