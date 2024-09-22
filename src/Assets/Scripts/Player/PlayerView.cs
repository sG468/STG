using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public void UpdatePosition(Vector2Int position)
    {
        transform.position = new Vector3(position.x, position.y, 0);
    }

    public void UpdateHealthBar(int health)
    {
        // ヘルスバーの表示を更新
    }
}
