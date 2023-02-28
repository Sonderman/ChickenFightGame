using UnityEngine;

namespace ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/Player")]
    public class PlayerScriptableObj : ScriptableObject
    {
        public float MaxHealth { get; private set; } = 100f;
        public float health = 100f;
        public float speed = 100f;
        
    }
}
