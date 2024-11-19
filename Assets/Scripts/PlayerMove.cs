//+-------------------------------
//|�X�N���v�g��
//|PlayerMoveController.cs
//|
//+-------------------------------
//|�X�N���v�g����
//|Player�̑�����������܂�
//|
//+-------------------------------
//|�쐬��
//|�L��
//|
//+-------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //�R���|�[�l���g---------------------------------------
    //�ڒn����̃R���C�_�[
    private BoxCollider2D isGroundBoxCollider2D;
    //�v���C���[��Rigidbody
    private Rigidbody2D rb;

    //�ݒu����̃Q�[���I�u�W�F�N�g
    [SerializeField]
    private GameObject isGroundObject;

    //�v���C���[��mass
    private float mass;
    //-----------------------------------------------------


    //�J�E���^--------------------------------------------
    //FixedUpdate�̉��J�E���g
    private int fixedFrameCount = 0;
    //----------------------------------------------------


    //�������x�̒萔--------------------------------------
    //�n�㎞�̐������x
    private const float groundSpeedLimit = 5.0f;
    //�󒆎��̐������x
    private const float aerialSpeedLimit = 7.0f;
    //----------------------------------------------------


    //�ړ��ƃW�����v�̗͂̑傫��--------------------------
    //����
    private const float speed = 30.0f;
    //�W�����v��
    private const float jumpVelocity = 3.0f;
    //�W�����v�̃t���[��
    private const float jumpFrameLimit = 20.0f;
    //----------------------------------------------------



    //�ڒn����--------------------------------------------
    //�ڒn���t���O
    private bool isGround = false;
    //�W�����v���t���O
    private bool isJump = false;
    //----------------------------------------------------


    //�v���C���[����--------------------------------------
    //���ړ��t���O
    private bool isLeft = false;
    //�E�ړ��t���O
    private bool isRight = false;
    //�W�����v�L�[�����������̃t���O
    //private bool isUp = false;
    //�W�����v�L�[�����������Ă�t���O
    private bool upNow = false;
    //�ǃW�����v�̃t���O
    //private bool wallJamp = false;
    //�L�[�������ĂȂ��t���O
    private bool notKey = false;
    //�v���[���[�ړ��p�̃x�N�g��
    private Vector2 vector2 = Vector2.zero;
    //----------------------------------------------------



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
        //�d�͂̊֌W��y���͂��̂܂�
        vector2.y = rb.velocity.y;

        //�ړ��x�N�g���̏�����
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
        //�W�����v
        //
        if (fixedFrameCount<100)
        {
            fixedFrameCount++;
        }
        if (isGround && fixedFrameCount > 0 && fixedFrameCount < 4)
        {
            isJump = true;
        }
        if (!upNow)
        {
            isJump = false;
        }
        if (isJump&&fixedFrameCount<jumpFrameLimit)
        {
            rb.velocity = new Vector2(rb.velocity.x,jumpVelocity);
        }
    }

    //+---------------------------------method
    //|�֐�����
    //|�ړ��̊֐��ł�
    //|
    //+-------------------------------
    private void Walk()
    {

        //
        //�ڒn���Ƌ󒆂̑���
        //
        //�ڒn���̑���
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
    //|�֐�����
    //|�L�[������܂Ƃ߂Ă܂�
    //|
    //+-------------------------------
    private void ProcessInput()
    {
        //���E�ړ�
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
        //�W�����v
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGround)
            {
                isJump = true;
                fixedFrameCount = 0;

            }
            if (!isGround)
            {
                //isUp = true;
                fixedFrameCount = 0;
            }
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if(fixedFrameCount < 4)
            {
                //isUp = false;
            }
            upNow = true;
        }
        else
        {
            //isUp = false;
            if (isJump)
            {
                upNow = false;
            }

        }
    }

    //+---------------------------------method
    //|�֐�����
    //|FixedFrame�P�ʂ�Force��Ԃ��܂�
    //|
    //+-------------------------------
    private float ForcePerFixedFrame(float force)
    {
        return force * Time.fixedDeltaTime / mass;
    }

    //+---------------------------------method
    //|�֐�����
    //|�������Ɋ�l�A�������Ɋ�l����͈̔́A��O�����Ɍv�肽���l�A��l�����ɋ������덷
    //|
    //+-------------------------------
    private bool BetweenValue(float value, float betValue, float measureValue, float percent)
    {
        float perValue = betValue * (1.0f + percent);
        if (value - betValue - perValue < measureValue && value + betValue + perValue > measureValue) return true;

        return false;
    }

    //+---------------------------------method
    //|�֐�����
    //|�q�I�u�W�F�N�g��isTriggerEnter�̌��m��isGround�ɓ���܂�
    //|
    //+-------------------------------
    private void HandleIsGroundTriggerTrue(Collider2D collider)
    {
        isGround = true;
    }

    //+---------------------------------method
    //|�֐�����
    //|�q�I�u�W�F�N�g��isTriggerExit�̌��m��isGround�ɓ���܂�
    //|
    //+-------------------------------
    private void HandleIsGroundTriggerFalse(Collider2D collider)
    {
        isGround = false;
    }

}
