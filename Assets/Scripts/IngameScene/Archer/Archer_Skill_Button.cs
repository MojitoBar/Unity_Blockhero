using UnityEngine;
using System.Collections;

public class Archer_Skill_Button : MonoBehaviour
{
    public GameObject Archer = null;
    public GameObject Arrow;
    public Sprite LastAnim;

    bool check = false;

    public void Skill()
    {
        Archer.GetComponent<CHero>().anim.SetBool("Skill", true);
        check = false;
    }

    void Update()
    {
        if (Archer.GetComponent<SpriteRenderer>().sprite == LastAnim)
        {
            if (!check)
            {
                CreateBat();
            }
            check = true;
        }
    }

    public void CreateBat()
    {
        GameObject obj = (GameObject)Instantiate(Arrow);
        obj.transform.parent = Archer.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = Vector3.zero;
    }
}
