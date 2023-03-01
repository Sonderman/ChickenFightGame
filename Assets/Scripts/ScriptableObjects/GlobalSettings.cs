using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GlobalSettingsSIO", menuName = "ScriptableObjects/GlobalSettings")]
    public class GlobalSettings : ScriptableObject
    {
        public int enemyAmount = 5;
        public float musicVolume = 50f;
    }
}