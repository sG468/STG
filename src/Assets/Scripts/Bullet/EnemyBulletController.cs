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

    //プレイヤーの方向に放たれる弾の生成
    public void Initialize(Vector2 initialPosition, PlayerController player_, GameManager gameManager_)
    {
        transform.position = initialPosition;
        this.velocity = ((Vector2)player_.transform.position - initialPosition).normalized;
        bulletController = GetComponent<EnemyBulletController>();
        player = player_;
        gameManager = gameManager_;
    }

    //円形に放たれる弾の一つの生成
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

    //敵が発射した弾とプレイヤーの当たり判定
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

    //弾が敵に当たったか判定
    bool IsCollision(Vector2 bulletPosition, Vector2 playerPosition)
    {
        Vector2 d = bulletPosition - playerPosition;
        return Mathf.Abs(d.x) < 0.5 && Mathf.Abs(d.y) < 0.5;
    }

    //当たった時の処理
    void HandleCollision(PlayerController player)
    {
        if (player != null)
        {
            gameManager.TakeDamage(1);
            OnEnemyBulletHit?.Invoke(bulletController); //コールバック
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Enemy is lost");
        }
    }
}
