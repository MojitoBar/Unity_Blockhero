using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CBlockMng : MonoBehaviour {

    public enum GameState
    {
        PreProcess,
        WatingTouch,
        MoveBlock,
        CreateNewBlock,
        MatchWait,
    }

    private float RectBoxWidth;
    private float RectBoxHeight;


    public GameState state;

    public Camera BlockCamera;

    public List<CBlock> blocks;
    

    public int B_FieldWidth = 8;
    public int B_FieldHeight = 8;

    public CBlock[,] m_Blocks;  // (0,0) , (0,7) , (7,0), (7,7) 는 쓰지않는다.
    
    public List<CMatchData> matchList = new List<CMatchData>();

    

    bool firstSidecreate = true;
    bool swipe = false;
    Vector2 prePos = Vector2.zero;
    Vector2 nowPos;
    RaycastHit hit;

    void Awake()
    {
        m_Blocks = new CBlock[B_FieldWidth, B_FieldHeight];
    }

    // Use this for initialization
    void Start() {
        state = GameState.WatingTouch;
        RectBoxWidth = RectBoxHeight = 60;

        RandomInit();

        UpdateSideBlock();
    }

    // Update is called once per frame
    void Update()
    {

        if(state == GameState.PreProcess)
        {
           
            Debug.Log("처리");
            state = GameState.WatingTouch;
        }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = BlockCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "Block")
                    {
                        Debug.Log("블록좌표" + hit.transform.localPosition);
                        Debug.Log("블록인덱스" + hit.transform.GetComponent<CBlock>()._mData.mX + ", " +
                            hit.transform.GetComponent<CBlock>()._mData.mY);
                        prePos = Input.mousePosition;
                        swipe = true;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                //bug.Log("뗄때 : " + Input.mousePosition);
                nowPos = Input.mousePosition;

                Vector2 hitBlockCoord = new Vector2((int)(hit.transform.localPosition.x + 0.5f), (int)(-(hit.transform.localPosition.y - 0.5f)));
                if (swipe == true)
                {
                    Vector2 distance = new Vector2(nowPos.x - prePos.x, nowPos.y - prePos.y); // 거리

                    if (distance.y > 10.0f && Mathf.Abs(distance.x) < 50.0f)//상
                    {
                        Debug.Log("위로이동");
                        MoveBlock(hitBlockCoord, new Vector2(0, 1), true);
                        state = GameState.MatchWait;
                    }
                    else if (distance.y < -10.0f && Mathf.Abs(distance.x) < 50.0f)//하
                    {
                        Debug.Log("아래로이동");
                        MoveBlock(hitBlockCoord, new Vector2(0, -1), true);
                        state = GameState.MatchWait;
                    }
                    else if (distance.x < -10.0f && Mathf.Abs(distance.y) < 50.0f)//좌
                    {
                        Debug.Log("좌로이동");
                        MoveBlock(hitBlockCoord, new Vector2(-1, 0), false);
                        state = GameState.MatchWait;
                    }
                    else if (distance.x > 10.0f && Mathf.Abs(distance.y) < 50.0f)//우
                    {
                        Debug.Log("우로이동");
                        MoveBlock(hitBlockCoord, new Vector2(1, 0), false);
                        state = GameState.MatchWait;
                    }


                UpdateSideBlock();
                DeleteSideBlocks();
                //state = GameState.PreProcess;
                swipe = false;

                }
        }

    }
    #region BlockSqawnAndDelete

    public void DeleteSideBlocks()
    {
        Debug.Log("옆삭제");
        for(int i = 1; i <= B_FieldHeight - 2; i++)
        {
            BlockDestroy(i, 7);
            BlockDestroy(i, 0);
            BlockDestroy(0, i);
            BlockDestroy(7, i);
        }
    }
    public void BlockDestroy(int x, int y)
    {
        Destroy(m_Blocks[x, y].gameObject);
        m_Blocks[x, y] = null;
    }


    public void RandomInit() // 랜덤으로 블록들 생성
    {
        
        int heart = 0;
        int sword =0;
        int magic =0; 
        int bow =0;
        int type ;
        
        for (int i = 1; i <= B_FieldWidth - 2; i++)
        {
            for (int j = 1; j <= B_FieldHeight - 2; j++)
            {
                type = Random.Range(0, blocks.ToArray().Length);
                m_Blocks[i, j] = (CBlock)Instantiate(blocks[type]);
                m_Blocks[i, j].transform.SetParent(this.transform);
                m_Blocks[i, j].transform.localPosition = new Vector3(
                    i, -j );
                m_Blocks[i, j].SetData(i, j);
                Debug.Log(i.ToString() + " , " + j.ToString() + " : " + m_Blocks[i, j]);
            }
        }
    }
    public void UpdateSideBlock()
    {
        Debug.Log("옆생성");
        if (state == GameState.MoveBlock) return;
        for (int i = 1; i <= B_FieldHeight - 2; i++)
        {
            CreateSideBlock(7, i, 1, i);
            CreateSideBlock(0, i, 6, i);
            CreateSideBlock(i, 0, i, 6);
            CreateSideBlock(i, 7, i, 1);
        }
        
        firstSidecreate = false;
    }
    public void CreateSideBlock(int x1, int y1, int x2 , int y2)
    {
        if (firstSidecreate == true || m_Blocks[x1, y1] == null)
        {
        }
            m_Blocks[x1, y1] = (CBlock)Instantiate(m_Blocks[x2, y2]);
            m_Blocks[x1, y1].transform.SetParent(this.transform);
            m_Blocks[x1, y1].transform.localPosition = new Vector3(x1, -y1);
            m_Blocks[x1, y1].SetData(x1, y1);

        //if (m_Blocks[x1,y1] == null | m_Blocks[x1, y1]._mType != m_Blocks[x2, y2]._mType )
        //{
        //    if (m_Blocks[x1, y1] != null)
        //    {
        //        Destroy(m_Blocks[x1, y1].gameObject);
        //        m_Blocks[x1, y1] = null;
        //    }

        //    m_Blocks[x1, y1] = (CBlock)Instantiate(m_Blocks[x2, y2]);
        //    m_Blocks[x1, y1].transform.SetParent(this.transform);
        //    m_Blocks[x1, y1].transform.localPosition = new Vector3(x1, -y1);
        //    m_Blocks[x1, y1].SetData(x1, y1);
        //}
    }

    #endregion

    #region Match
    void matchProcess()
    {
        foreach(CMatchData data in matchList)
        {
            int x = data.mX;
            int y = data.mY;
            int length = data.mLength;

            if(m_Blocks[x, y]._mState == CBlock.BlockState.MoveEnd)
            {
                continue;
            }

            switch(data.mDir)
            {
                case CMatchData.Direction.LEFT:
                    for (int i = 0; i < length; ++i) m_Blocks[x - i, y].Match();
                    break;
                case CMatchData.Direction.RIGHT:
                    for (int i = 0; i < length; ++i) m_Blocks[x + i, y].Match();
                    break;
                case CMatchData.Direction.UP:
                    for (int i = 0; i < length; ++i) m_Blocks[x, y - i].Match();
                    break;
                case CMatchData.Direction.DOWN:
                    for (int i = 0; i < length; ++i) m_Blocks[x, y + i].Match();
                    break;
            }

        }

        matchList.Clear();
    }

    bool checkMatch(CBlock pBlock)
    {
        CBlock.BlockName type = pBlock._mType;
        int x = pBlock._X;
        int y = pBlock._Y;

        if (x >= 3 &&  x <= B_FieldWidth - 2 &&type == m_Blocks[x - 1, y]._mType && type == m_Blocks[x - 2, y]._mType) return true;
        //right
        else if (x <= B_FieldWidth - 4 && x >= 1 && type == m_Blocks[x + 1, y]._mType && type == m_Blocks[x + 2, y]._mType) return true;
        //top
        else if (y <= B_FieldHeight - 2 && y >= 3 && type == m_Blocks[x, y - 1]._mType && type == m_Blocks[x, y - 2]._mType) return true;
        //bottom
        else if (y >= 1 && y <= B_FieldHeight - 4 && type == m_Blocks[x, y + 1]._mType && type == m_Blocks[x, y + 2]._mType) return true;

        return false;
    }

    public void FinishMoveEvent(CBlock pBlock)
    {

        if (findMatchfromBlock(pBlock))
            matchProcess();
    }

    bool findMatchfromBlock(CBlock pBlock)
    {
        if (pBlock.b_state == CBlock.BlockState.MatchEnd) return false;
        bool match = false;

        bool hor, ver, left, top, right, bottom;

        hor = CompareHorizon(pBlock);//좌우로 1개씩
        ver = CompareVertical(pBlock);// 위앙래로 1개씩
        left = CompareDirection(CMatchData.Direction.LEFT, pBlock);// 각방향으로 2개
        top = CompareDirection(CMatchData.Direction.UP, pBlock);
        right = CompareDirection(CMatchData.Direction.RIGHT, pBlock);
        bottom = CompareDirection(CMatchData.Direction.DOWN, pBlock);

        match = hor || ver || left || top || right || bottom;

        if (match)
        {
            if (hor)
            {
                if (left && right)
                    matchList.Add(new CMatchData(CMatchData.Direction.RIGHT,
                        5, pBlock._X - 2, pBlock._Y));
                else if (left)
                    matchList.Add(new CMatchData(CMatchData.Direction.RIGHT,
                        4, pBlock._X - 2, pBlock._Y));
                else if (right)
                    matchList.Add(new CMatchData(CMatchData.Direction.RIGHT,
                        4, pBlock._X - 1, pBlock._Y));
                else
                    matchList.Add(new CMatchData(CMatchData.Direction.RIGHT,
                        3, pBlock._X - 1, pBlock._Y));

            }
            else
            {
                if (left)
                    matchList.Add(new CMatchData(CMatchData.Direction.LEFT,
                        3, pBlock._X, pBlock._Y));
                if (right)
                    matchList.Add(new CMatchData(CMatchData.Direction.RIGHT,
                        3, pBlock._X, pBlock._Y));
            }

            if (ver)
            {
                if (top && bottom)
                    matchList.Add(new CMatchData(CMatchData.Direction.UP,
                        5, pBlock._X, pBlock._Y + 2));
                else if (top)
                    matchList.Add(new CMatchData(CMatchData.Direction.UP,
                        4, pBlock._X, pBlock._Y + 1));
                else if (bottom)
                    matchList.Add(new CMatchData(CMatchData.Direction.UP,
                        4, pBlock._X, pBlock._Y + 2));
                else
                    matchList.Add(new CMatchData(CMatchData.Direction.UP,
                        3, pBlock._X, pBlock._Y + 1));
            }
            else
            {
                if (top)
                    matchList.Add(new CMatchData(CMatchData.Direction.UP,
                        3, pBlock._X, pBlock._Y));
                if (bottom)
                    matchList.Add(new CMatchData(CMatchData.Direction.DOWN,
                        3, pBlock._X, pBlock._Y));
            }
        }

        return match;

    }

    bool CompareHorizon(CBlock pBlock)
    {
        int x = pBlock._X;
        int y = pBlock._Y;

        return (CompareType(pBlock, x - 1, y) && CompareType(pBlock, x + 1, y));


    }
    bool CompareVertical(CBlock pBlock)
    {
        int x = pBlock._X;
        int y = pBlock._Y;

        return (CompareType(pBlock, x, y - 1) && CompareType(pBlock, x, y + 1));
    }
    bool CompareDirection(CMatchData.Direction pDir, CBlock pBlock)
    {
        int x = pBlock._X;
        int y = pBlock._Y;

        switch (pDir)
        {
            case CMatchData.Direction.LEFT:
                return (CompareType(pBlock, x - 2, y) && CompareType(pBlock, x - 1, y));
            case CMatchData.Direction.RIGHT:
                return (CompareType(pBlock, x + 2, y) && CompareType(pBlock, x + 1, y));
            case CMatchData.Direction.DOWN:
                return (CompareType(pBlock, x, y - 2) && CompareType(pBlock, x, y - 1));
            case CMatchData.Direction.UP:
                return (CompareType(pBlock, x, y + 2) && CompareType(pBlock, x, y + 1));
        }

        return false;

    }

    public bool CompareType(CBlock pBlock, int px, int py)
    {
        if (px < 1 || py < 1 || px > B_FieldWidth - 2 || py > B_FieldHeight) return false;
        return pBlock.GetType() == m_Blocks[px, py].GetType();
    }

    #endregion

    #region Move


    public void MoveBlock(Vector2 coord, Vector2 pDirection, bool verOrhor) // 상하좌우 0,1,2,3 좌우 이거나 상하
    {
        state = GameState.MoveBlock;

        if (verOrhor == true) // 상하
        {
            
            for (int i = 0; i < B_FieldHeight; i++)
            {
                if (m_Blocks[(int)coord.x, i] != null)
                {
                    m_Blocks[(int)coord.x, i].BlockMove(pDirection);
                }
                else { Debug.Log("null 인것" + m_Blocks[(int)coord.x, i]); }
            }
            state = GameState.WatingTouch;

            if (pDirection.y == -1)
                DownMove((int)coord.x);// 아래로
            else if (pDirection.y == 1)
                UpMove((int)coord.x); // 위로

        }
        else // 좌우
        {
            
            for (int i = 0; i < B_FieldWidth; i++)
            {
                if (m_Blocks[i, (int)coord.y] != null)
                {
                    m_Blocks[i, (int)coord.y].BlockMove(pDirection);
                }
                else { Debug.Log(m_Blocks[i, (int)coord.y]); }
            }

            state = GameState.WatingTouch;

            if (pDirection.x == -1)
                LeftMove((int)coord.y);// 좌
            else if (pDirection.x == 1)
                RightMove((int)coord.y); // 우
            
        }

        

        //for (int j = 0; j < 8; j++)
        //{
        //    Debug.Log((int)coord.x + " , " + j.ToString() + " : " + m_Blocks[(int)coord.x, j]);
        //}
        //for (int j = 1; j <= 6; j++)
        //{
        //    Debug.Log(j.ToString() + " , " + (int)coord.y +  " : " + m_Blocks[j, (int)coord.y]);
        //}

        

    }
    public void LeftMove(int yPos)
    {

        CBlock tempBlock;
        tempBlock = m_Blocks[1, yPos];

        for(int i = 0; i < 7; i++)
        {
            m_Blocks[i, yPos] = m_Blocks[i + 1, yPos];
        }

        m_Blocks[6, yPos] = tempBlock;
        //m_Blocks[6, yPos].LocalPosition = new Vector2(6, -yPos);
        
    }
    public void RightMove(int yPos)
    {
        CBlock tempBlock;
        tempBlock = m_Blocks[6, yPos];
        for(int i = 1; i < 6; i++)
        {
            m_Blocks[7 - i , yPos] = m_Blocks[7 - i - 1, yPos];
        }

        m_Blocks[1, yPos] = tempBlock;
    }
    public void UpMove(int xPos)
    {
        CBlock tempBlock;
        tempBlock = m_Blocks[xPos, 1];
        for (int i = 1; i < 6; i++)
        {
            m_Blocks[xPos, i] = m_Blocks[xPos, i+1];
        }

        m_Blocks[xPos, 6] = tempBlock;
    }
    public void DownMove(int xPos)
    {
        CBlock tempBlock;
        tempBlock = m_Blocks[xPos, 6];
        for (int i = 1; i < 6; i++)
        {
            m_Blocks[xPos, 7 - i] = m_Blocks[xPos, 7 - i - 1];
        }

        m_Blocks[xPos, 1] = tempBlock;
        m_Blocks[xPos, 1].LocalPosition = new Vector2(xPos, -1);
        Debug.Log("다운");
    }


    #endregion




    //void SwapBlock(CBlock a, CBlock b)
    //{
    //    m_Blocks[a._X, a._Y ] = b;
    //    m_Blocks[b._X, b._Y ] = a;

    //    a.SetData();
    //    b.SetData();
    //}
}


