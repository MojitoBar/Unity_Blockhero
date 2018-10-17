using UnityEngine;
using System.Collections;

public class HeroAttack : MonoBehaviour
{
    public GameObject obj;
    GameObject Hero;

    void Start()
    {
        Hero = gameObject.transform.parent.gameObject;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag.Equals("Enemy"))
        {
            obj = coll.gameObject;
            Hero.GetComponent<CHero>().state = CHero.State.Attack;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.Equals("Enemy"))
        {
            Hero.GetComponent<CHero>().state = CHero.State.Move;
        }
    }
}
