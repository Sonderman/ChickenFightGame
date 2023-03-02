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
        public EnemyScriptableObj enemySo;
        public GameManagerSo gameManagerSo;
        private Vector3 _targetVector;
        private GameObject _player;
        internal int ID;
        private float _health;
        private bool _runOnce;
        private bool _attackCooldown;

        private void Awake()
        {
            ID = Random.Range(0, 1000);
            enemySo.Enemies.Add(ID, new EnemyData(id: ID));
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            ApplySettings();
            _player = GameObject.FindWithTag("Player");
        }

        private void FixedUpdate()
        {
            if (_player != null&&enemySo.Enemies[ID].CurrentState!= EnemyData.States.Die )
            {
                if (Vector3.Distance(transform.position, _player.transform.position) <= enemySo.visionRange)
                {
                    if (enemySo.Enemies[ID].CurrentState != EnemyData.States.Chasing &&
                        enemySo.Enemies[ID].CurrentState != EnemyData.States.Attack)
                    {
                        enemySo.Enemies[ID].SetState(EnemyData.States.Chasing);
                        _agent.stoppingDistance = 2f;
                    }

                    ChasePlayer();
                }
                else if (enemySo.Enemies[ID].CurrentState == EnemyData.States.Patrolling)
                {
                    if (Mathf.Abs(_targetVector.x - transform.position.x) <= 1f &&
                        Mathf.Abs(_targetVector.z - transform.position.z) <= 1f)
                    {
                        SetDestination(Utilities.GetRandomTargetPosition(enemySo.aiNavigationRange.x, transform.position.y,
                            enemySo.aiNavigationRange.z));
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
                    other.transform.GetComponent<PlayerController>().OnDamageTaken(enemySo.damageGiven);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                enemySo.Enemies[ID].SetState(EnemyData.States.Chasing);
                _agent.isStopped = false;
            }
        }

        private IEnumerator Attack()
        {
            enemySo.Enemies[ID].SetState(EnemyData.States.Attack);
            yield return new WaitForSeconds(100 / enemySo.attackSpeed);
            _attackCooldown = !_attackCooldown;
        }

        private void ChasePlayer()
        {
            _agent.SetDestination(_player.transform.position);
        }

        private IEnumerator WaitForPatrolling()
        {
            enemySo.Enemies[ID].SetState(EnemyData.States.Idle);
            _agent.isStopped = true;
            _agent.ResetPath();
            yield return new WaitForSeconds(enemySo.secondsForIdleToPatrolling);
            enemySo.Enemies[ID].SetState(EnemyData.States.Patrolling);
            _agent.isStopped = false;
            _agent.stoppingDistance = 0f;
            SetDestination(Utilities.GetRandomTargetPosition(enemySo.aiNavigationRange.x, transform.position.y,
                enemySo.aiNavigationRange.z));
            _runOnce = !_runOnce;
        }

        private void ApplySettings()
        {
            _targetVector = transform.position;
            enemySo.Enemies[ID].Health = enemySo.maxHealth;
            _agent.speed = enemySo.maxSpeed;
        }

        private void SetDestination(Vector3 vector)
        {
            _targetVector = vector;
            _agent.SetDestination(vector);
        }

        public void OnDamageTaken(float damage)
        {
            enemySo.Enemies[ID].Health -= damage;
            if (enemySo.Enemies[ID].Health <= 0f)
            {
                gameManagerSo.OnEnemyKilled?.Invoke(enemySo.scoreValue);
                enemySo.Enemies[ID].SetState(EnemyData.States.Die);
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}