using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public Vector3 GetRandomDestination(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector3(x, y, 0);
    }

    public float GetDistanceBetweenTwoPoints(Transform point1, Transform point2)
    {
        return Mathf.Sqrt(Mathf.Pow(point2.position.x - point1.position.x, 2) + Mathf.Pow(point2.position.y - point1.position.y, 2));
    }
}