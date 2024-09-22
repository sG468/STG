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
        // �e�̃��f���ƃr���[��������
        bulletModel = new BulletModel(initialPosition, initialVelocity);
        bulletView = GetComponent<BulletView>();
        fireTime = Time.time;
    }

    void Update()
    {
        if (Time.time > fireTime + fireIntervalTime)
        {
            // �e�̈ʒu���X�V
            bulletModel.UpdatePosition();
            fireTime = Time.time;
        }
      
        // �r���[�̈ʒu���X�V
        bulletView.UpdatePosition(bulletModel.Position);

        // ��ʊO�ɏo���ꍇ�ɒe���폜�i��Ƃ��ĉ�ʊO�������Ɂj
        if (IsOutOfScreen(bulletModel.Position))
        {
            Destroy(gameObject);
        }
    }

    // ��ʊO�ɏo�����ǂ����𔻒�
    bool IsOutOfScreen(Vector2Int position)
    {
        // ���ɉ�ʂ͈̔͂�X:-10~10,Y:-10~10�Ƃ����ꍇ
        return position.x < -10 || position.x > 10 || position.y < -10 || position.y > 10;
    }
}
