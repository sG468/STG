using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModel
{
    public Vector2Int Position { get; set; } //位置
    public Vector2Int Velocity { get; set; } //方向

    public BulletModel(Vector2Int initialPosition, Vector2Int initialVelocity)
    {
        Position = initialPosition;
        Velocity = initialVelocity;
    }

    // 弾の位置を更新する
    public void UpdatePosition()
    {
        Position += Velocity;
    }
}
