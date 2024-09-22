using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public int HP { get; set; } //HP
    public int Score { get; set; } //���_
    public Vector2Int Position { get; set; } //�v���C���[�̈ʒu�i�����l�ŊǗ��j

    public PlayerModel() //������
    {
        HP = 100;
        Score = 0;
        Position = new Vector2Int(0, 0); // �����ʒu
    }
}
