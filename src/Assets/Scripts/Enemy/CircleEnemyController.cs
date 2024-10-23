using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CircleEnemyController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletNum = 15;
    [SerializeField] private float fireInterval = 0.75f;

    GameManager gameManager;

    private float moveSpeed = 3f;

    float fireTimer;
    bool isArrived = false;

    void Start()
    {
        //スタート位置
        gameManager = FindObjectOfType<GameManager>();
        fireTimer = fireInterval;
    }

    void Update()
    {
        if (isArrived)
        {
            WaveMoveEnemy(Time.deltaTime);
        }
        else
        {
            MoveEnemy(Time.deltaTime);
        }
        

        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0)
        {
            FireCircleBullet();
            fireTimer = fireInterval;
        }

        if (gameManager.isArrived)
        {
            isArrived = true;
        }

        if (IsOffScreen(transform.position))
        {
            Destroy(gameObject);
            Debug.Log("Circle Enemy is lost!");
        }
    }

    //下移動
    void MoveEnemy(float time)
    {
        transform.Translate(Vector3.down * moveSpeed * time);
    }

    //波状移動
    void WaveMoveEnemy(float time_)
    {
        if (transform.position.x > 18 || transform.position.x < -18) 
        {
            moveSpeed *= -1;
        }      

        transform.Translate(Vector3.right * moveSpeed * time_);

    }

    void FireCircleBullet()
    {
        float angleStep = 360f / bulletNum;
        float angle = 0f;

        for (int i = 0; i < bulletNum; i++)
        {
            float bulletDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float bulletDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 bulletMoveVector = new Vector3(bulletDirX, bulletDirY, 0);
            Vector2 bulletDir = (bulletMoveVector - transform.position).normalized;

            gameManager.enemyBulletManager.FireCircleBullet(transform.position, bulletDir, gameManager.player);

            angle += angleStep;
        }
    }

    bool IsOffScreen(Vector2 position)
    {
        return position.x < -23 || position.x > 23 || position.y < -11;
    }

    
}
