using UnityEngine;
using System.Collections;

public class PopupMove : MonoBehaviour {

    Animator animator;
    Vector2 endpos = new Vector2(0f,1280.0f);
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //transform.localPosition = Vector2.Lerp(transform.localPosition, endpos, 0.3f);
	}

    public void OnPopUpOn()
    {
        //endpos = new Vector2(0f, 0f);
        animator.SetBool("On", true);
    }
    public void OnPopUpOut()
    {
        //endpos = new Vector2(0f, 1280f);
        animator.SetBool("On", false);
    }
   
}
