using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SHeroView : MonoBehaviour {

    public Animation anim;
    public Image image;

    public AnimationClip aniclip;
    public Sprite sprite;
    

    public SHeroData herodata;
	// Use this for initialization
	void Start () {
        image.sprite = this.sprite;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
