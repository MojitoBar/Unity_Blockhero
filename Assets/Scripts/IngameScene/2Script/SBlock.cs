using UnityEngine;
using System.Collections;

public class SBlock : MonoBehaviour
{
    public enum BlockType
    {
        Posion,
        Sword,
        Magic,
        Bow
    }

    public enum BlockState
    {
        Match,
        Wait,
        Move
    }


    public bool isChecked = false;

    public BlockType mType;
    public BlockState mState;
    public CBlockData mData;


    public Sprite[] data;
    public SpriteRenderer Sprite;
    public SBlockMng blockmng;

    void Start()
    {

    }

    void Update()
    {
        // isMoving 이 true일때
        _LocalPosition = Vector2.Lerp(_LocalPosition,
            new Vector2(mData.mX, mData.mY),
            0.2f);

        // isMatching이 true일때
        _LocalScale = Vector2.Lerp(_LocalScale, new Vector2(0, 0), 0.2f);

        if(isMoveEnd())
        {
            blockmng.isMoving = false;
        }

        if(isMatchEnd())
        {
            blockmng.isMatching = false;
        }

    }
    void SetStateToMatch()
    {
        mState = BlockState.Match;
    }

    public bool isMoveEnd()
    {
        if (blockmng.isMoving == true &&
            _LocalPosition == new Vector2(mData.mX, mData.mY))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool isMatchEnd()
    {
        //isMatching 이 true이고 다줄어들었을때
        if(blockmng.isMatching == true &&
            _LocalScale == new Vector2(0,0))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public void MoveBlock(int dir)// 0 , 1, 2, 3 => 상 하 좌 우
    {
        switch (dir)
        {
            case 0:
                mData.mY--;//상
                break;
            case 1:
                mData.mY++;//하
                break;
            case 2:
                mData.mX--;//좌
                break;
            case 3:
                mData.mX++;//우
                break;
        }
    }

    public void SetPositionwithData()// 인덱스로 포지션 바꿔준다.
    {
        _LocalPosition = new Vector2(mData.mX, (-mData.mY));
    }

    public void RandomwTypeSet()
    {
        int typeNum = Random.Range(0, 4);
        mType = (BlockType)typeNum;
        Sprite.sprite = data[typeNum];
    }

    public Vector2 _LocalPosition
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }

    public Vector2 _LocalScale
    {
        get { return transform.localScale; }
        set { transform.localPosition = value; }
    }

}
