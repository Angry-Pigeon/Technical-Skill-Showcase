using Tool_Development.SerializableScriptableObject.Scripts;
using UnityEngine;
namespace Game.GameLogic.Managers.SaveData
{
    [CreateAssetMenu(fileName = "GameDataSave", menuName = "Save/Game Data", order = 5)]
    public class GameDataSave : SerializedSaveData
    {
        public int Level;
        public int LevelPreview = 1;
    }
}