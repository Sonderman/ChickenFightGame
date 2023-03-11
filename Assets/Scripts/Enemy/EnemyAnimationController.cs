using ScriptableObjects.Enemy;
using UnityEngine;

namespace Enemy
{
    public class EnemyAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private EnemyAIController _enemyAI;
        private bool _attackLeft=true;

        private void Start()
        {
            _enemyAI = GetComponent<EnemyAIController>();
            _animator = GetComponent<Animator>();
            Locator.Instance.enemySo.Enemies[_enemyAI.ID].AnimationChangeEvent += OnAnimationChanged;
        }

        private void OnAnimationChanged(EnemyData.States state)
        {
            if (state == EnemyData.States.Idle)
            {
                _animator.SetBool("IsRunning", false);
            }

            if (state == EnemyData.States.Chasing
                || state == EnemyData.States.Patrolling)
            {
                _animator.SetBool("IsRunning", true);
            }

            if (state == EnemyData.States.Attack)
            {
                _animator.SetTrigger(_attackLeft ? "PunchL" : "PunchR");
                _animator.SetBool("IsRunning", false);
                _attackLeft = !_attackLeft;
            }

            if (state == EnemyData.States.Die)
            {
                _animator.SetTrigger("Die");
            }
        }
    }
}