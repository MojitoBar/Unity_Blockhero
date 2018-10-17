using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CBlock : MonoBehaviour {

    public enum BlockName
    {
        Heart,
        Sword,
        Magic,
        Bow
    }// 블록의종류

    public enum BlockState
    {
        Idle,
        Move,
        Selected,
        MatchWait,
        MatchEnd,
        MoveEnd,

    }

    

    #region Variable 

    static CBlockMng m_BlockMng;

    public BlockName b_name;//종류
    public BlockState b_state;
    public SpriteRenderer sprite;

    [SerializeField]
    private CBlockData mData;
    private Vector2 blockPos;// 블록의 좌표

    

    #endregion 

    #region -============ Default function ===========
    // Use this for initialization
    void Start() {
        if (!m_BlockMng) m_BlockMng = transform.parent.GetComponent<CBlockMng>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    #endregion
    

    #region Custom Function

    #region SetData
     
    public void SetData(int pX, int pY)//배열
    {
        mData = new CBlockData(pX, pY);
    }
    
    public void SetData()
    {
        mData.mX = (int)(transform.localPosition.x + 0.5);
        mData.mY = (int)(-(transform.localPosition.y - 0.5));
    }
    #endregion


    #region Match function
    public void Match()
    {
        if (b_state == BlockState.MatchWait) return;
        b_state = BlockState.MatchWait;

        StartCoroutine(matchCoroutine());
    }

    IEnumerator matchCoroutine()
    {
        yield return StartCoroutine(scaleCoroutine(Vector2.zero));
        b_state = BlockState.MatchEnd;
    }

    

    #endregion

    #endregion

    #region ==================== 이동 =====================

    public void BlockMove(Vector2 dir)
    {
        if (b_state == BlockState.Move) return;
        StartCoroutine(BlockMoveCorutine(dir));
    }

    IEnumerator BlockMoveCorutine(Vector2 direction)//상하좌우
    {
        Vector2 startPos = transform.localPosition;
        Vector2 endPos = startPos + direction;

        float startTime = Time.time;
        float duration = .2f;
        b_state = BlockState.Move;

        while (Time.time - startTime <= duration)
        {
            LocalPosition = Vector2.Lerp(startPos, endPos, (Time.time - startTime) / duration);
            yield return null;
        }

        b_state = BlockState.Idle;
        LocalPosition = endPos;
        SetData();
        //m_BlockMng.FinishMoveEvent(this);//매치되는걸 찾는 함수 호출
        m_BlockMng.UpdateSideBlock();
        m_BlockMng.DeleteSideBlocks();
        // 주변 삭제

    }

    IEnumerator scaleCoroutine(Vector2 endVec)
    {
        float startTime = Time.time; // 현재시간을 starttime으로
        Vector2 orgVec = transform.localScale; // orgvec을 현재 스케일

        while (Time.time - startTime <= 0.3f) // 함수호출하고 0.3초가되기전까지
        {
            transform.localScale = Vector2.Lerp(orgVec, endVec, (Time.time - startTime) / 0.3f);
            yield return null;
        }

        transform.localScale = endVec;
    }

    #endregion

    #region =========== getter =============
    public CBlockData _mData
    {
        get { return mData; }
    }
    public BlockState _mState
    {
        get { return b_state; }
    }

    public BlockName _mType
    {
        get { return b_name; }
    }

    public Vector2 LocalPosition
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }

    public int _X
    {
        get { return mData.mX; }
    }

    public int _Y
    {
        get { return mData.mY; }
    }

    #endregion
}
