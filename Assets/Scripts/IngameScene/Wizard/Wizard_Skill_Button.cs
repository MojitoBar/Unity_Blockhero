using UnityEngine;
using System.Collections;

public class Wizard_Skill_Button : MonoBehaviour
{
    public GameObject Wizard = null;
    public GameObject Bat;
    public Sprite LastAnim;

    bool check = false;

    public void Skill()
    {
        Wizard.GetComponent<CHero>().anim.SetBool("Skill", true);
        check = false;
    }

    void Update()
    {
        if (Wizard.GetComponent<SpriteRenderer>().sprite == LastAnim)
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
        GameObject obj = (GameObject)Instantiate(Bat);
        obj.transform.parent = Wizard.transform;
        obj.transform.localScale = new Vector3(2, 2, 2);
        obj.transform.localPosition = Vector3.zero;
    }
}
