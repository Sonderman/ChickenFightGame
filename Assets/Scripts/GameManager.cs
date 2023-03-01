using ScriptableObjects;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] public GlobalSettings globalSettings;
    [SerializeField] public GameObject enemyPrefab;

    private void Start()
    {
        SpawnEnemies(globalSettings.enemyAmount);
    }

    private void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(enemyPrefab, GetRandomTargetPosition(40, 1f, 40), Quaternion.identity);
        }
    }

    private Vector3 GetRandomTargetPosition(float rangeX, float axisY, float rangeZ)
    {
        return new Vector3(Random.Range(-rangeX, rangeX), axisY, Random.Range(-rangeZ, rangeZ));
    }
}