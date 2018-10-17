using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Animator anim;
    public float Hp = 100;
    public GameObject Hpbar;

    public Sprite LastSprite = null;

    GameObject Range;

    Vector3 EnemyPos;
    Vector3 TargetPos;

    public GameObject AttackTarget;

    bool check = false;

    public enum State
    {
        Move,
        Attack
    }

    public State state = State.Move;

    void Start()
    {
        Hpbar = gameObject.transform.FindChild("hpbar").transform.FindChild("Ancher").gameObject;
        Range = gameObject.transform.GetChild(0).gameObject;
        EnemyPos = gameObject.transform.localPosition;
        TargetPos = new Vector3(EnemyPos.x + 70.0f, 0, 1);
    }

    void Update()
    {

        if (AttackTarget != null)
        {
            if (!AttackTarget.active)
            {
                state = State.Move;
            }
        }

        switch (state)
        {
            case State.Attack:
                anim.SetBool("Condition", false);
                break;
            case State.Move:
                anim.SetBool("Condition", true);
                EnemyMove();
                break;
            default:
                break;
        }
        gameObject.transform.localPosition = EnemyPos;
        CheckTarget();
        EnemyAttack();
        HpControl();
    }

    void CheckTarget()
    {
        if (gameObject.transform.FindChild("Range").GetComponent<EnemyAttack>().obj == null)
        {
            return;
        }
        else
        {
            AttackTarget = gameObject.transform.FindChild("Range").GetComponent<EnemyAttack>().obj;
        }
    }

    void EnemyMove()
    {
        EnemyPos -= new Vector3(10.0f * Time.deltaTime, 0, 0);
    }

    void EnemyAttack()
    {
        if (GetComponent<SpriteRenderer>().sprite == LastSprite)
        {
            if (!check)
            {
                AttackTarget.GetComponent<CHero>().Hp -= 6;
                anim.SetBool("Damage", true);
            }
            check = true;
        }
        else
        {
            anim.SetBool("Damage", false);
            check = false;
        }
    }

    void HpControl()
    {
        Hpbar.transform.localScale = new Vector3(1 - (Hp / 100), 1, 1);
        if (Hpbar.transform.localScale.x >= 1.0f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
