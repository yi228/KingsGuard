using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using Random = UnityEngine.Random;

public class Monster : Creature
{
    SpriteRenderer sprite;
    Rigidbody2D rigid;
    BoxCollider2D boxCollider;
    Animator anim;

    public Transform pos;
    public Vector2 boxSize;
    public GameObject hitBox;

    Vector3 dirVec;
    Collider2D[] atkUnits;
    bool detectTarget = false;
    float distance;
    bool canMove = true;
    bool isAttack = false;
    float atkTimer = 0f;
    bool dead = false;
    bool drop = false;
    bool atkDelayEnd = false;
    float hitTimer = 0f;
    bool getHit = false;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        dirVec = new Vector3(-1, 0, 0);
    }

    void Update()
    {
        if(!detectTarget) //방향 전환
            sprite.flipX = dirVec.x ==1 ? false:true;
        else
            sprite.flipX = GameManager.instance.player.GetComponent<Rigidbody2D>().position.x < rigid.position.x;

        if (sprite.flipX)
            hitBox.transform.localPosition = new Vector2(-1, 0);
        else
            hitBox.transform.localPosition = new Vector2(1, 0);

        Detect();
        Attack_Delay();


        if (isAttack) //공격 타이머
        {
            atkTimer += Time.deltaTime;
            if (atkTimer > 1)
            {
                isAttack = false;
                atkTimer = 0;
            }
        }

        if (getHit) //피격 시 효과
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

        if (hp <= 0) //죽음
            Death();
    }

    void FixedUpdate()
    {
        if(!dead&&canMove&&!GameManager.instance.talkOn&& !GameManager.instance.helpOn)
            Move();
    }

    public override void Attack() //공격
    {
        if (!isAttack && !GameManager.instance.player.dead)
        {
            anim.SetBool("Run", false);
            isAttack =true;

            atkUnits = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);

            anim.SetTrigger("Attack");
            canMove = false;

            if (atkDelayEnd&&!dead)
            {
                atkDelayEnd = false;
                foreach (Collider2D atkUnit in atkUnits)
                {
                    if (atkUnit.tag == "Player")
                    {
                        if (!atkUnit.GetComponent<Player>().isBlock)
                        {
                            atkUnit.GetComponent<Player>().hp -= attack;
                            atkUnit.GetComponent<Player>().GetHit();
                        }
                        else
                            atkUnit.GetComponent<Animator>().SetTrigger("Block");
                    }
                }
            }
        }
    }

    void Attack_Delay() //애니메이션 일정 비율 재생돼야 공격 효과
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Goblin_Attack1") &&
               anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.6f)
        {
            atkDelayEnd=true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    public override void Death() //죽음
    {
        dead = true;
        anim.SetBool("Death", true);
        if (!drop)
        {
            drop = true;
            ItemDrop();
        }
        Destroy(this.gameObject, 1f);
    }
        
    private void ItemDrop() //아이템 드랍
    {
        Vector2 pos = new Vector2(rigid.transform.position.x, (float)Math.Truncate(rigid.transform.position.y) + 0.17f);
        GameObject dropItem = Instantiate(ItemDatabase.Instance.itemPrefab, pos, Quaternion.identity);
        dropItem.GetComponent<FieldItems>().SetItem(ItemDatabase.Instance.itemDB[Random.Range(0, 6)]);
    }

    public override void GetHit() //피격
    {
        getHit = true;
    }

    public override void Move() //이동
    {
        anim.SetBool("Run", true);

        if (!detectTarget) //공격 대상 없을 때
        {
            Vector3 frontVec = new Vector3(this.GetComponent<Transform>().position.x + dirVec.x, this.GetComponent<Transform>().position.y, 0);
            Debug.DrawRay(frontVec, Vector3.down * 1f, new Color(1, 1, 0));
            RaycastHit2D groundEnd = Physics2D.Raycast(frontVec, Vector3.down, 1f, LayerMask.GetMask("Ground"));
            Debug.DrawRay(frontVec, Vector3.up * 1f, new Color(1, 1, 0));
            RaycastHit2D upWall = Physics2D.Raycast(frontVec, Vector3.up, 1f, LayerMask.GetMask("Ground"));
            Debug.DrawRay(this.GetComponent<Transform>().position, dirVec * 1f, new Color(1, 1, 0));
            RaycastHit2D frontWall = Physics2D.Raycast(this.GetComponent<Transform>().position, dirVec, 1f, LayerMask.GetMask("Ground"));

            this.GetComponent<Transform>().Translate(new Vector3(dirVec.x * speed * Time.fixedDeltaTime, 0, 0));

            if (groundEnd.collider == null || frontWall.collider != null ||upWall.collider!=null)
                dirVec.x = -dirVec.x;
        }
        else
        {
            Vector2 targetVec = GameManager.instance.player.GetComponent<Rigidbody2D>().position - rigid.position;
            Vector2 traceVec = targetVec.normalized * speed * Time.fixedDeltaTime;
            traceVec = new Vector2(traceVec.x, 0);
            rigid.MovePosition(rigid.position + traceVec);
            //rigid.velocity = Vector2.zero;
        }
    }

    void Detect() //공격 대상 탐색, 발견
    {
        distance = Vector2.Distance(GameManager.instance.player.GetComponent<Rigidbody2D>().position, rigid.position);

        if(distance <= 4f)
        {
            detectTarget = true;
            Debug.Log("target detect");
            if (distance <= 1.5f)
            {
                Debug.Log("attack start");
                Attack();
            }
            else
                canMove = true;
        }
        else
        {
            detectTarget = false;
            canMove = true;
        }
    }
}
