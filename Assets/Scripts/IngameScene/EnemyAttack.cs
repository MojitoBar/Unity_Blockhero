using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public GameObject obj;
    GameObject _Enemy;

    void Start()
    {
        _Enemy = gameObject.transform.parent.gameObject;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag.Equals("Hero"))
        {
            obj = coll.gameObject;
            _Enemy.GetComponent<Enemy>().state = Enemy.State.Attack;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag.Equals("Hero"))
        {
            _Enemy.GetComponent<Enemy>().state = Enemy.State.Move;
        }
    }
}
