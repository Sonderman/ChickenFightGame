using UnityEngine;
using UnityEngine.Events;


namespace ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObjects/Player")]
    public class PlayerScriptableObj : ScriptableObject
    {
        [Header("Variables")] public float maxHealth = 100f;
        public float health;
        public float speed = 1f;
        public float killScore;
        public float damageGiven;
        public bool isAnimationsAllowed = true;

        //---Events---
        public UnityAction OnUIUpdateNeeded;
        public UnityAction OnDie;

        public void Initialize()
        {
            health = maxHealth;
            killScore = 0f;
            OnUIUpdateNeeded = null;
            OnDie = null;
        }
    }
}