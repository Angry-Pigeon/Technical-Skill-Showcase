using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Game.GameLogic
{
    [CreateAssetMenu(fileName = "LevelSettingsDatabase", menuName = "Game/Level/Level Settings Database", order = 0)]
    public class LevelSettingsDatabase : SerializedResourcedScriptableObject<LevelSettingsDatabase>
    {
        public LevelSettingsData DefaultLevelSettingsData;
        [TableList]
        public List<LevelSettingsData> LevelSettingsData;
        
        public LevelSettings GetLevelSettings(int level)
        {
            
            if(level == -1) return DefaultLevelSettingsData.LevelSettings;
            
            LevelSettingsData levelSettingsData = LevelSettingsData.Find(x => x.Level == level);
            if (levelSettingsData == null)
            {
                return DefaultLevelSettingsData.LevelSettings;
            }
            return levelSettingsData.LevelSettings;
        }
        
        public LevelSettingsData GetLevelSettingCycle(int level)
        {
            if (level == -1) return DefaultLevelSettingsData;
            LevelSettingsData levelSettingsData = LevelSettingsData.Find(x => x.Level == level);
            if (levelSettingsData == null)
            {
                return DefaultLevelSettingsData;
            }
            
            if(level > LevelSettingsData.Count)
            {
                level = level % LevelSettingsData.Count;
                levelSettingsData = LevelSettingsData.Find(x => x.Level == level);
            }
            
            return levelSettingsData;
        }
        
        [Button]
        public void ReOrderLevels()
        {
            LevelSettingsData = LevelSettingsData.OrderBy(x => x.Level).ToList();
        }
        
    }

    [System.Serializable]
    public class LevelSettingsData
    {
        [field: SerializeField]
        public int Level { get; private set; } = 0;
        [field: SerializeField]
        public LevelSettings LevelSettings { get; private set; }
        [field: SerializeField]
        public GameObject LevelPrefab { get; private set; }

        

        
    }
}