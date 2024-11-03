using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에셋에서 클릭 가능한 나가기 버튼 오브젝트 클래스.
public class ObjectExitButton : ObjectButton
{
    [SerializeField]
    private GameObject puzzleObject;

    public override void ClickButton()
    {
        Exit();
    }

    public void Exit() {
        puzzleObject.SetActive(false);
    }
}
