using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    public void UpdatePosition(Vector2Int position)
    {
        // モデルの位置に基づいて弾の表示を更新
        transform.position = new Vector3(position.x, position.y, 0);
    }
}
