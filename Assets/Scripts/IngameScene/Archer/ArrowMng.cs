using UnityEngine;
using System.Collections;

public class ArrowMng : MonoBehaviour
{
    public float speed;
    public float height;

    void Update()
    {
        gameObject.transform.localPosition += new Vector3(speed * Time.deltaTime, 0, 0);

        transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);

        //if (transform.localPosition.y <= (height))
        //{
        //    transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
        //}
        //else if (transform.localPosition.y <= (height))
        //{
        //    transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);
        //}

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
