using UnityEngine;
using UnityEngine.Events;


namespace ScriptableObjects.Player
{
    [CreateAssetMenu(fileName = "PlayerSIO", menuName = "ScriptableObjects/Player")]
    public class PlayerScriptableObj : ScriptableObject
    {
        [Header("Variables")] public float maxHealth = 100f;
        public float health;
        public float speed = 1f;
        public float killScore;

        public float damageGiven;

        //---Events---
        public UnityEvent onUIUpdateNeeded;
        public UnityEvent onDie;

        public void InitializePlayer()
        {
            health = maxHealth;
            killScore = 0f;
        }
    }
}