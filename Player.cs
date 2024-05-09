using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;

public class Player : Creature
{
    SpriteRenderer sprite;
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    Animator anim;

    public Slider hpBar;
    public float jumpPower;
    public float runSpeed;
    public Transform pos;
    public Vector2 boxSize;
    public GameObject hitBox;
    public bool isBlock = false;
    public bool dead = false;
    public bool isTalk = false;
    public float baseSpeed;
    public bool atkPotion = false;
    public bool armorOn = false;
    public bool helmetOn = false;

    bool isJump = false;
    bool isAttack = false;
    bool firstAtk = false;
    bool secondAtk = false;
    bool getHit = false;
    float atkTimer = 0f;
    float jumpTimer = 0f;
    float atkPotionTimer = 0f;
    float atkPotionPoint = 2;
    float hitTimer = 0f;
    Collider2D[] atkUnits;


    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        baseSpeed = speed;
    }

    void Update()
    {
        if (hp <= 0&&!dead) //죽음 처리
        {
            dead = true;
            Death();
        }

        if (hp > maxhp) //체력 상한
        {
            hp = maxhp;
        }

        hpBar.value = hp / maxhp; //체력바

        if (!dead)
        {
            if (sprite.flipX) //좌우 반전
                hitBox.transform.localPosition = new Vector2(-1, 1);
            else
                hitBox.transform.localPosition = new Vector2(1, 1);

            if (Input.GetKeyDown(KeyCode.D)) //D 누르면 공격
            {
                Attack();
            }
            if (isAttack) //공격 타이머
            {
                atkTimer += Time.deltaTime;
                if (atkTimer > 0.3)
                {
                    isAttack = false;
                    atkTimer = 0;
                }
            }

            if (Input.GetKey(KeyCode.S)) //S 누르면 방어
            {
                Block();
            }
            else
            {
                isBlock = false;
                anim.SetBool("IdleBlock", false);
            }

            if (getHit) //피격 시 효과
            {
                hitTimer += Time.deltaTime;
                
                if (hitTimer > 0.5f)
                {
                    sprite.color = new Color32(255, 255, 255, 255);
                    getHit = false;
                    hitTimer = 0;
                }
            }
        }

        if (atkPotion) //공격력증가포션 효과 타이머
        {
            atkPotionTimer += Time.deltaTime;
            if(atkPotionTimer > 10)
            {
                atkPotion = false;
                GameManager.instance.atkPotionOn.color = new Color32(255, 255, 255, 0);
                attack -= atkPotionPoint;
                atkPotionTimer = 0;
            }
        }

        if (!isJump) //점프 초기화 안될 때 타이머로 초기화
        {
            jumpTimer = 0f;
        }
        else 
        {
            jumpTimer += Time.deltaTime;
            if(jumpTimer > 1f)
            {
                jumpTimer = 0f;
                if (isJump)
                {
                    isJump = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!dead && !isAttack && !isBlock && !isTalk )
        {
            Move();
            Run();
        }
    }

    public override void Move() //이동
    {
        anim.SetBool("Move", Input.GetKey(KeyCode.LeftArrow)|| Input.GetKey(KeyCode.RightArrow));
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            sprite.flipX = true;
            transform.Translate(new Vector3(-speed * Time.fixedDeltaTime, 0, 0));
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            sprite.flipX = false;
            transform.Translate(new Vector3(speed * Time.fixedDeltaTime, 0, 0));
        }

        if (Input.GetKey(KeyCode.UpArrow)) //점프
        {
            if (!isJump)
            {
                isJump = true;
                rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
                anim.SetTrigger("Jump");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJump = false;
        }

        if (collision.collider.CompareTag("Fall"))
        {
            if (hp > 1)
                hp = 1;
            else
                hp = 0;
            rigid.velocity = new Vector2(0, 0);
            transform.position = GameManager.instance.spawnPos[GameManager.instance.sceneIndex - 2];
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Talk")
        {
            if(SceneManager.GetActiveScene().name == "Scene 5")
            {
                GameManager.instance.meetKing = true;
            }
        }
    }

    void Run() //빠른 이동(스페이스바)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            speed = runSpeed;
            anim.SetBool("RunFast", true);
        }
        else
        {
            speed = baseSpeed;
            anim.SetBool("RunFast", false);
        }
    }

    public override void Attack() //공격 함수
    {
        if (!isAttack)
        {
            isAttack = true;

            atkUnits = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

            foreach (Collider2D atkUnit in atkUnits)
            {
                if(atkUnit.tag == "Enemy")
                {
                    atkUnit.GetComponent<Creature>().hp -= attack;
                    atkUnit.GetComponent<Creature>().GetHit();
                }
            }

            if (!firstAtk && !secondAtk)
            {
                anim.SetTrigger("Attack1");
                firstAtk = true;
            }
            else if(firstAtk && !secondAtk)
            {
                anim.SetTrigger("Attack2");
                secondAtk = true;
            }
            else if(firstAtk && secondAtk)
            {
                anim.SetTrigger("Attack3");
                atkTimer += Time.deltaTime;
                firstAtk = false;
                secondAtk = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    void Block() //방어
    {
        anim.SetBool("IdleBlock", true);
        isBlock = true;
    }

    public override void Death() //죽음
    {
        anim.SetBool("Death", true);
        anim.SetTrigger("DeathAnim");
    }


    public override void GetHit() //피격
    {
        if (armorOn)
        {
            hp = hp + 1f;
        }
        if (helmetOn)
        {
            hp = hp + 0.5f;
        }
        Debug.Log("player get hit");
        sprite.color = new Color32(255, 110, 110, 255);
        getHit = true;
    }
}
