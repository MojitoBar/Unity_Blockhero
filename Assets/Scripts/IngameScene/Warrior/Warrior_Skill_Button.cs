using UnityEngine;
using System.Collections;

public class Warrior_Skill_Button : MonoBehaviour
{
    public GameObject Warrior = null;

    public void Skill()
    {
        Warrior.GetComponent<CHero>().anim.SetBool("Skill", true);
    }
}
