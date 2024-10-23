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
        //maxBullets���̒e�𐶐����Ă����āA�K�v�Ȏ��ȊO�͔�\���ɂ��Ă����B�i�I�u�W�F�N�g�v�[���őҋ@�j
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

    //���˃{�^���������ꂽ��A�e�𐶐�����(�v���C���[�����ɒ��i)
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

    //���˃{�^���������ꂽ��A�e�𐶐�����(�~�`�ɔ���)
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

    //�R�[���o�b�N�������蔻�肩��Ԃ��Ă�����ADeactiveBullet�֐������s
    private void HandleBulletHit(EnemyBulletController bullet)
    {
        Debug.Log("����");

        DeactiveBullet(bullet);
    }

    //�e���\���ɂ��āA�I�u�W�F�N�g�v�[���ɖ߂�
    void DeactiveBullet(EnemyBulletController bullet)
    {
        bullet.gameObject.SetActive(false);
        activeBullets.Remove(bullet);
        bulletPool.Enqueue(bullet);      
    }

    //�g�O���ǂ���
    bool IsOffScreen(Vector2 position)
    {
        return position.x < -23 || position.x > 23 || position.y < -11 || position.y > 11;
    }
}
