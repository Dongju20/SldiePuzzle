using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board : MonoBehaviour
{
    private BoardList boardList;
    public void SetBoardList(BoardList boardList) {this.boardList = boardList;}
    //초기화 위치...
    //여기선 타일의 중심좌표
    private Transform postion;
    
    //타일 오브젝트
    [SerializeField]
    private GameObject TilePrefab;

    [SerializeField, Range(2, 8)]
    private int size;

   // 섞이는 횟수.
    [SerializeField, Range(1, 100)]
    private int shuffleCount;

    //타일 간 거리
    private float distance;

    //보드 길이
    private float boardLength;
    //빈 타일의 위치
    public GameObject EmptyTile { set; get; }

    private List<Tile> tileList = new List<Tile>();                                // 생성한 타일 정보 저장

    //게임 끝났는지 여부를 확인하는 함수
    private bool isGameOver = false;

    //게임이 한번활성화되었는가 안되었는가 확인
    private bool isSecondTimeActive = false;

    //풀이 가능한 퍼즐인지 확인하는 여부
    // private bool isSolvable = false;

    private bool isShuffling = false;   // 현재 이 퍼즐이 섞이는 중인지.

    private PuzzleManager puzzleManager;

    // Start is called before the first frame update
    void Start()
    {
        puzzleManager = PuzzleManager.GetInstance();
        postion = this.transform;
        //보드 길이를 가져와 초기화
        boardLength = transform.localScale.x;
        //타일 간 거리 구하기... --> 나중에 타일을 옮길 때 쓰이는 변수
        distance = boardLength / size;
        //타일 생성함수....
        Initialized();
        //타일 정보를 담는 리스트 배열
        tileList.ForEach(tile => tile.SetCorrectPosition());
    }

    //z는 양수 x음수부터 시작
    private void Initialized()
    {
        //시작 위치 x, y를 설정
        float xStart = -boardLength / 2 + distance / 2;
        float yStart = boardLength / 2 - distance / 2;
        Debug.Log(xStart);
        
        int xCount = size;
        int yCount = size;
        
        ImageController boardImageController = this.GetComponent<ImageController>();

        //y번째 --> 행
        for (int y = 0; y < yCount; y++)
        {
            //x번째 --> 열
            for(int x = 0; x < xCount; x++)
            {
                //타일 오브젝트를 생성..
                GameObject clone = Instantiate(TilePrefab, postion);
                //vector3는 생성한 타일의 위치를 나타냄..
                Vector3 vector3 = new Vector3(xStart + x * distance, yStart - y * distance);
                //타일의 x, y크기를 지정 --> 정사각형 모양이다.
                float scaleX = boardLength / xCount;
                float scaleY = boardLength / yCount;
                //생성한 타일 위치 지정 // SetLocalPositionAndRotation() 함수 미지원 버전 위한 코드 수정.
                clone.transform.localPosition = vector3;
                clone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                //생성한 타일 크기 조절
                clone.transform.localScale = new Vector3(scaleX, scaleY, 1);
                Tile tile = clone.GetComponent<Tile>();
                ImageController tileImageController = tile.GetComponent<ImageController>();
                tileImageController.texture = boardImageController.texture;
                tileImageController.renderType = boardImageController.renderType;
                tileImageController.alpha = boardImageController.alpha;
                //타일의 셋업 함수를 부른다음
                tile.SetUp(this, y * yCount + x + 1, xCount * yCount, x, y);
                //타일의 정보를 저장
                tileList.Add(tile);
            }
        }
    }

    //움직이는 함수를 호출합니다...
    public void MoveTile(GameObject gameObject)
    {
        if (puzzleManager.GetIsShuffle())
            return;
        //빈 타일의 위치를 가져옵니다...
        Vector3 EmptyTilePosition = EmptyTile.transform.localPosition;
        //만약 움직이고자 하는 타일과 빈칸 타일의 거리가 타일의 거리와 같다면... --> 즉 움직일 수 있는 타일
        if(Vector3.Distance(EmptyTilePosition, gameObject.transform.localPosition) == distance)
        {
            //두개의 위치를 바꿔주고
            //빈 타일의 위치와 정보를 갱신합니다.
            Vector3 goalPostion = EmptyTilePosition;
            Vector3 moveEmptyTilePosition = gameObject.transform.localPosition; 
            EmptyTilePosition = gameObject.GetComponent<Transform>().localPosition;
            gameObject.GetComponent<Tile>().Move(goalPostion);
            EmptyTile.GetComponent<Tile>().Move(moveEmptyTilePosition);
        }
        else
        {
            Debug.Log("이동불가...");
        }
    }
    public void IsGameOver()
    {
        //타일 리스트에서 현재 위치가 알맞은 타일들을 가져옴.. 모든 타일이 같은 위치에 있으면 tiles.Count는 size * size - 1이 됨
        List<Tile> tiles = tileList.FindAll(x => x.IsCorrected == true);
        //만약 모든 타일이 같은 위치에 있다면...
        if (tiles.Count == size * size - 1)
        {
            Debug.Log("GameClear");
            //isGameOver를 트루로 바꿈
            isGameOver = true;
        }
        else
        {
            //isGameOver는 false임
            isGameOver = false;
        }

        if (isGameOver)
        {
            boardList.ClearPuzzle(this.gameObject);
        }
    }

    //타일 간 거리를 리턴하는 함수
    public float GetDistance()
    {
        return distance;
    }

    public void Shuffle() {
        if (isShuffling == false)
        {
            StartCoroutine(ShuffleCoroutine());
        }
    }
    
    //타일을 섞는 함수
    private IEnumerator ShuffleCoroutine()
    {
        isShuffling = true;
        puzzleManager.SetIsShuffle(true);
        
        int xCount = size;
        int yCount = size;

        do {
            shuffleCount = 5;
            int currentCount = 0;
            // 리스트 초기화.
            while (currentCount < shuffleCount)
            {
                currentCount++;

                // tileList에서 index1, index2번째의 위치를 랜덤으로 가져오고, (단, index1 != index2)
                int index1 = Random.Range(0, xCount * yCount);
                int index2;// = Random.Range(0, xCount * yCount);
                while (true)
                {
                    index2 = Random.Range(0, xCount * yCount);

                    if (index1 != index2) break;
                }

                //tileList에서 index1번째 와 index2번째의 위치를 서로 서서히 바꿔줌.
                Vector3 tile1Pos = tileList[index1].transform.localPosition;
                Vector3 tile2Pos = tileList[index2].transform.localPosition;
                float current = 0;
                float percent = 0;
                float moveTime = 0.2f;
                while(percent < 1)
                {
                    current += Time.deltaTime;
                    percent = current / moveTime;
                    tileList[index2].transform.localPosition = Vector3.Lerp(tile2Pos, tile1Pos, percent);
                    tileList[index1].transform.localPosition = Vector3.Lerp(tile1Pos, tile2Pos, percent);
                    yield return null;
                }

                yield return null;
            }
        } while(CheckSolvable() == false);

        isShuffling = false;
        puzzleManager.SetIsShuffle(false);
        yield break;
    }

    public bool CheckSolvable()
    {
        int boardSize = size * size;
        int[] tileArr = new int[boardSize];
        //N X N의 배열에서 빈칸을 제외한 타일들의 위치를 1차원 배열로 바꿈
        //이때 보드의 자식 객체만 타일이므로 타일 정보만 가져오기 위해 자기 자신은 제외
        //안그러면 보드의 위치 정보도 포함이 되어 N X N + 1이 됨
        Transform[] tileTransforms = this.GetComponentsInChildren<Transform>().Where(t => t != this.transform).ToArray();
        
        for (int y = 0; y < this.size; y++)
        {
            for (int x = 0; x < this.size; x++)
            {
                //N x N 타일의 순서를 1차원 배열로 표현.. // tile 변수 중복에 의해 tile -> t 로 변경.
                Tile tile = tileList.Find(t => t.GetCorrectPosition() == tileTransforms[y * this.size + x].transform.localPosition);
                tileArr[tile.Numeric - 1] = y * this.size + x + 1;
            }
        }
        
        //빈칸을 제외한 타일의 위치 정보를 나타냄..
        int[] tileArrExcludeVoid = tileArr.Where(val => val != boardSize).ToArray();

        //빈칸 인덱스 위치..
        int blankSpace = -1;

        //빈칸 위치 찾기..
        for(int i = 0; i < boardSize; i++)
        {
            if(boardSize == tileArr[i])
            {
                blankSpace = i;
                break;
            }
        }

        //인버젼 카운트 초기값은 0이다.
        int inversionCount = 0;
        //배열 크기
        
        //빈칸 행을 구하는 로직
        //행 크기
        int rowCount = size;
        //빈칸이 아래에서 홀수번째 혹은 짝수번째에 위치해 있는가를 계산하는 로직입니다.
        int rowPosition = blankSpace / rowCount;
        //비어있는 빈칸의 행의 위치
        //이거는 N X N에서 N이 짝수 일때 빈칸이 아래에서 홀수 번째 혹은 짝수 번째에 있을 때 확인하기 위한 변수
        int blankSpaceRow = rowCount - rowPosition;

        //inversion을 세는 for문
        for (int i = 0; i < boardSize - 1; i++)
        {
            for (int j = i + 1; j < boardSize - 1; j++)
            {
                if (tileArrExcludeVoid[i] > tileArrExcludeVoid[j])
                {
                    inversionCount++;
                }
            }
        }
        Debug.Log("인버전 카운트" + inversionCount);


        //check the Puzzle is Solvable.
        bool isSolvable;
        if (rowCount % 2 != 0)
        {
            // rowCount 가 홀수인 경우,
            // inversion 이 짝수이면 풀이 가능.
            if (inversionCount % 2 == 0)
            {
                Debug.Log("이퍼즐은 풀이가 가능합니다..");
                isSolvable = true;
            }
            else
            {
                Debug.Log("이퍼즐은 풀이가 불가능합니다.. 다시 섞습니다.");
                isSolvable = false;
                // StartCoroutine(ShuffleCoroutine());
            }
        }
        else
        {
            // rowCount 가 짝수인 경우,
            if (blankSpaceRow % 2 == 0)
            {
                // 빈 타일의 행이 아래서부터 짝수이면,
                // inversion 이 홀수이면 풀이 가능.
                if (inversionCount % 2 != 0)
                {
                    Debug.Log("이퍼즐은 풀이가 가능합니다..");
                    isSolvable = true;
                }
                else
                {
                    Debug.Log("이퍼즐은 풀이가 불가능합니다.. 다시 섞습니다.");
                    isSolvable = false;
                    // StartCoroutine(ShuffleCoroutine());
                }
            }
            else
            {
                // 빈 타일의 행이 아래서부터 홀수이면,
                // inversion 이 짝수이면 풀이 가능.
                if (inversionCount % 2 == 0)
                {
                    Debug.Log("이퍼즐은 풀이가 가능합니다..");
                    isSolvable = true;
                } 
                else
                {
                    Debug.Log("이퍼즐은 풀이가 불가능합니다.. 다시 섞습니다.");
                    isSolvable = false;
                    // StartCoroutine(ShuffleCoroutine());
                }
            }
        }

        return isSolvable;
    }

    //게임 오브젝트 활성화/비활성화 함수
    public void SetActiveBoard(){
        //섞는 함수를 실행합니다...
        if (!isSecondTimeActive){
            isSecondTimeActive = true;
            puzzleManager.SetIsShuffle(true);
            StartCoroutine(StartShuffle());
        }
    }

    public IEnumerator StartShuffle(){
        puzzleManager.SetIsShuffle(true);
        yield return new WaitForSeconds(1.5f);
        Shuffle();
    }

    public bool GetIsGameOver()
    {
        return isGameOver;
    }
}
