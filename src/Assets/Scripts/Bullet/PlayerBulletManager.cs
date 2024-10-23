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
        //maxBullets���̒e�𐶐����Ă����āA�K�v�Ȏ��ȊO�͔�\���ɂ��Ă����B�i�I�u�W�F�N�g�v�[���őҋ@�j
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

    //���˃{�^���������ꂽ��A�e���v���C���[�̑O�ɗ���悤�ɐ�������
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

    //�R�[���o�b�N�������蔻�肩��Ԃ��Ă�����ADeactiveBullet�֐������s
    private void HandleBulletHit(PlayerBulletController bullet) 
    {
        Debug.Log("����");
        gameManager.UpdateScoreText(100);
        DeactiveBullet(bullet);
    }

    //�e���\���ɂ��āA�I�u�W�F�N�g�v�[���ɖ߂�
    void DeactiveBullet(PlayerBulletController bullet)
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

    //bulletPrefab�ɂ���EnemyList�ɁAEnemy�����݂��Ă��邩�̗L��
    List<EnemyController> GetEnemyList()
    {
        return FindObjectsOfType<EnemyController>().ToList();
    }

    CircleEnemyController GetCircleEnemy()
    {
        return FindObjectOfType<CircleEnemyController>();
    }
}
