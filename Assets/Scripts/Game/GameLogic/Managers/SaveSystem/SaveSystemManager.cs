using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tool_Development.SerializableScriptableObject.Scripts;
namespace Game.GameLogic.Managers
{
    public enum GameSaveType
    {
        PlayerData,
        GameData
    }
    public class SaveSystemManager : Manager_Base
    {
        public Dictionary<GameSaveType, SerializedSaveData> SaveData = new Dictionary<GameSaveType, SerializedSaveData>();
        public override IEnumerator Initialize()
        {
            State = ManagerState.InitializationStarted;
            
            LoadAll();
            
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
            SaveAll();
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
            SaveAll();
            State = ManagerState.Disposed;
        }
        
        public T GetSaveData<T>(GameSaveType saveType) where T : SerializedSaveData
        {
            return (T) SaveData[saveType];
        }
        
        public void LoadAll()
        {
            foreach (KeyValuePair<GameSaveType, SerializedSaveData> serializedSaveData in SaveData)
            {
                serializedSaveData.Value.Load();
            }
        }
        
        public void SaveAll()
        {
            foreach (KeyValuePair<GameSaveType, SerializedSaveData> serializedSaveData in SaveData)
            {
                serializedSaveData.Value.Save();
            }
        }


        #if UNITY_EDITOR
        
        // [Button]
        // private void CreateAccessors()
        // {
        //     foreach (KeyValuePair<GameSaveType, SerializedSaveData> serializedSaveData in SaveSystem)
        //     {
        //         string accessorName = serializedSaveData.Key.ToString() + "Accessor";
        //         string accessorPath = "Assets/Scripts/Game/GameLogic/Managers/SaveSystemAccessors/" + accessorName + ".cs";
        //         string accessorContent = "using Tool_Development.SerializableScriptableObject.Scripts;\n" +
        //                                  "namespace Game.GameLogic.Managers\n" +
        //                                  "{\n" +
        //                                  "    public class " + accessorName + " : SaveDataAccessor<SerializedSaveData>\n" +
        //                                  "    {\n" +
        //                                  "        public override SerializedSaveData SaveSystem => SaveSystemManager.Instance.SaveSystem[GameSaveType." + serializedSaveData.Key + "];\n" +
        //                                  "    }\n" +
        //                                  "}\n";
        //         System.IO.File.WriteAllText(accessorPath, accessorContent);
        //     }
        // }
        
  #endif
    }
}