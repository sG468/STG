using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView : MonoBehaviour
{
    public void UpdatePosition(Vector2Int position)
    {
        // ���f���̈ʒu�Ɋ�Â��Ēe�̕\�����X�V
        transform.position = new Vector3(position.x, position.y, 0);
    }
}
