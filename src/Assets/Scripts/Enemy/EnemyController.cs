using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    private float fireInterval = 0.2f;
    private float moveSpeed = 5;

    GameManager gameManager;

    //敵の発射間隔用のタイマー
    
    float fireTimer;


    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        MoveEnemy(Time.deltaTime);

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0)
        {
            FireBullet();
            fireTimer = fireInterval;
        }

        if (IsOffScreen(transform.position))
        {
            Destroy(gameObject);
        }
    }

    //敵の移動処理
    void MoveEnemy(float time)
    {
        transform.Translate(Vector3.down * moveSpeed * time);
    }

    //プレイヤー方向に一直線に進む弾の発射
    void FireBullet()
    {
        gameManager.enemyBulletManager.FireBullet(transform.position, gameManager.player);
    }

    //枠外かどうかの判定
    bool IsOffScreen(Vector2 position)
    {
        return position.x < -23 || position.x > 23 || position.y < -11;
    }
}
