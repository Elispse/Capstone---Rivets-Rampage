using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerState
{
    public List<string> weapons; // Save weapon names or IDs
    public float health;
    public string currentScene;
}