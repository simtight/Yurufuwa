using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShield : MonoBehaviour
{
    // �e�̃X�s�[�h�̒���
    [SerializeField] float BulletSpeed;
    // ������܂ł̎��ԁi�b�j
    [SerializeField] float DestroyTime;
    private void Start()
    {
        // Velocity�ɃX�s�[�h����
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right * BulletSpeed; // �����𒼐ړ����

        // ��莞�Ԍ�ɍ폜
        Destroy(this.gameObject,DestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���������I�u�W�F�N�g���v���C���[���e�̏ꍇ�͂����Œ��f
        if (collision.tag == "Player" || collision.tag == "Bullet") return;
        // �����ɓ�����������e������
        Destroy(this.gameObject);
    }
}
