using UnityEngine;
using System.Collections;

public class BallMng : MonoBehaviour
{
    public float speed;
    
    void Update()
    {
        gameObject.transform.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);

        if (gameObject.transform.localPosition.x >= 10.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag.Equals("Enemy"))
        {
            coll.GetComponent<Enemy>().Hp -= 10;
            Destroy(gameObject);
        }
    }
}
