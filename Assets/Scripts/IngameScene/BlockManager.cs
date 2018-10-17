using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockManager : MonoBehaviour {
    public enum GameState
    {
        Wait, Move, Match, Create, SideMatch
    }

    #region public Variable

    public Block[,] mBlockArray = new Block[8, 8]; //블록배열

    public Block mBlock; // 블록프리펩을 담을 객체

    public Camera BlockCamera; // 블록 카메라
    
    public GameState gamestate; // 상태 객체

    public List<Block> matchBlockList = new List<Block>();

    public List<Block> tempBlockList = new List<Block>();

    #endregion

    #region private Variable
    // 터치에 쓰임
    Vector2 prePos = Vector2.zero; 
    Vector2 nowPos; 
    RaycastHit hit; // 터치 레이캐스트
    bool swipe = false; // 스와이프가 됬는지
    bool moved = false;
    int swipeDir = 0; // 0123 상하좌우
    Vector2 hitBlockCoord;
    bool checkend=false;
    #endregion

    #region Default Function
    void Awake()
    {
        Screen.SetResolution(542, 964, false);
    }

    void Start () {
        InitBlockArray();// 8x8 블록생성
        SetAllRandomType(); //랜덤으로 타입변경
        CreateSideBlock(); // 옆블록 갱신
        ChangeArrayPosition(); // 좌표갱신
        MatchFail();
        
    }

    // Update is called once per frame
    void Update () {
        switch(gamestate)
        {
            case GameState.Wait:
                ChangeArrayPosition();
                //터칠게 잇으면 match스테이트로
                if (checkend == false)
                {
                    checkend = true;
                    #region CheckHorizontal
                    for (int i = 1; i < 7; i++)
                    {
                        for (int j = 1; j < 7; j++)
                        {

                            CheckBlockoverHorizontal(i, j, i, j);
                            
                            if (tempBlockList.Count >= 3)
                            {
                                matchBlockList.AddRange(tempBlockList);
                                tempBlockList = null;
                                tempBlockList = new List<Block>();
                            }
                            else
                            {
                                tempBlockList = null;
                                tempBlockList = new List<Block>();
                            }
                            MatchFail();
                        }
                    }
                    #endregion
                    #region CheckVertical
                    for (int i = 1; i < 7; i++)
                    {
                        for (int j = 1; j < 7; j++)
                        {

                            CheckBlockoverVertical(i, j, i, j);

                            if (tempBlockList.Count >= 3)
                            {
                                matchBlockList.AddRange(tempBlockList);
                                tempBlockList = null;
                                tempBlockList = new List<Block>();
                            }
                            else
                            {
                                tempBlockList = null;
                                tempBlockList = new List<Block>();
                            }
                            MatchFail();
                        }
                    }
                    #endregion
                    
                    if (matchBlockList.Count > 0 && matchBlockList[1].isMoving == false)
                        gamestate = GameState.Match;
                }
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
                                
                            }
                        }
                    }

                    if (Input.GetMouseButtonUp(0))
                    {

                        moved = false; // 움직일수잇을만큼 됫는지
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
                            
                            gamestate = GameState.Move;
                        

                        }
                    }

                #endregion
                checkend = false;
                break;


            case GameState.Move:
                #region MoveProcess

                if (moved == true)
                {
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
                    
                    moved = false;
                }
                #endregion
                

                break;

            case GameState.Match:
                MatchEnd();
                break;

            case GameState.Create:
                ChageMatchedBlockIndex();

                
                for (int x = 1; x < 7; x++)
                {
                    for (int y = 1; y < 7; y++)
                    {
                        if(CheckmDataisLocalPosition(x,y) == false)
                        {
                            mBlockArray[x, y].MoveDownAnimation();
                        }
                    }
                }
                
                break;

            case GameState.SideMatch:
                CreateSideBlock();
                gamestate = GameState.Wait;
                break;
        }
        
    }
    #endregion

    #region Match
    public void MatchEnd()
    {
        //Debug.Log("matchend" + matchBlockList.Count.ToString());
        if (matchBlockList.Count > 0)
        {
            int x = 0;
            int y = 0;

            for (int i = 0; i < matchBlockList.Count; i++)
            {
                x = matchBlockList[i].mData.mX;
                y = matchBlockList[i].mData.mY;
                mBlockArray[x, y].DeleteAnimation();
            }

            matchBlockList = null;
            matchBlockList = new List<Block>();
        }
    }

    void MatchFail()
    {
        for(int i = 1; i < 7; i ++)
        {
            for(int j = 1; j < 7; j ++)
            {
                mBlockArray[i, j].isChecked = false;
            }
        }
    }

    int CheckBlockoverHorizontal(int x, int y, int ox, int oy)
    {
        if (x < 1 || x > 6)
            return 0;
        if (mBlockArray[x, y].mType != mBlockArray[ox, oy].mType)
            return 0;
        if (mBlockArray[x, y].isChecked == true)
            return 0;
        if (matchBlockList.Contains(mBlockArray[x, y]) == true)
            return 0;

        mBlockArray[x, y].isChecked = true;
        tempBlockList.Add(mBlockArray[x, y]);
        
        

        return 1 + 
            CheckBlockoverHorizontal(x - 1, y, ox, oy) +
            CheckBlockoverHorizontal(x + 1, y, ox, oy); 
    }

    int CheckBlockoverVertical(int x, int y, int ox, int oy)
    {
        if (y < 1 || y > 6)
            return 0;
        else if (mBlockArray[x, y].mType != mBlockArray[ox, oy].mType)
            return 0;
        else if (mBlockArray[x, y].isChecked == true)
            return 0;
        if (matchBlockList.Contains(mBlockArray[x, y]) == true)
            return 0;

        mBlockArray[x, y].isChecked = true;
        tempBlockList.Add(mBlockArray[x, y]);

        return 1 +
            CheckBlockoverVertical(x, y + 1, ox, oy) +
            CheckBlockoverVertical(x, y - 1, ox, oy);
    }
  
    bool CheckMatchForSetRandomType(Block block)
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

    #endregion

    #region SideBlock

    void CreateSideBlock() 
    {
        for(int i =1; i < 7; i ++)
        {
            UpdateOneSide(7, i, 1, i);
            UpdateOneSide(0, i, 6, i);
            UpdateOneSide(i, 0, i, 6);
            UpdateOneSide(i, 7, i, 1);
        }
       // Debug.Log("옆블록");
    }

    void UpdateOneSide(int x1, int y1, int x2, int y2)
    {
        if(mBlockArray[x1,y1] != null)
            Destroy(mBlockArray[x1, y1].gameObject);
        mBlockArray[x1, y1] = (Block)Instantiate(mBlock);
        mBlockArray[x1, y1].transform.SetParent(this.transform);
        mBlockArray[x1, y1].SetType(mBlockArray[x2, y2].mType);
        mBlockArray[x1, y1].mData = new CBlockData(x1, y1);
        mBlockArray[x1, y1].SetPositionwithData();
    }

    #endregion

    #region SwipeMove

    void MoveUp(int xPos)
    {
        for (int i = 0; i < 7; i++)
        {
            mBlockArray[xPos, i] = mBlockArray[xPos, i + 1];
            mBlockArray[xPos, i + 1].MoveBlock(0);
            mBlockArray[xPos, i + 1].MoveAnimation(new Vector2(0, 1));
        }
        mBlockArray[xPos, 7] = null;
    }
    void MoveDown(int xPos)
    {
        for (int i = 7; i > 0; i--)
        {
            mBlockArray[xPos, i] = mBlockArray[xPos, i - 1];
            mBlockArray[xPos, i - 1].MoveBlock(1);
            mBlockArray[xPos, i - 1].MoveAnimation(new Vector2(0, -1));
        }
        mBlockArray[xPos, 0] = null;

    }
    void MoveLeft(int yPos)
    {
        for (int i = 0; i < 7; i++)
        {
            mBlockArray[i, yPos] = mBlockArray[i+1, yPos];
            mBlockArray[i+1,yPos].MoveBlock(2);
            mBlockArray[i+1, yPos].MoveAnimation(new Vector2(-1,0));
        }
        mBlockArray[7, yPos] = null;
    }
    void MoveRight(int yPos)
    {
        
        for (int i = 7; i > 0; i--)
        {
            mBlockArray[i, yPos] = mBlockArray[i-1, yPos];
            mBlockArray[i-1, yPos].MoveBlock(3);
            mBlockArray[i - 1, yPos].MoveAnimation(new Vector2(1, 0));
        }
        mBlockArray[0, yPos] = null;
    }

    #endregion
    
    #region CreateandReset()

    void InitBlockArray()
    {
        for(int i = 1; i < 7; i++)
        {
            for( int j = 1; j < 7; j++)
            {
                if(mBlockArray[i,j] == null)
                {
                    mBlockArray[i, j] = (Block)Instantiate(mBlock);
                    mBlockArray[i, j].transform.SetParent(this.transform);
                    mBlockArray[i, j].RandomwTypeSet();
                    mBlockArray[i, j].mData = new CBlockData(i, j);
                }
            }
        }
    }
    void ChangeArrayPosition()
    {
        for(int i  =0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if(mBlockArray[i,j] != null)
                {
                    mBlockArray[i, j].SetPositionwithData();
                }
            }
        }
    }
    void SetAllRandomType()
    {
        for(int i = 1; i < 7; i ++)
        {
            for(int j = 1; j < 7; j ++)
            {
                mBlockArray[i, j].RandomwTypeSet();
                while (CheckMatchForSetRandomType(mBlockArray[i, j]))
                    mBlockArray[i, j].RandomwTypeSet();
            }
        }
    }

    void ChageMatchedBlockIndex()
    {
        for (int x = 1; x < 7; x++)
        {
            List<Block> list = new List<Block>(); // 터진것을담을 리스트
            for (int y = 6; y > 0; y--)
            {
                Block nowblock = mBlockArray[x, y];
                Sprite sprite = mBlockArray[x, y].Sprite.sprite;

                //Debug.Log("")
                if (nowblock.isMatched == true)
                {
                    if (list.Contains(nowblock) == false)
                    {
                        list.Add(nowblock);                                     //리스트에추가
                        nowblock._LocalPosition = new Vector2(x,  list.Count - 1);//포지션 위로올린다.
                    }
                    nowblock.isMatched = false;
                }
                else
                {
                    swap(x, y, x, y + list.Count);
                }
            }

            for(int i = 0; i < list.Count; i++)
            {
                Block block = mBlockArray[x, i + 1];
                block._LocalScale = new Vector2(1, 1);

                block.RandomwTypeSet();
                while (CheckMatchForSetRandomType(block))
                    block.RandomwTypeSet();
            }
            
        }

    }
    void IndexDownForMatch(List<Block> pList, int xPos)
    {
        for(int i = 0; i < pList.Count; i ++)
        {
            Block temp = pList[i];
            for(  int j = pList[i].mData.mY - 1   ;     j > 0    ;      j--  )
            {
                mBlockArray[xPos, j + 1] = mBlockArray[xPos, j];
                mBlockArray[xPos, j + 1].mData = new CBlockData(xPos, j + 1);
            }
            mBlockArray[xPos , 1] = temp;
            mBlockArray[xPos, 1].mData = new CBlockData(xPos, 1);// mData갱신
        }
    }

    

    bool CheckmDataisLocalPosition(int x, int y)
    {
        if (mBlockArray[x, y]._LocalPosition ==
            new Vector2(mBlockArray[x, y].mData.mX, -mBlockArray[x, y].mData.mY))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool isAllStop()
    {
        for(int i = 1; i < 7; i++)
        {
            for(int j = 1; j < 7; j++)
            {
                if (mBlockArray[i, j].isMoving == true)
                    return false;
            }
        }

        return true;
    }

    public void swap(int x, int y, int sx, int sy)
    {
        CBlockData tempdata = mBlockArray[x, y].mData;
        mBlockArray[x, y].mData = mBlockArray[sx, sy].mData;
        mBlockArray[sx, sy].mData = tempdata;

        Block temp = mBlockArray[x, y];
        mBlockArray[x, y] = mBlockArray[sx, sy];
        mBlockArray[sx, sy] = temp;
    }
    #endregion

   
}