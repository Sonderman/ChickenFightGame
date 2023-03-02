using UnityEngine;
using Random = UnityEngine.Random;

public static class Utilities
{
        // Y axis is steady
        public static Vector3 GetRandomTargetPosition(float rangeX, float axisY, float rangeZ)
        {
                return new Vector3(Random.Range(-rangeX, rangeX), axisY, Random.Range(-rangeZ, rangeZ));
        }
}