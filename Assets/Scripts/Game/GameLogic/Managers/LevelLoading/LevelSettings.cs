using Testing.HoleSystem.Scripts.HoleLogic;
using UnityEngine;
namespace Game.GameLogic
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/Level/Level Settings", order = 1)]
    public class LevelSettings : ScriptableObject
    {
        [field: SerializeField]
        public HoleData HoleData { get; private set; }

        [field: SerializeField]
        public float LevelTimer { get; private set; }

        [field: SerializeField]
        public int LevelGoal { get; private set; }

        public HoleData GetHoleData()
        {
            return HoleData;
        }

    }
}