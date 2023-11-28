using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class King : Creature
{
    SpriteRenderer sprite;
    Rigidbody2D rigid;
    Animator anim;
    BoxCollider2D boxCollidier;

    public GameObject talkTrigger;
    public Transform pos;
    public Vector2 boxSize;
    public GameObject hitBox;
    public float jumpPower;

    bool isAttack = false;
    bool dead = false;
    bool firstAtk = false;
    bool secondAtk = false;
    bool getHit = false;
    bool isJump = false;
    float atkTimer = 0f;
    float hitTimer = 0f;
    float dist;
    Collider2D[] atkUnits;
    Vector2 dirVec;
    Vector2 frontVec;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollidier = GetComponent<BoxCollider2D>();

        GameManager.instance.meetKing = false;
    }

    private void Start()
    {
        dirVec = new Vector2(1, 0);
    }

    private void Update()
    {
        dist = Mathf.Abs(GameManager.instance.player.GetComponent<Transform>().position.x - transform.position.x);
        if (dist <= 2f)
        {
            GameManager.instance.helpText.text = "Press F to talk.";
            GameManager.instance.helpText.gameObject.SetActive(true);
        }
        else
        {
            GameManager.instance.helpText.gameObject.SetActive(false);
        }

        if (!GameManager.instance.talkEnd[2]&&Input.GetKey(KeyCode.F) && dist <= 2f)
        {
            GameManager.instance.talkOn = true;
        }

        if (GameManager.instance.talkEnd[2] && !dead)
        {
            if(GameManager.instance.player.GetComponent<Transform>().position.x - transform.position.x > 1)
            {
                dirVec.x = 1;
                sprite.flipX = false;
                hitBox.transform.localPosition = new Vector2(1, 0);
            }
            else
            {
                dirVec.x = -1;
                sprite.flipX = true;
                hitBox.transform.localPosition = new Vector2(-1, 0);
            }

            if (dist <= 3f)
            {
                Attack();
                if (isAttack)
                {
                    atkTimer += Time.deltaTime;
                    if (atkTimer > 1f)
                    {
                        isAttack = false;
                        atkTimer = 0;
                    }
                }
            }

            if (getHit)
            {
                hitTimer += Time.deltaTime;
                sprite.color = new Color32(255, 110, 110, 255);
                if (hitTimer > 0.5f)
                {
                    sprite.color = new Color32(255, 255, 255, 255);
                    getHit = false;
                    hitTimer = 0;
                }
            }

            if(hp<= 0)
            {
                Death();
            }
        }

        if (dead && !GameManager.instance.talkEnd[3])
        {
            GameManager.instance.talkOn = true;
        }
    }

    private void FixedUpdate()
    {
        frontVec = new Vector2(rigid.transform.position.x + dirVec.x, rigid.transform.position.y);
        RaycastHit2D frontDown = Physics2D.Raycast(frontVec, Vector2.down, 2f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(frontVec, Vector2.down * 2f, new Color(1, 1, 0));
        RaycastHit2D frontWall = Physics2D.Raycast(rigid.transform.position, frontVec, 1.2f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(rigid.transform.position, dirVec * 1.2f, new Color(1, 1, 1));

        if (frontDown.collider == null || frontWall.collider != null)
        {
            if (frontDown.collider == null)
                Debug.Log("front");
            else
                Debug.Log("wall");
            Jump();
        }

        if (GameManager.instance.talkEnd[2] && !dead && dist>3f)
        {
            Move();

        }
    }

    public override void Attack()
    {
        if (!isAttack)
        {
            anim.SetBool("Move", false);
            isAttack = true; 

            atkUnits = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

            foreach (Collider2D atkUnit in atkUnits)
            {
                if (atkUnit.tag == "Player")
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
            else if (firstAtk && !secondAtk)
            {
                anim.SetTrigger("Attack2");
                secondAtk = true;
            }
            else if (firstAtk && secondAtk)
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    public override void Move()
    {
        anim.SetBool("Move", true);
        transform.Translate(new Vector2(dirVec.x * speed * Time.fixedDeltaTime, 0));
    }

    public override void GetHit()
    {
        getHit = true;
    }

    public override void Death()
    {
        dead = true;
        GameManager.instance.kingDead = true;
        anim.SetBool("Death", true);
        anim.SetTrigger("DeathAnim");
    }

    void Jump()
    {
        Debug.Log("Jump");
        if (!isJump)
        {
            isJump = true;
            rigid.AddForce(new Vector2(3, jumpPower), ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isJump = false;
        }
    }
}
