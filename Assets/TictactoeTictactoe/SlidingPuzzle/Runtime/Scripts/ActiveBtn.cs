using UnityEngine;

namespace TictactoeTictactoe.SlidingPuzzle.Runtime
{
    public class ActiveBtn : MonoBehaviour
    {
        //보드리스트 정보가져오기..
        private BoardList boardList;
        //보드 오브젝트
        private GameObject board;
        
        //true면 오브젝트 활성화 false면 비활성화하는 함수...
        public void DoActive(bool isOn = true){
            boardList.ActiveBoard(this.gameObject, isOn);
        }

        public void SetBoardList(BoardList boardList){
            this.boardList = boardList;
        }
    }
}