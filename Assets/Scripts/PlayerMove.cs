//+-------------------------------
//|スクリプト名
//|PlayerMoveController.cs
//|
//+-------------------------------
//|スクリプト説明
//|Playerの操作を実装します
//|
//+-------------------------------
//|作成者
//|鵜飼
//|
//+-------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //コンポーネント---------------------------------------
    //接地判定のコライダー
    private BoxCollider2D isGroundBoxCollider2D;
    //プレイヤーのRigidbody
    private Rigidbody2D rb;
    //設置判定のゲームオブジェクト
    [SerializeField]
    private GameObject isGroundObject;
    //プレイヤーのmass
    private float mass;
    //-----------------------------------------------------


    //カウンタ--------------------------------------------
    //FixedUpdateの回るカウント
    private int fixedFrameCount = 0;
    //----------------------------------------------------


    //制限速度の定数--------------------------------------
    //地上時の制限速度
    private const float groundSpeedLimit = 5.0f;
    //空中時の制限速度
    private const float aerialSpeedLimit = 7.0f;
    //----------------------------------------------------


    //移動とジャンプの力の大きさ--------------------------
    //速さ
    private const float speed = 30.0f;
    //ジャンプ力
    private const float jumpForce = 250.0f;
    //----------------------------------------------------



    //接地判定--------------------------------------------
    //接地中フラグ
    private bool isGround = false;
    //----------------------------------------------------


    //プレイヤー操作--------------------------------------
    //左移動フラグ
    private bool isLeft = false;
    //右移動フラグ
    private bool isRight = false;
    //ジャンプキーを押した時のフラグ
    private bool canJump = false;
    //キーを押してないフラグ
    private bool notKey = false;
    //プレーヤー移動用のベクトル
    private Vector2 vector2 = Vector2.zero;
    //----------------------------------------------------

    //弾--------------------------------------------------
       //弾発射
    private bool isBullet = false;
    //
    private int kindsBullet = 0;
    //----------------------------------------------------


    //
    private enum Bullet
    {
        shield,
        spread,
        penetrate
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mass = rb.mass;

        isGroundBoxCollider2D = isGroundObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        //重力の関係でy軸はそのまま
        vector2.y = rb.velocity.y;

        //移動ベクトルの初期化
        vector2 = Vector2.zero;
        if (notKey)
        {
            rb.velocity = new Vector2(0.0f,vector2.y);
        }
        if (isLeft) vector2.x -= 1.0f;
        if (isRight) vector2.x += 1.0f;
        vector2.x *= speed;
        Walk();
        if(!(isLeft^isRight)) rb.velocity = new Vector2(0f,rb.velocity.y);

        //
        //ジャンプ
        //
        if (canJump && isGround)
        {
            canJump = false;
            rb.AddForce(new Vector2(0.0f, jumpForce));
        }
        ///
        ///
        ///
        if(isBullet)
        {
            isBullet = false;
            switch(kindsBullet)
            {
                //
                case 0:
                    Debug.Log("Bullet.shield");
                    break;
                //
                case 1:
                    Debug.Log("Bullet.spread");
                    break;
                //
                case 2:
                    Debug.Log("Bullet.penetrate");
                    break;
            }
        }
    }

    //+---------------------------------method
    //|関数説明
    //|移動の関数です
    //|
    //+-------------------------------
    private void Walk()
    {

        //
        //接地中と空中の走り
        //
        //接地中の走り
        if (isGround && vector2.x != 0)
        {
            if (rb.velocity.x < groundSpeedLimit)
            {
                if (isRight)
                {

                    if (BetweenValue(groundSpeedLimit, ForcePerFixedFrame(vector2.x), rb.velocity.x, 0.01f))
                    {
                        rb.velocity = new Vector2(groundSpeedLimit, rb.velocity.y);
                    }
                    else
                    {
                        rb.AddForce(new Vector2(vector2.x, 0.0f));
                    }
                }
            }
            if (rb.velocity.x > -groundSpeedLimit)
            {
                if (isLeft)
                {
                    if (BetweenValue(-groundSpeedLimit, -(ForcePerFixedFrame(vector2.x)), rb.velocity.x, 0.01f))
                    {
                        rb.velocity = new Vector2(-groundSpeedLimit, rb.velocity.y);
                    }
                    else
                    {
                        rb.AddForce(new Vector2(vector2.x, 0.0f));
                    }
                }
            }

        }
        else
        {
            if (vector2.x != 0)
            {
                if (rb.velocity.x < aerialSpeedLimit)
                {
                    if (isRight)
                    {
                        if (BetweenValue(aerialSpeedLimit, ForcePerFixedFrame(vector2.x), rb.velocity.x, 0.01f))
                        {
                            rb.velocity = new Vector2(aerialSpeedLimit, rb.velocity.y);
                        }
                        else
                        {
                            rb.AddForce(new Vector2(vector2.x, 0.0f));
                        }
                    }
                }
                if (rb.velocity.x > -aerialSpeedLimit)
                {
                    if (isLeft)
                    {
                        if (BetweenValue(-aerialSpeedLimit, -(ForcePerFixedFrame(vector2.x)), rb.velocity.x, 0.01f))
                        {
                            rb.velocity = new Vector2(-aerialSpeedLimit, rb.velocity.y);
                        }
                        else
                        {
                            rb.AddForce(new Vector2(vector2.x, 0.0f));
                        }
                    }
                }
            }
        }
    }



    //+---------------------------------method
    //|関数説明
    //|キー操作をまとめてます
    //|
    //+-------------------------------
    private void ProcessInput()
    {
        int maxBullet = Enum.GetValues(typeof(Bullet)).Length - 1;
        //左右移動
        isLeft = false;
        isRight = false;
        if (Input.GetKey(KeyCode.A))
        {
            isLeft = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            isRight = true;
        }
        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canJump = true;
        }
        //弾の種類の遷移
        if (Input.GetKeyDown(KeyCode.I))
        {
            if(kindsBullet <= 0)
            {
                kindsBullet = maxBullet;
            }
            else
            {
                kindsBullet--;
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (kindsBullet >= maxBullet)
            {
                kindsBullet = 0;
            }
            else
            {
                kindsBullet++;
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            isBullet = true;
        }
    }

    //+---------------------------------method
    //|関数説明
    //|FixedFrame単位のForceを返します
    //|
    //+-------------------------------
    private float ForcePerFixedFrame(float force)
    {
        return force * Time.fixedDeltaTime / mass;
    }

    //+---------------------------------method
    //|関数説明
    //|第一引数に基準値、第二引数に基準値からの範囲、第三引数に計りたい値、第四引数に許される誤差
    //|
    //+-------------------------------
    private bool BetweenValue(float value, float betValue, float measureValue, float percent)
    {
        float perValue = betValue * (1.0f + percent);
        if (value - betValue - perValue < measureValue && value + betValue + perValue > measureValue) return true;

        return false;
    }

    //+---------------------------------method
    //|関数説明
    //|子オブジェクトのisTriggerEnterの検知をisGroundに入れます
    //|
    //+-------------------------------
    private void HandleIsGroundTriggerTrue(Collider2D collider)
    {
        isGround = true;
    }

    //+---------------------------------method
    //|関数説明
    //|子オブジェクトのisTriggerExitの検知をisGroundに入れます
    //|
    //+-------------------------------
    private void HandleIsGroundTriggerFalse(Collider2D collider)
    {
        isGround = false;
    }

}
