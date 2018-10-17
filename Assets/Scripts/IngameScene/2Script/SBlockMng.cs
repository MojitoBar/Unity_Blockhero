using UnityEngine;
using System.Collections;

public class SBlockMng : MonoBehaviour {
    public bool isMoving;
    public bool isMatching;
    public bool isEmpty;
    public bool isStable;

    public SBlock[,] mBlockArray = new SBlock[8, 8]; //블록배열
    public SBlock mBlock; // 블록프리펩을 담을 객체
    public Camera BlockCamera; // 블록 카메라
    

    // 터치에 쓰임
    Vector2 prePos = Vector2.zero;
    Vector2 nowPos;
    RaycastHit hit; // 터치 레이캐스트
    bool swipe = false; // 스와이프가 됬는지
    int swipeDir = 0; // 0123 상하좌우
    Vector2 hitBlockCoord;


    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (isStable == true)
        {

            #region TouchProccess
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = BlockCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "Block")
                    {
                        prePos = Input.mousePosition;
                        swipe = true;
                        Debug.Log(hit.transform.GetComponent<Block>().mType);
                        Debug.Log(mBlockArray[(int)hit.transform.localPosition.x, -(int)hit.transform.localPosition.y].mType);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {

                bool moved = false; // 움직일수잇을만큼 됫는지
                nowPos = Input.mousePosition;

                if (swipe == true)
                {
                    hitBlockCoord
                    = new Vector2(
                   (int)(hit.transform.GetComponent<Block>().mData.mX),
                   (int)(hit.transform.GetComponent<Block>().mData.mY)
                   );

                    Vector2 distance = new Vector2(nowPos.x - prePos.x, nowPos.y - prePos.y); // 거리

                    if (distance.y > 10.0f && Mathf.Abs(distance.x) < 50.0f)//상
                    {

                        moved = true;
                        swipeDir = 0;


                    }
                    else if (distance.y < -10.0f && Mathf.Abs(distance.x) < 50.0f)//하
                    {

                        moved = true;
                        swipeDir = 1;
                    }
                    else if (distance.x < -10.0f && Mathf.Abs(distance.y) < 50.0f)//좌
                    {

                        moved = true;
                        swipeDir = 2;
                    }
                    else if (distance.x > 10.0f && Mathf.Abs(distance.y) < 50.0f)//우
                    {

                        moved = true;
                        swipeDir = 3;
                    }
                    swipe = false;
                }
                if (moved == true)
                {
                    moved = false;
                    Move();
                }
            }
        #endregion

        }



    }

    void Wait()
    {
        if(isStable == true && isMoving == false)
        {
            Match();
        }
        if(isStable == true && isMatching == false && isEmpty == true)
        {
            Fill();
        }
    }

    void Match()
    {
        isStable = false;
        
        for(int x = 1; x < 7; x ++)
        {
            for(int y = 1; y < 7; y ++)
            {
                if(CheckBlockoverHorizontal(x,y,mBlockArray[x,y].mType) >= 3)
                {
                    isMatching = true;

                }
            }
        } 
    }

    int Match2(int x, int y, SBlock.BlockType key)
    {
        if (mBlockArray[x, y].isChecked == true)
        {
            return 0;
        }
        mBlockArray[x, y].isChecked = true;
        if (x < 1 || x > 6)
        {
            return 0;
        }
        if (mBlockArray[x, y].mType != key)
        {
            return 0;
        }
        return 1;
    }
       
    
    void Fill()
    {

    }
    void Move()
    {
        isStable = false;
        isMoving = true;
        switch (swipeDir)
        {
            case 0:
                MoveUp((int)hitBlockCoord.x);
                break;
            case 1:
                MoveDown((int)hitBlockCoord.x);
                break;
            case 2:
                MoveLeft((int)hitBlockCoord.y);
                break;
            case 3:
                MoveRight((int)hitBlockCoord.y);
                break;
        }
    }
    #region MoveFunction
    void MoveUp(int xPos)
    {


        for (int i = 0; i < 7; i++)
        {
            mBlockArray[xPos, i] = mBlockArray[xPos, i + 1];
            mBlockArray[xPos, i + 1].MoveBlock(0);
            //mBlockArray[xPos, i + 1].MoveAnimation(new Vector2(0, 1));
        }
        mBlockArray[xPos, 7] = null;
    }
    void MoveDown(int xPos)
    {
        for (int i = 7; i > 0; i--)
        {
            mBlockArray[xPos, i] = mBlockArray[xPos, i - 1];
            mBlockArray[xPos, i - 1].MoveBlock(1);
            //mBlockArray[xPos, i - 1].MoveAnimation(new Vector2(0, -1));
        }
        mBlockArray[xPos, 0] = null;

    }
    void MoveLeft(int yPos)
    {
        for (int i = 0; i < 7; i++)
        {
            mBlockArray[i, yPos] = mBlockArray[i + 1, yPos];
            mBlockArray[i + 1, yPos].MoveBlock(2);
            //mBlockArray[i + 1, yPos].MoveAnimation(new Vector2(-1, 0));
        }
        mBlockArray[7, yPos] = null;
    }
    void MoveRight(int yPos)
    {

        for (int i = 7; i > 0; i--)
        {
            mBlockArray[i, yPos] = mBlockArray[i - 1, yPos];
            mBlockArray[i - 1, yPos].MoveBlock(3);
            //mBlockArray[i - 1, yPos].MoveAnimation(new Vector2(1, 0));
        }
        mBlockArray[0, yPos] = null;
    }
    #endregion

    #region MatchFunction
    int CheckBlockoverHorizontal(int x, int y, SBlock.BlockType type)
    {
        if (x < 1 || x > 6)
            return 0;
        if (mBlockArray[x, y].mType != type)
            return 0;
        if (mBlockArray[x, y].mState == SBlock.BlockState.Match)
        {
            return 0;

        }
       
        
        //mBlockArray[x, y].SetStateToMatch();


        return 1 +
            CheckBlockoverHorizontal(x - 1, y, type) +
            CheckBlockoverHorizontal(x + 1, y, type);
    }
    //int CheckBlockoverVertical(int x, int y, SBlock.BlockType type)
    //{


    //    if (y < 1 || y > 6)
    //        return 0;
    //    else if (mBlockArray[x, y].mType != type)
    //        return 0;
    //    else if (mBlockArray[x, y].mState == Block.BlockState.Match)
    //        return 0;


    //    mBlockArray[x, y].SetStateToMatch();

    //    return 1 +
    //        CheckBlockoverVertical(x, y + 1, type) +
    //        CheckBlockoverVertical(x, y - 1, type);
    //}
    #endregion
    
    #region SettingFunciont
    bool CheckMatchForSetRandomType(SBlock block)
    {
        int x = block.mData.mX;
        int y = block.mData.mY;
        //left
        if (x > 2 && x < 7 && block.mType == mBlockArray[x - 1, y].mType && block.mType == mBlockArray[x - 2, y].mType) return true;
        //right
        else if (x < 5 && x > 0 && block.mType == mBlockArray[x + 1, y].mType && block.mType == mBlockArray[x + 2, y].mType) return true;
        //top
        else if (y > 2 && y < 7 && block.mType == mBlockArray[x, y - 1].mType && block.mType == mBlockArray[x, y - 2].mType) return true;
        //bottom
        else if (y < 5 && y > 0 && block.mType == mBlockArray[x, y + 1].mType && block.mType == mBlockArray[x, y + 2].mType) return true;
      
        return false;
    }
    void InitBlockArray()
    {
        for (int i = 1; i < 7; i++)
        {
            for (int j = 1; j < 7; j++)
            {
                if (mBlockArray[i, j] == null)
                {
                    mBlockArray[i, j] = (SBlock)Instantiate(mBlock);
                    mBlockArray[i, j].transform.SetParent(this.transform);
                    mBlockArray[i, j].RandomwTypeSet();
                    mBlockArray[i, j].mData = new CBlockData(i, j);

                }
            }
        }
    }
    void SetAllRandomType()
    {
        for (int i = 1; i < 7; i++)
        {
            for (int j = 1; j < 7; j++)
            {
                mBlockArray[i, j].RandomwTypeSet();
                while (CheckMatchForSetRandomType(mBlockArray[i, j]))
                    mBlockArray[i, j].RandomwTypeSet();
            }
        }
    }
    //void UpdateOneSide(int x1, int y1, int x2, int y2)
    //{
    //    if (mBlockArray[x1, y1] != null)
    //        Destroy(mBlockArray[x1, y1].gameObject);
    //    mBlockArray[x1, y1] = (Block)Instantiate(mBlock);
    //    mBlockArray[x1, y1].transform.SetParent(this.transform);
    //    mBlockArray[x1, y1].SetType(mBlockArray[x2, y2].mType);
    //    mBlockArray[x1, y1].mData = new CBlockData(x1, y1);
    //    mBlockArray[x1, y1].SetPositionwithData();
    //}
    //void CreateSideBlock()
    //{
    //    for (int i = 1; i < 7; i++)
    //    {
    //        UpdateOneSide(7, i, 1, i);
    //        UpdateOneSide(0, i, 6, i);
    //        UpdateOneSide(i, 0, i, 6);
    //        UpdateOneSide(i, 7, i, 1);
    //    }
    //    // Debug.Log("옆블록");
    //}
    //void ChangeArrayPosition()
    //{
    //    for (int i = 0; i < 8; i++)
    //    {
    //        for (int j = 0; j < 8; j++)
    //        {
    //            if (mBlockArray[i, j] != null)
    //            {
    //                mBlockArray[i, j].SetPositionwithData();
    //            }
    //        }
    //    }
    //}

    #endregion
}
