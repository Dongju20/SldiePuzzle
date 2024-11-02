using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에셋에서 클릭 가능한 나가기 버튼 오브젝트 클래스.
public class ObjectExitButton : ObjectButton
{
    public override void ClickButton()
    {
        Exit();
    }

    public void Exit() {
        // TODO gameObject 대신, 슬라이딩 퍼즐 보드 오브젝트를 가져올 수 있어야 함.
        gameObject.SetActive(false);
    }
}
