using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroMng : MonoBehaviour {


    SHeroView[] ClassArr = new SHeroView[3]; // 
    List<SHeroView>[] HeroArr = new List<SHeroView>[3]; // 0 궁수 1 전사 2 법사
    List<GameObject>[] HeroObjArr = new List<GameObject>[3];


    public GameObject nowHero; // 현재 히어로
    public List<GameObject> SlotArr = new List<GameObject>();// 슬롯들;

    int Classpoint = 1; // 0 궁수 1 전사 2 법사
    int point; // 슬롯 가르키는것
    bool characterSelected= false; // 슬롯이 선택됫는지

	// Use this for initialization
	void Start () {
	    // 클래스마다 배열삽입
        for(int i = 0; i < GameContext.Instance.AllHeroList.Count; i ++)
        {
            switch(GameContext.Instance.AllHeroList[i].herodata.heroclass)
            {
                case 0:
                    HeroArr[0].Add(GameContext.Instance.AllHeroList[i]);
                    break;
                case 1:
                    HeroArr[1].Add(GameContext.Instance.AllHeroList[i]);
                    break;
                case 2:
                    HeroArr[2].Add(GameContext.Instance.AllHeroList[i]);
                    break;
            }
        }

	}
	
    public void FillHeroList(int classnumber) // 클래스번호
    {
        for(int i = 0; i < HeroArr[classnumber].Count; i++)
        {
            GameObject temp = (GameObject)Instantiate(HeroArr[classnumber][i].gameObject);
            temp.transform.parent.SetParent(SlotArr[i].transform);
            temp.transform.localPosition =Vector2.zero;
            HeroObjArr[classnumber].Add(temp);
        }
    }
    public void VisOn(int classnumber)
    {
        for (int i = 0; i < HeroArr[classnumber].Count; i++)
        {
            HeroObjArr[classnumber][i].SetActive(true);
        }
    }
    public void VisOff(int classnumber)
    {
        for (int i = 0; i < HeroArr[classnumber].Count; i++)
        {
            HeroObjArr[classnumber][i].SetActive(false);
        }
    }
	// Update is called once per frame
	void Update () {
	    
	}

    // 현재히어로의 데이터와 이미지를 바꾸고 현재클래스 갱신
    public void CharacterSelect()
    {
        
        if (characterSelected == true) // 선택되잇을때만 
        {
            ClassArr[Classpoint] = HeroArr[Classpoint][point]; //클래스 배열
             // 현재 히어로 보여주는곳
        }
    }

    // 포인트를 슬롯 배열의 인덱스로 설정하고 
    public void ClickSlot(int index)
    {
        if (point == index) // 현재 인덱스하고 같으면 체크 취소
        {
            characterSelected = false;
        }
        else
        {
            point = index;
            characterSelected = true;
        }
    }

    public void ChangeClass(int classnumber) // 0 1 2 궁수 전사 마법사
    {
        //현재거 안보이게하기

        if(Classpoint != classnumber)
        {
            Classpoint = classnumber;
        }

        //다른 바뀐것으로 보이게하기

            
    }

    public void reset()
    {
        
    }

    public void savetoSingleton()
    {

    }
}
