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

    //�G�̔��ˊԊu�p�̃^�C�}�[
    
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

    //�G�̈ړ�����
    void MoveEnemy(float time)
    {
        transform.Translate(Vector3.down * moveSpeed * time);
    }

    //�v���C���[�����Ɉ꒼���ɐi�ޒe�̔���
    void FireBullet()
    {
        gameManager.enemyBulletManager.FireBullet(transform.position, gameManager.player);
    }

    //�g�O���ǂ����̔���
    bool IsOffScreen(Vector2 position)
    {
        return position.x < -23 || position.x > 23 || position.y < -11;
    }
}
