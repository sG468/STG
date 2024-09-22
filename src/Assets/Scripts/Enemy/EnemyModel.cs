using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel
{
    public int Health { get; set; }
    public Vector2Int Position { get; set; }

    public EnemyModel(int health, Vector2Int position)
    {
        Health = health;
        Position = position;
    }
}
