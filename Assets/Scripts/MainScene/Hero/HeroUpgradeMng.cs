using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroUpgradeMng : MonoBehaviour {// 영웅 강화
    public List<GameObject> HeroList = new List<GameObject>();

    

    public GameObject CharacterSlot;

    public GameObject[] ComposeSlot = new GameObject[5];
    public int[] ComposePoint = new int[5];
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Upgrade()
    {
        //슬롯에있는 만큼 확률정해서 업그레이드
    }
}
