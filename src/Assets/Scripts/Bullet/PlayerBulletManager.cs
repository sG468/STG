using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBulletManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject bulletPrefab;
    private List<PlayerBulletController> activeBullets = new List<PlayerBulletController>();
    private Queue<PlayerBulletController> bulletPool = new Queue<PlayerBulletController>();
    public int maxBullets = 5;

 
    void Start()
    {
        //maxBullets分の弾を生成しておいて、必要な時以外は非表示にしておく。（オブジェクトプールで待機）
        for (int i = 0; i < maxBullets; ++i)
        {
            var bulletObject = Instantiate(bulletPrefab);
            var bulletController = bulletObject.GetComponent<PlayerBulletController>();
            bulletController.OnPlayerBulletHit = HandleBulletHit;
            bulletObject.SetActive(false);
            bulletPool.Enqueue(bulletController);
        }
    }

    void Update()
    {
        for (int i = 0; i < activeBullets.Count; ++i)
        {
            var bullet = activeBullets[i];

            if (IsOffScreen(bullet.transform.position))
            {
                DeactiveBullet(bullet);
            }
        }
    }

    //発射ボタンが押されたら、弾をプレイヤーの前に来るように生成する
    public void FireBullet(Vector2 position, Vector2 direction)
    {
        if (bulletPool.Count > 0)
        {
            var bullet = bulletPool.Dequeue();
            bullet.Initialize(position, direction, GetEnemyList(), GetCircleEnemy(), gameManager);
            bullet.gameObject.SetActive(true);
            activeBullets.Add(bullet);
        }
    }

    //コールバックが当たり判定から返ってきたら、DeactiveBullet関数を実行
    private void HandleBulletHit(PlayerBulletController bullet) 
    {
        Debug.Log("撃墜");
        gameManager.UpdateScoreText(100);
        DeactiveBullet(bullet);
    }

    //弾を非表示にして、オブジェクトプールに戻す
    void DeactiveBullet(PlayerBulletController bullet)
    {
        bullet.gameObject.SetActive(false);
        activeBullets.Remove(bullet);
        bulletPool.Enqueue(bullet);
    }

    //枠外かどうか
    bool IsOffScreen(Vector2 position)
    {
        return position.x < -23 || position.x > 23 || position.y < -11 || position.y > 11;
    }

    //bulletPrefabにあるEnemyListに、Enemyが存在しているかの有無
    List<EnemyController> GetEnemyList()
    {
        return FindObjectsOfType<EnemyController>().ToList();
    }

    CircleEnemyController GetCircleEnemy()
    {
        return FindObjectOfType<CircleEnemyController>();
    }
}
