using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletModel
{
    public Vector2Int Position { get; set; } //�ʒu
    public Vector2Int Velocity { get; set; } //����

    public BulletModel(Vector2Int initialPosition, Vector2Int initialVelocity)
    {
        Position = initialPosition;
        Velocity = initialVelocity;
    }

    // �e�̈ʒu���X�V����
    public void UpdatePosition()
    {
        Position += Velocity;
    }
}
