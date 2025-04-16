using System.Collections;
using System.Collections.Generic;
using Game.GameLogic.Managers.SaveData;
using Tool_Development.SerializableScriptableObject.Scripts;
using UnityEngine;
using Zenject;
namespace Game.GameLogic.Managers
{
    public class LevelLoadingManager : Manager_Base
    {
        public LevelSettingsDatabase LevelSettingsDatabase;
        
        public GameObject LevelParent;
        
        [Inject]
        private SaveSystemManager _saveSystemManager;
        
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
        
        public LevelSettingsData GetCurrentlyLoadedLevelData()
        {
            return LevelSettingsDatabase.GetLevelSettingCycle(_saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).Level);
        }
        
        public IEnumerator IE_LoadLevel(int levelIndex)
        {
            ClearLevel();
            yield return new WaitForSeconds(0.2f);
            
            LevelSettingsData data = LevelSettingsDatabase.GetLevelSettingCycle(levelIndex);
            if (data == null)
            {
                Debug.LogError($"Level data at index of {levelIndex} is null.");
                yield break;
            }
            
            GameObject newLevelObject = Instantiate(data.LevelPrefab, LevelParent.transform);
            
            yield return new WaitForFixedUpdate();
        }
        
        public IEnumerator IE_LoadNextLevel()
        {
            ClearLevel();
            yield return new WaitForSeconds(0.2f);
            
            
            int nextLevelIndex = _saveSystemManager.GetSaveData<GameDataSave>(GameSaveType.GameData).Level;
            nextLevelIndex++;
            yield return IE_LoadLevel(nextLevelIndex);
        }

        private void ClearLevel()
        {
            for (int i = LevelParent.transform.childCount - 1; i >= 0; i--)
            {
                GameObject child = LevelParent.transform.GetChild(i).gameObject;
                #if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    Destroy(child);
                }
                else
                {
                    DestroyImmediate(child);
                }
                #else
                Destroy(child);
                #endif
                
                
            }
        }
    }
}