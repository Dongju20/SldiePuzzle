using UnityEngine;

namespace TictactoeTictactoe.SlidingPuzzle.Runtime.PuzzleButton
{
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
            PuzzleManager.GetInstance().SetIsGamePlay(false);
            puzzleObject.SetActive(false);
        }
    }   
}