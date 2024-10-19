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
}
