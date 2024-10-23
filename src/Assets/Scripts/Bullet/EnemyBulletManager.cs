using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject bulletPrefab;
    private List<EnemyBulletController> activeBullets = new List<EnemyBulletController>();
    private Queue<EnemyBulletController> bulletPool = new Queue<EnemyBulletController>();
    public int maxBullets = 5;


    void Start()
    {
        //maxBullets分の弾を生成しておいて、必要な時以外は非表示にしておく。（オブジェクトプールで待機）
        for (int i = 0; i < maxBullets; ++i)
        {
            var bulletObject = Instantiate(bulletPrefab);
            var bulletController = bulletObject.GetComponent<EnemyBulletController>();
            bulletController.OnEnemyBulletHit = HandleBulletHit;
            bulletObject.gameObject.SetActive(false);
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
                Debug.Log("Bullet is lost!");
            }
        }
    }

    //発射ボタンが押されたら、弾を生成する(プレイヤー方向に直進)
    public void FireBullet(Vector2 position, PlayerController playerPosition)
    {
        if (bulletPool.Count > 0)
        {
            var bullet = bulletPool.Dequeue();
            bullet.Initialize(position, playerPosition, gameManager);
            bullet.gameObject.SetActive(true);
            activeBullets.Add(bullet);
        }
    }

    //発射ボタンが押されたら、弾を生成する(円形に発射)
    public void FireCircleBullet(Vector2 position, Vector2 velocity, PlayerController player)
    {
        if (bulletPool.Count > 0)
        {
            var bullet = bulletPool.Dequeue();
            bullet.InitializeCircleBullet(position, velocity, player, gameManager);
            bullet.gameObject.SetActive(true);
            activeBullets.Add(bullet);
        }
    }

    //コールバックが当たり判定から返ってきたら、DeactiveBullet関数を実行
    private void HandleBulletHit(EnemyBulletController bullet)
    {
        Debug.Log("撃墜");

        DeactiveBullet(bullet);
    }

    //弾を非表示にして、オブジェクトプールに戻す
    void DeactiveBullet(EnemyBulletController bullet)
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
}
