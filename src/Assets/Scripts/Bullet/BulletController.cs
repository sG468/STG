using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private BulletModel bulletModel;
    private BulletView bulletView;

    public float fireIntervalTime;
    float fireTime;

    public void Initialize(Vector2Int initialPosition, Vector2Int initialVelocity)
    {
        // 弾のモデルとビューを初期化
        bulletModel = new BulletModel(initialPosition, initialVelocity);
        bulletView = GetComponent<BulletView>();
        fireTime = Time.time;
    }

    void Update()
    {
        if (Time.time > fireTime + fireIntervalTime)
        {
            // 弾の位置を更新
            bulletModel.UpdatePosition();
            fireTime = Time.time;
        }
      
        // ビューの位置も更新
        bulletView.UpdatePosition(bulletModel.Position);

        // 画面外に出た場合に弾を削除（例として画面外を条件に）
        if (IsOutOfScreen(bulletModel.Position))
        {
            Destroy(gameObject);
        }
    }

    // 画面外に出たかどうかを判定
    bool IsOutOfScreen(Vector2Int position)
    {
        // 仮に画面の範囲をX:-10~10,Y:-10~10とした場合
        return position.x < -10 || position.x > 10 || position.y < -10 || position.y > 10;
    }
}
