using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyModel enemyModel;
    private EnemyView enemyView;
    private int moveSpeed = 1; // 整数単位での移動速度

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
        // 整数単位での移動処理
        enemyModel.Position += new Vector2Int(0, -moveSpeed);
    }

    void UpdateView()
    {
        // モデルの位置をビューに反映
        enemyView.UpdatePosition(enemyModel.Position);
    }
}
