using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public enum BlockType
    {
        Posion,
        Sword,
        Magic,
        Bow,
    }

    public enum BlockState
    {
        Idle,
        Move,
        Match,
    }
    public bool isMoving= false;
    public bool isChecked = false;
    public bool isMatched = false;
    public bool isDownblock = false;
    public BlockType mType; // 블록타입
    public BlockState mState;
    public CBlockData mData; // 인덱스

    public Sprite[] data;
    public SpriteRenderer Sprite;
    public BlockManager Blockmng;

	// Use this for initialization
	void Start () {
        Blockmng = GameObject.Find("BlockOffset").GetComponent<BlockManager>();
        mState = BlockState.Idle;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SetStateToMatch()
    {
        mState = BlockState.Match;
    }

    public void SetStateToIdle()
    {
        mState = BlockState.Idle;
        //Debug.Log("idle변경");
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
    public Sprite RandomwTypeSetforCreate()
    {
        int typeNum = Random.Range(0, 4);
        mType = (BlockType)typeNum;
        return data[typeNum];
    }

    public void SetType(BlockType type)
    {
        mType = type;
        Sprite.sprite = data[(int)type];
    }
    
    /// <summary>
    /// 인덱스 이동
    /// </summary>
    
    public void MoveBlock(int dir)// 0 , 1, 2, 3 => 상 하 좌 우
    {
        switch(dir)
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

    public void MoveAnimation(Vector2 direction)
    {
        StartCoroutine(MoveCoroutine(direction));
    }
    IEnumerator MoveCoroutine(Vector2 direction)
    {

        Vector2 startPos = transform.localPosition;
        Vector2 endPos = startPos + direction;


        float startTime = Time.time;
        float duration = 0.2f;


        isMoving = true;

        while (Time.time - startTime <= duration)
        {
            transform.localPosition = Vector2.Lerp(startPos, endPos, (Time.time - startTime) / duration);
            yield return null;
        }


        isMoving = false;

        if (Blockmng.gamestate == BlockManager.GameState.Move)
        {
            Blockmng.gamestate = BlockManager.GameState.SideMatch;
            Debug.Log("toSideMatch");
        }


    }



    public void MoveDownAnimation()
    {
        StartCoroutine(MoveDownCoroutine());
    }
    IEnumerator MoveDownCoroutine()
    {
        Vector2 startPos = transform.localPosition;

        float startTime = Time.time;
        float duration = 0.2f * (mData.mY + _LocalPosition.y);
        //Debug.Log("좌표 " + _LocalPosition);
        
            isMoving = true;
            

            while (Time.time - startTime <= duration)
            {
                transform.localPosition = Vector2.Lerp(_LocalPosition,
                    new Vector2(mData.mX, -mData.mY),
                    (Time.time - startTime) / duration);
                yield return null;
            }
            _LocalPosition = new Vector2(mData.mX, -mData.mY);
            isMoving = false;
        
            
        

        if (Blockmng.gamestate == BlockManager.GameState.Create && Blockmng.isAllStop() == true)
            Blockmng.gamestate = BlockManager.GameState.SideMatch;
        
    }


    public void DeleteAnimation()
    {
        StartCoroutine(DeleteCoroutine());
    }
    IEnumerator DeleteCoroutine()
    {
        float startTime = Time.time; // 현재시간을 starttime으로
        Vector2 orgVec = transform.localScale; // orgvec을 현재 스케일

        //==========================================================================
        while (Time.time - startTime <= 0.2f) // 함수호출하고 0.3초가되기전까지
        {
            transform.localScale = Vector2.Lerp(orgVec, new Vector2(0,0), (Time.time - startTime) / 0.2f);
            yield return null;
        }
        //============================================================================

        transform.localScale = new Vector2(0, 0);
        isMatched = true;
        
        if (Blockmng.gamestate != BlockManager.GameState.Create)
        {
            Blockmng.gamestate = BlockManager.GameState.Create;
            Debug.Log("toCreate");
        }
    }

    public Vector2 _LocalPosition
    {
        get { return transform.localPosition; }
        set { transform.localPosition = value; }
    }
    public Vector2 _LocalScale
    {
        get { return transform.localScale; }
        set { transform.localScale = value; }
    }
}
