using System.Collections;
using ScriptableObjects.Enemy;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyAIController : MonoBehaviour
    {
        private NavMeshAgent _agent;
        [SerializeField] public EnemyScriptableObj enemySio;
        private Vector3 _targetVector;
        private GameObject _player;
        internal int ID;
        private float _health;
        private bool _runOnce;
        private bool _attackCooldown;

        private void Awake()
        {
            ID = Random.Range(0, 1000);
            enemySio.Enemies.Add(ID, new EnemyData(id: ID));
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            ApplySettings();
            _player = GameObject.FindWithTag("Player");
        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(transform.position, _player.transform.position) <= enemySio.visionRange)
            {
                if (enemySio.Enemies[ID].CurrentState != EnemyData.States.Chasing &&
                    enemySio.Enemies[ID].CurrentState != EnemyData.States.Attack)
                {
                    enemySio.Enemies[ID].SetState(EnemyData.States.Chasing);
                    _agent.stoppingDistance = 2f;
                }

                ChasePlayer();
            }
            else if (enemySio.Enemies[ID].CurrentState == EnemyData.States.Patrolling)
            {
                if (Mathf.Abs(_targetVector.x - transform.position.x) <= 1f &&
                    Mathf.Abs(_targetVector.z - transform.position.z) <= 1f)
                {
                    MoveEnemy(GetRandomTargetPosition(enemySio.aiNavigationRange.x, transform.position.y,
                        enemySio.aiNavigationRange.z));
                }
            }
            else if (!_runOnce)
            {
                _runOnce = !_runOnce;
                StartCoroutine(WaitForPatrolling());
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
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag.Equals("Player"))
            {
                enemySio.Enemies[ID].SetState(EnemyData.States.Chasing);
                _agent.isStopped = false;
            }
        }

        private IEnumerator Attack()
        {
            enemySio.Enemies[ID].SetState(EnemyData.States.Attack);
            yield return new WaitForSeconds(100 / enemySio.attackSpeed);
            _attackCooldown = !_attackCooldown;
        }

        private void ChasePlayer()
        {
            _agent.SetDestination(_player.transform.position);
        }

        private IEnumerator WaitForPatrolling()
        {
            enemySio.Enemies[ID].SetState(EnemyData.States.Idle);
            _agent.isStopped = true;
            _agent.ResetPath();
            yield return new WaitForSeconds(enemySio.secondsForIdleToPatrolling);
            enemySio.Enemies[ID].SetState(EnemyData.States.Patrolling);
            _agent.isStopped = false;
            _agent.stoppingDistance = 0f;
            MoveEnemy(GetRandomTargetPosition(enemySio.aiNavigationRange.x, transform.position.y,
                enemySio.aiNavigationRange.z));
            _runOnce = !_runOnce;
        }

        private void ApplySettings()
        {
            _targetVector = transform.position;
            enemySio.Enemies[ID].Health = enemySio.maxHealth;
            _agent.speed = enemySio.maxSpeed;
        }

        private void MoveEnemy(Vector3 vector)
        {
            _targetVector = vector;
            _agent.SetDestination(vector);
        }

        private Vector3 GetRandomTargetPosition(float rangeX, float axisY, float rangeZ)
        {
            return new Vector3(Random.Range(-rangeX, rangeX), axisY, Random.Range(-rangeZ, rangeZ));
        }
    }
}