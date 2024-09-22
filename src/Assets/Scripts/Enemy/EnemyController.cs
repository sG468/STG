using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyModel enemyModel;
    private EnemyView enemyView;
    private int moveSpeed = 1; // �����P�ʂł̈ړ����x

    void Start()
    {
        enemyModel = new EnemyModel(100, new Vector2Int(0, 5));
        enemyView = GetComponent<EnemyView>();
    }

    void Update()
    {
        MoveEnemy();
        UpdateView();
    }

    void MoveEnemy()
    {
        // �����P�ʂł̈ړ�����
        enemyModel.Position += new Vector2Int(0, -moveSpeed);
    }

    void UpdateView()
    {
        // ���f���̈ʒu���r���[�ɔ��f
        enemyView.UpdatePosition(enemyModel.Position);
    }
}
