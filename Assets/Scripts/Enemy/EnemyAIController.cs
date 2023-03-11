using System.Collections;
using Player;
using ScriptableObjects;
using ScriptableObjects.Enemy;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyAIController : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private EnemyScriptableObj _enemySo;
        private GameManagerSo _gameManagerSo;
        private Vector3 _targetVector;
        private GameObject _player;
        internal int ID;
        private float _health;
        private bool _runOnce;
        private bool _attackCooldown;

        private void Awake()
        {
            _enemySo = Locator.Instance.enemySo;
            _gameManagerSo = Locator.Instance.gameManagerSo;
            ID = Random.Range(0, 1000);
            _enemySo.Enemies.Add(ID, new EnemyData(id: ID));
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            ApplySettings();
            _player = GameObject.FindWithTag("Player");
        }

        private void FixedUpdate()
        {
            if (_player != null && _enemySo.Enemies[ID].CurrentState != EnemyData.States.Die)
            {
                if (Vector3.Distance(transform.position, _player.transform.position) <= _enemySo.visionRange)
                {
                    if (_enemySo.Enemies[ID].CurrentState != EnemyData.States.Chasing &&
                        _enemySo.Enemies[ID].CurrentState != EnemyData.States.Attack)
                    {
                        _enemySo.Enemies[ID].SetState(EnemyData.States.Chasing);
                        _agent.stoppingDistance = 2f;
                    }

                    ChasePlayer();
                }
                else if (_enemySo.Enemies[ID].CurrentState == EnemyData.States.Patrolling)
                {
                    if (Mathf.Abs(_targetVector.x - transform.position.x) <= 1f &&
                        Mathf.Abs(_targetVector.z - transform.position.z) <= 1f)
                    {
                        SetDestination(Utilities.GetRandomTargetPosition(_enemySo.aiNavigationRange.x,
                            transform.position.y,
                            _enemySo.aiNavigationRange.z));
                    }
                }
                else if (!_runOnce)
                {
                    _runOnce = !_runOnce;
                    StartCoroutine(WaitForPatrolling());
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                if (!_attackCooldown)
                {
                    _attackCooldown = !_attackCooldown;
                    StartCoroutine(Attack());
                    other.transform.GetComponent<PlayerController>().OnDamageTaken(_enemySo.damageGiven);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                _enemySo.Enemies[ID].SetState(EnemyData.States.Chasing);
                _agent.isStopped = false;
            }
        }

        private IEnumerator Attack()
        {
            _enemySo.Enemies[ID].SetState(EnemyData.States.Attack);
            yield return new WaitForSeconds(100 / _enemySo.attackSpeed);
            _attackCooldown = !_attackCooldown;
        }

        private void ChasePlayer()
        {
            _agent.SetDestination(_player.transform.position);
        }

        private IEnumerator WaitForPatrolling()
        {
            _enemySo.Enemies[ID].SetState(EnemyData.States.Idle);
            _agent.isStopped = true;
            _agent.ResetPath();
            yield return new WaitForSeconds(_enemySo.secondsForIdleToPatrolling);
            _enemySo.Enemies[ID].SetState(EnemyData.States.Patrolling);
            _agent.isStopped = false;
            _agent.stoppingDistance = 0f;
            SetDestination(Utilities.GetRandomTargetPosition(_enemySo.aiNavigationRange.x, transform.position.y,
                _enemySo.aiNavigationRange.z));
            _runOnce = !_runOnce;
        }

        private void ApplySettings()
        {
            _targetVector = transform.position;
            _enemySo.Enemies[ID].Health = _enemySo.maxHealth;
            _agent.speed = _enemySo.maxSpeed;
        }

        private void SetDestination(Vector3 vector)
        {
            _targetVector = vector;
            _agent.SetDestination(vector);
        }

        public void OnDamageTaken(float damage)
        {
            _enemySo.Enemies[ID].Health -= damage;
            if (_enemySo.Enemies[ID].Health <= 0f)
            {
                _gameManagerSo.OnEnemyKilled?.Invoke(_enemySo.scoreValue);
                _enemySo.Enemies[ID].SetState(EnemyData.States.Die);
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}