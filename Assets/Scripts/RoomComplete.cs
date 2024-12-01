using Edgar.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomComplete : DungeonGeneratorPostProcessingComponentGrid2D
{
    [SerializeField] public bool generationIsComplete;
    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        generationIsComplete = true;
    }
}