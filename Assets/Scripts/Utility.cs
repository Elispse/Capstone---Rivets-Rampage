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

    public float GetDistanceBetweenTwoPoints(Vector3 point1, Vector3 point2)
    {
        return Mathf.Sqrt(Mathf.Pow(point2.x - point1.x, 2) + Mathf.Pow(point2.y - point1.y, 2));
    }

    public Quaternion GetAngleBetweenTwoPoints(Vector3 point1, Vector3 point2)
    {
        // Calculate the direction vector from pointA to pointB
        Vector3 direction = point2 - point1;

        // Normalize the direction vector
        direction.Normalize();

        // Create a quaternion that rotates from the forward direction to the calculated direction
        Quaternion rotation = Quaternion.LookRotation(direction);

        return rotation;
    } 

    public Vector3 GetRandomPositionAroundObject(Vector3 center, float radius, float minAngle, float maxAngle)
    {
        // Convert degrees to radians
        float minAngleRad = minAngle * Mathf.Deg2Rad;
        float maxAngleRad = maxAngle * Mathf.Deg2Rad;

        // Generate a random angle within the specified range
        float randomAngle = Random.Range(minAngleRad, maxAngleRad);

        // Calculate the x and y offsets using polar coordinates
        float xOffset = radius * Mathf.Cos(randomAngle);
        float yOffset = radius * Mathf.Sin(randomAngle);

        // Add the offsets to the game object's position
        Vector3 randomPosition = center + new Vector3(xOffset, yOffset, 0);

        return randomPosition;
    }
}