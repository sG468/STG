using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    public void UpdatePosition(Vector2Int position)
    {
        transform.position = new Vector3(position.x, position.y, 0);
    }

    // �G�ŗL�̕\�����W�b�N���ǉ��ł���
}
