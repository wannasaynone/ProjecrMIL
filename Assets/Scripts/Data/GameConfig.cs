using UnityEngine;

namespace ProjectMIL.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ProjectMIL/Data/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        public int AddStatusValuePerLevel { get { return addStatusValuePerLevel; } }
        [SerializeField] private int addStatusValuePerLevel;
    }
}