using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace ScriptableObjects.Enemy
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
    public class EnemyScriptableObj : ScriptableObject
    {
        public Dictionary<int, EnemyData> Enemies = new();
        public float maxHealth = 100f;
        public float maxSpeed = 10f;
        public float attackSpeed = 50f;
        public float visionRange = 5f;
        public float secondsForIdleToPatrolling = 5f;
        public Vector3 aiNavigationRange = new Vector3(10f, 0f, 10f);
    }

    public class EnemyData
    {
        public enum States
        {
            Patrolling,
            Idle,
            Chasing,
            Attack
        }

        private int _id;
        public float Health;
        public States CurrentState = States.Idle;
        public UnityAction<States> AnimationChangeEvent;

        public EnemyData(int id)
        {
            _id = id;
        }

        public void SetState(States state)
        {
            CurrentState = state;
            AnimationChangeEvent?.Invoke(state);
        }
    }
}