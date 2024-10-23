using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public System.Action<EnemyBulletController> OnEnemyBulletHit;
    public EnemyBulletController bulletController;
    public PlayerController player;

    private Vector2 velocity;
    private float speed = 10.0f;

    GameManager gameManager;
     

    // Start is called before the first frame update
    void Start()
    {
        
    }

    //�v���C���[�̕����ɕ������e�̐���
    public void Initialize(Vector2 initialPosition, PlayerController player_, GameManager gameManager_)
    {
        transform.position = initialPosition;
        this.velocity = ((Vector2)player_.transform.position - initialPosition).normalized;
        bulletController = GetComponent<EnemyBulletController>();
        player = player_;
        gameManager = gameManager_;
    }

    //�~�`�ɕ������e�̈�̐���
    public void InitializeCircleBullet(Vector2 initialPosition, Vector2 initialVelocity, PlayerController player_, GameManager gameManager_)
    {
        transform.position = initialPosition;
        velocity = initialVelocity;
        bulletController = GetComponent<EnemyBulletController>();
        player = player_;
        gameManager = gameManager_;
    }

    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * speed * Time.deltaTime);
        CheckCollisionPlayer();
    }

    //�G�����˂����e�ƃv���C���[�̓����蔻��
    void CheckCollisionPlayer()
    {
        if (player != null)
        {
            if (IsCollision(transform.position, player.transform.position))
            {
                HandleCollision(player);
            }
        }
        else
        {
            //Debug.Log("Player is lost");
        }
    }

    //�e���G�ɓ�������������
    bool IsCollision(Vector2 bulletPosition, Vector2 playerPosition)
    {
        Vector2 d = bulletPosition - playerPosition;
        return Mathf.Abs(d.x) < 0.5 && Mathf.Abs(d.y) < 0.5;
    }

    //�����������̏���
    void HandleCollision(PlayerController player)
    {
        if (player != null)
        {
            gameManager.TakeDamage(1);
            OnEnemyBulletHit?.Invoke(bulletController); //�R�[���o�b�N
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Enemy is lost");
        }
    }
}
