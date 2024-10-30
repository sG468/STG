using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{
    public System.Action<PlayerBulletController> OnPlayerBulletHit;
    [SerializeField] GameObject circleEnemyDestroyParticle;

    public PlayerBulletController bulletController;
    List<EnemyController> enemies;
    CircleEnemyController circleEnemy;

    private Vector3 Velocity;
    private float speed = 3;

    GameManager gameManager;

    //�e�Ɋ܂ޏ��̋L�^
    public void Initialize(Vector2 initialPosition, Vector2 initialVelocity, List<EnemyController> enemyList, CircleEnemyController circleEnemy_, GameManager gameManager_)
    {
        //�e�̍��W�ƕ�����^���A�G����n��
        transform.position = initialPosition;
        Velocity = initialVelocity;
        bulletController = GetComponent<PlayerBulletController>();

        gameManager = gameManager_;

        if (gameManager.EnemyTypeReference() == GameManager.EnemyType.circle)
        {
            circleEnemy = circleEnemy_;
        }
        else
        {
            enemies = enemyList;
        }
    }


    void Update()
    {

        // �e�̈ʒu���X�V
        //transform .position += Velocity * Time.deltaTime;
        transform.Translate(Velocity * speed * Time.deltaTime);

        if (gameManager.EnemyTypeReference() == GameManager.EnemyType.circle)
        {
            CheckCollisionCircleEnemy();
        }
        else
        {
            CheckCollisionEnemies();
        }
    }

    //�����蔻��
    void CheckCollisionEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                if (IsCollision(transform.position, enemy.transform.position))
                {
                    ShootDownNormalEnemy(enemy);
                    break;
                }
            }
            else
            {
                Debug.Log("Enemy is lost");
            } 
        }
    }

    void CheckCollisionCircleEnemy()
    {
        if (circleEnemy != null)
        {
            if (IsCollision(transform.position, circleEnemy.transform.position))
            {
                ShootDownCircleEnemy(circleEnemy);
            }
        }
        else
        {
            Debug.Log("Enemy is Lost!");
        }
    }

    //�e���G�ɓ�������������
    bool IsCollision(Vector2 bulletPosition, Vector2 enemyPosition)
    {
        Vector2 d = bulletPosition - enemyPosition;
        return Mathf.Abs(d.x) < 0.7 && Mathf.Abs(d.y) < 0.7;
    }

    //�������Ă������̌��ď���
    void ShootDownNormalEnemy(EnemyController enemy)
    {
        if (enemy != null)
        {
            enemies.Remove(enemy);
            var obj = Instantiate(circleEnemyDestroyParticle);
            obj.transform.position = enemy.gameObject.transform.position;
            Destroy(enemy.gameObject);
            OnPlayerBulletHit?.Invoke(bulletController); //�R�[���o�b�N
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Enemy is lost");
        }
    }

    void ShootDownCircleEnemy(CircleEnemyController enemy)
    {
        if (enemy != null)
        {
            gameManager.TakeDamageEnemy(1);
            OnPlayerBulletHit?.Invoke(bulletController);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Enemy is Lost");
        }
    }
}
