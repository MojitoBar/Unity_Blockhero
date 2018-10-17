using UnityEngine;
using System.Collections;

public class CHero : MonoBehaviour
{
    public Animator anim;
    public float Hp = 100;
    public GameObject AttackTarget;
    public GameObject Hpbar;
    public GameObject WizardBall;
    public GameObject Arrow;

    public Sprite _SkillEnd;
    public Sprite LastSprite;

    GameObject Range;

    bool AttackCheck = false;
    bool SkillCheck = false;

    Vector3 HeroPos;
    Vector3 TargetPos;

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
        HeroPos = gameObject.transform.localPosition;
        TargetPos = new Vector3(HeroPos.x + 70.0f, 0, 1);
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
                PlayerMove();
                break;
            default:
                break;
        }
        gameObject.transform.localPosition = HeroPos;
        PlayerAttack();
        HpControl();
        SkillEnd();
        CheckTarget();
    }

    void PlayerMove()
    {
        HeroPos += new Vector3(10.0f * Time.deltaTime, 0, 0);
    }

    void PlayerAttack()
    {
        if (GetComponent<SpriteRenderer>().sprite == LastSprite)
        {
            if (!AttackCheck)
            {
                ClassAttack();
                anim.SetBool("Damage", true);
            }
            AttackCheck = true;
        }
        else
        {
            anim.SetBool("Damage", false);
            AttackCheck = false;
        }
    }

    void ClassAttack()
    {
        if (gameObject.name == "Warrior")
        {
            AttackTarget.GetComponent<Enemy>().Hp -= 10;
        }
        else if (gameObject.name == "Wizard")
        {
            CreateWizardBall();
        }
        else if (gameObject.name == "Archer")
        {
            CreateArrow();
        }
    }

    void CreateArrow()
    {
        GameObject obj = (GameObject)Instantiate(Arrow);
        obj.transform.parent = gameObject.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
    }

    void CreateWizardBall()
    {
        GameObject obj = (GameObject)Instantiate(WizardBall);
        obj.transform.parent = gameObject.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
    }

    void SkillEnd()
    {
        if (GetComponent<SpriteRenderer>().sprite == _SkillEnd)
        {
            if (!SkillCheck)
            {
                if (AttackTarget != null)
                {
                    AttackTarget.GetComponent<Enemy>().Hp -= 20;
                    anim.SetBool("Damage", true);
                }
            }
            SkillCheck = true;

            anim.SetBool("Skill", false);
        }
        else
        {
            SkillCheck = false;
        }
    }

    void CheckTarget()
    {
        if (gameObject.transform.FindChild("Range").GetComponent<HeroAttack>().obj == null)
        {
            return;
        }
        else
        {
            AttackTarget = gameObject.transform.FindChild("Range").GetComponent<HeroAttack>().obj;
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
