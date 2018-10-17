using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CBlockData 
{

    [SerializeField]
    // Use this for initialization
    public int mX;
    [SerializeField]
    public int mY;

    public CBlockData(int pX, int pY)
    {
        this.mX = pX;
        this.mY = pY;
    }
}
