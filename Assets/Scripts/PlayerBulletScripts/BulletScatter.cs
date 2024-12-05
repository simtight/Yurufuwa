using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BulletScatter : MonoBehaviour
{
    // 弾のスピードの調整
    [SerializeField] float BulletSpeed;
    // 消えるまでの時間（秒）
    [SerializeField] float DestroyTime;
    private void Start()
    {
        // Velocityにスピードを代入
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = gameObject.transform.rotation * Vector2.right * BulletSpeed;

        // 一定時間後に削除
        Destroy(this.gameObject, DestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 当たったオブジェクトがプレイヤーか弾の場合はここで中断
        if (collision.tag == "Player" || collision.tag == "Bullet") return;
        // 何かに当たったから弾を消す
        Destroy(this.gameObject);
    }
}