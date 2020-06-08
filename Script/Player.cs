using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    //=====================================
    // 初期化
    //=====================================
    public float Speed;             // 移動速度
    public float JumpVector;        // ジャンプベクトル
    [HideInInspector]
    public bool bAnimJump;           // ジャンプできるか
    
    private Vector3 gScale;

    private Rigidbody2D Rigid;        // Rigidbody入れる
    private bool gMove;              // 移動している

    private Animator animator;                  // アニメーター
    private float IdleChange;
    private bool move = false, jump = false;    // 移動,ジャンプ,ライト操作
    private bool grounded = true;


    //=====================================
    // 初期化
    //=====================================
    void Start()
    {
        // 変数の初期化
        bAnimJump = false;
        grounded = true;

        Rigid = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

        gScale = transform.localScale;
    }

    //=====================================
    // 更新
    //=====================================
    void Update()
    {
        IdleChange += 0.01f;
        if (IdleChange >= 30 || move || jump)
            IdleChange = 0;

        animator.SetBool("Move", move);
        animator.SetBool("Jump", jump);
        animator.SetBool("Grounded", grounded);
        animator.SetFloat("IdleChange", IdleChange);

        //移動
        Move();

        //ジャンプ
        Jump();

        jump = bAnimJump;
        transform.localScale = gScale;
        move = gMove;

        if (!grounded)
            move = false;
    }

    //=====================================
    // 移動
    //=====================================
    private void Move()
    {
        if (MoveLeft() || MoveRight())
        {
            gMove = true;
        }
        else
            gMove = false;
    }
    //=====================================
    // 右に移動
    //=====================================
    private bool MoveRight()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Rigid.AddForce(new Vector2(Speed, 0.0f));

            gScale.x = 0.12f;


            return true;
        }
        return false;
    }

    //=====================================
    // 左に移動
    //=====================================
    private bool MoveLeft()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Rigid.AddForce(new Vector2(-Speed, 0.0f));

            gScale.x = -0.12f;

            return true;
        }
        return false;
    }

    //=====================================
    // ジャンプ
    //=====================================
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // ジャンプしてるよ
            bAnimJump = true;
            grounded = false;

            // ジャンプするときに重力(下ベクトル)をリセットする
            Rigid.velocity = new Vector2(Rigid.velocity.x, 0.0f);

            // ジャンプ！！
            Rigid.AddForce(new Vector2(0.0f, JumpVector), ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("floor"))
        {
            bAnimJump = false;
            grounded = true;
        }
    }
}
