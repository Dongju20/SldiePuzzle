using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager
{
    private static PuzzleManager puzzleManager = null;
    //현재 게임이 플레이 중인지 확인하는 함수..
    private bool isGamePlay = false;

    //현재 퍼즐이 섞이고 있는지 여부 확인하는 함수..
    private bool isShuffle = false;
    
    public static PuzzleManager GetInstance()
    {
        if (puzzleManager == null)
            puzzleManager = new PuzzleManager();
        return puzzleManager;
    }

    public void SetIsGamePlay(bool isGamePlay)
    {
        this.isGamePlay = isGamePlay;
    }

    public void SetIsShuffle(bool isShuffle)
    {
        this.isShuffle = isShuffle;
    }

    public bool GetIsGamePlay()
    {
        return isGamePlay;
    }

    public bool GetIsShuffle()
    {
        return isShuffle;
    }
}
