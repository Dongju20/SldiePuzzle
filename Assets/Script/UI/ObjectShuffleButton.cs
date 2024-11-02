using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에셋에서 클릭 가능한 섞기 버튼 오브젝트 클래스.
public class ObjectShuffleButton : ObjectButton
{
    public override void ClickButton()
    {
        Shuffle();
    }

    public void Shuffle() {
        Debug.Log("Shuffle");
    }
}
