using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public GameObject bulletPrefab;

    private PlayerModel playerModel;
    private PlayerView playerView;
    private int moveSpeed = 1;

    public float getKeyIntervalTime, getFireKeyIntervalTime;
    float getKeyTime, getFireKeyTime;

    void Start()
    {
        playerModel = new PlayerModel();
        playerView = GetComponent<PlayerView>();
        getKeyTime = Time.time;
        getFireKeyTime = Time.time;
    }

    void Update()
    {
        HandleInput();
        UpdateView();
    }

    void HandleInput()
    {
        if (Time.time > (getKeyTime + getKeyIntervalTime)) //とりあえず上下左右移動を、同じ処理下で（分けない）
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveUp();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveDown();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveRight();
            }
            getKeyTime = Time.time;
        }

        if ((Input.GetKeyDown(KeyCode.Space)) && (Time.time > (getFireKeyTime + getFireKeyIntervalTime)))
        {
            FireBullet();
            getFireKeyTime = Time.time;
        }

     
    }

    void MoveLeft()
    {
        playerModel.Position += new Vector2Int(-moveSpeed, 0);
    }

    void MoveRight()
    {
        playerModel.Position += new Vector2Int(moveSpeed, 0);
    }

    void MoveUp()
    {
        playerModel.Position += new Vector2Int(0, moveSpeed);
    }

    void MoveDown()
    {
        playerModel.Position += new Vector2Int(0, -moveSpeed);
    }

    void UpdateView()
    {
        playerView.UpdatePosition(playerModel.Position);
    }

    void FireBullet()
    {
        // 弾を発射する位置と速度
        Vector2Int bulletPosition = playerModel.Position + new Vector2Int(0, 1); // プレイヤーの前方から発射
        Vector2Int bulletVelocity = new Vector2Int(0, 1);  // 弾の方向（上向き）

        // 弾のプレハブをインスタンス化し、コントローラーを初期化
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Initialize(bulletPosition, bulletVelocity);
    }
}
