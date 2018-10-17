using UnityEngine;
using System.Collections;

public class CMatchData : MonoBehaviour {

    public enum Direction
    {
        LEFT, RIGHT, UP, DOWN
    }

    public int mX;
    public int mY;
    public int mLength;
    public Direction mDir;

    public CMatchData(Direction dir, int length, int x, int y)
    {
        this.mDir = dir;
        this.mLength = length;
        this.mX = x;
        this.mY = y;
    }
}
