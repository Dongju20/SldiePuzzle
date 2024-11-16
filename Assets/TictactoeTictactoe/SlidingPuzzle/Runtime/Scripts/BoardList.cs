using UnityEngine;
using UnityEngine.Events;

namespace TictactoeTictactoe.SlidingPuzzle.Runtime
{
    public class BoardList : MonoBehaviour
    {
        [SerializeField]
        private BoardAndTile[] boards;

        private PuzzleManager puzzleManager;

        [System.Serializable]
        public struct BoardAndTile
        {
            [SerializeField]
            private GameObject board;
            [SerializeField]
            private GameObject button;
            [SerializeField]
            private UnityEvent targetEvent;

            // button과 board에 접근할 수 있는 읽기 전용 프로퍼티 추가
            public GameObject Button => button;
            public GameObject Board => board;

            // targetEvent 이벤트 실행 메소드.
            public void RunTargetEvent() { targetEvent?.Invoke(); }
        }


        //특정 버튼을 누르면 활성화가 가능하게 하는 함수..
        public void ActiveBoard(GameObject btn, bool onOff = true)
        {
            //타일 배열을 순회하면서 어떤 타일의 어떤 버튼인지 확인하기...

            for (int i = 0; boards.Length > i; i++)
            {
                Board board = boards[i].Board.GetComponentInChildren<Board>();
                if (puzzleManager.GetIsShuffle())
                {
                    return;
                }
                
                boards[i].Board.SetActive(false);
                if (boards[i].Button == btn)
                {
                    Debug.Log("섞습니다..");
                    boards[i].Board.SetActive(onOff);
                    Debug.Log(board);
                    board.SetActiveBoard();
                }
            }
        }

        // 퍼즐 클리어 시 해당 퍼즐에 연결된 타겟이벤트 실행하는 함수.
        public void ClearPuzzle(GameObject board)
        {
            for (int i=0; boards.Length > i; i++)
            {
                if (boards[i].Board.GetComponentInChildren<Board>().gameObject == board)
                {
                    boards[i].RunTargetEvent();         // 연결된 이벤트 실행.
                    boards[i].Board.SetActive(false);   // 퍼즐 보드 오브젝트 비활성화. 
                    break;
                }
            }
        }

        public void Start()
        {
            for (int i = 0; boards.Length > i; i++)
            {
                boards[i].Board.SetActive(false);
                boards[i].Button.GetComponent<ActiveBtn>().SetBoardList(this);
                boards[i].Board.GetComponentInChildren<Board>().SetBoardList(this);
            }
            puzzleManager = PuzzleManager.GetInstance();
            Debug.Log("퍼즐 매니저 " + puzzleManager);
        }

    }   
}