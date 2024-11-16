using UnityEngine;

namespace TictactoeTictactoe.SlidingPuzzle.Runtime.PuzzleButton
{
    // 에셋에서 클릭 가능한 버튼 오브젝트 추상 클래스.
    public abstract class ObjectButton : MonoBehaviour, IClickableObj
    {
        public void ClickObj()
        {
            ClickButton();
        }

        public abstract void ClickButton(); // 버튼 클릭 시 조작 메소드.
    }
}