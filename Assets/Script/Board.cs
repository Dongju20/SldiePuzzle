using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Board : MonoBehaviour
{
    class TileInfo
    {
        int index;
    }
    //초기화 위치...
    //여기선 타일의 중심좌표
    private Transform postion;
    
    //타일 오브젝트
    [SerializeField]
    private GameObject TilePrefab;

    [SerializeField]
    [Range(2, 8)]
    private int size;

    //타일 간 거리
    private float distance;

    //보드 길이
    private float boardLength;
    //빈 타일의 위치
    public GameObject EmptyTile { set; get; }

    private List<Tile> tileList = new List<Tile>();                                // 생성한 타일 정보 저장

    //게임 끝났는지 여부를 확인하는 함수
    private bool isGameOver = false;
    
    // Start is called before the first frame update
    void Start()
    {
        postion = this.transform;
        //보드 길이를 가져와 초기화
        boardLength = transform.localScale.x;
        //타일 간 거리 구하기... --> 나중에 타일을 옮길 때 쓰이는 변수
        distance = boardLength / size;
        //타일 생성함수....
        Initialized();
        //타일 정보를 담는 리스트 배열
        tileList.ForEach(tile => tile.SetCorrectPosition());
        //섞는 함수 실행...
    }

    //z는 양수 x음수부터 시작
    private void Initialized()
    {
        //시작 위치 x, y를 설정
        float xStart = -boardLength / 2 + distance / 2;
        float yStart = boardLength / 2 - distance / 2;
        Debug.Log(xStart);
        
        //y번째 --> 행
        for (int y = 0; y < size; y++)
        {
            //x번째 --> 열
            for(int x = 0; x < size; x++)
            {
                //타일 오브젝트를 생성..
                GameObject clone = Instantiate(TilePrefab, postion);
                //vector3는 생성한 타일의 위치를 나타냄..
                Vector3 vector3 = new Vector3();
                vector3.x = xStart + x * distance;
                vector3.y = yStart - y * distance;
                //타일의 x, y크기를 지정 --> 정사각형 모양이다.
                float scaleX = boardLength / size;
                float scaleY = boardLength / size;
                //생성한 타일 위치 지정
                clone.transform.SetLocalPositionAndRotation(vector3, Quaternion.Euler(Vector3.zero));
                //생성한 타일 크기 조절
                clone.transform.localScale = new Vector3(scaleX, scaleY, 1);
                Tile tile = clone.GetComponent<Tile>();
                tile.GetComponent<ImageController>().texture = this.GetComponent<ImageController>().texture;
                tile.GetComponent<ImageController>().renderType = this.GetComponent<ImageController>().renderType;
                tile.GetComponent<ImageController>().alpha = this.GetComponent<ImageController>().alpha;
                //타일의 셋업 함수를 부른다음
                tile.SetUp(this, (y * size + x + 1), size * size, x, y);
                //타일의 정보를 저장
                tileList.Add(tile);
            }
        }
    }

    //움직이는 함수를 호출합니다...
    public void MoveTile(GameObject gameObject)
    {
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
    }

    //타일 간 거리를 리턴하는 함수
    public float GetDistance()
    {
        return distance;
    }

    //타일을 섞는 함수
    public IEnumerator Shuffle()
    {
        float current = 0;
        float percent = 0;
        float time = 0.3f * size;
        
        //리스트 초기화
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            //tileList에서 index1번째의 위치를 가져오고
            int index1 = Random.Range(0, size * size);
            Vector3 first = tileList[index1].transform.localPosition;
            int index2 = -1;
            do
            {
                //tileList에서 index2번째의 위치를 가져옴 이때 index1 != index2가 되기 위해 while문을 사용함
                index2 = Random.Range(0, size * size);
            } while(index2 == index1);
            Vector3 second = tileList[index2].transform.localPosition;
            //tileList에서 index1번째 와 index2번째의 위치를 서로 바꿔줌 
            tileList[index2].transform.localPosition = first;
            tileList[index1].transform.localPosition = second;
            yield return null;
        }
        //풀이가 가능한지 확인을 하는 함수 호출...
        CheckSolveAble();
    }

    public void CheckSolveAble()
    {
        int boardSize = this.size * this.size;
        int[] arr = new int[boardSize];
        //N X N의 배열에서 빈칸을 제외한 타일들의 위치를 1차원 배열로 바꿈
        //이때 보드의 자식 객체만 타일이므로 타일 정보만 가져오기 위해 자기 자신은 제외
        //안그러면 보드의 위치 정보도 포함이 되어 N X N + 1이 됨
        Transform[] transforms = this.GetComponentsInChildren<Transform>().Where(t => t != this.transform)
                             .ToArray();

        int[] newArr = new int[boardSize - 1];

        for (int y = 0; y < this.size; y++)
        {
            for (int x = 0; x < this.size; x++)
            {
                //N x N 타일의 순서를 1차원 배열로 표현..
                Tile tile = tileList.Find(tile => tile.GetCorrectPosition() == transforms[y * this.size + x].transform.localPosition);
                arr[tile.Numeric - 1] = y * this.size + x + 1;
            }
        }

        //빈칸을 제외한 타일의 위치 정보를 나타냄..
        newArr = arr.Where(val => val != boardSize).ToArray();

        //빈칸 인덱스 위치..
        int blankSpace = -1;
        //비어있는 빈칸의 행의 위치
        //이거는 N X N에서 N이 짝수 일때 빈칸이 아래에서 홀수 번째 혹은 짝수 번째에 있을 때 확인하기 위한 변수
        int blankSpaceRow = -1;

        //빈칸 위치 찾기..
        for(int i = 0; i < boardSize; i++)
        {
            if(boardSize == arr[i])
            {
                blankSpace = i;
                break;
            }
        }

        //인버젼 카운트 초기값은 0이다.
        int cnt = 0;
        //배열 크기
        int size = arr.Length;
        //빈칸 행을 구하는 로직
        //행 크기
        int rowCnt = (int)Mathf.Sqrt(size);
        //빈칸이 아래에서 홀수번째 혹은 짝수번째에 위치해 있는가를 계산하는 로직입니다.
        int rowPosition = blankSpace / rowCnt;
        blankSpaceRow = rowCnt - rowPosition;

        //inversion을 세는 for문
        for (int i = 0; i < boardSize - 1; i++)
        {
            for (int j = i + 1; j < boardSize - 1; j++)
            {
                if (newArr[i] > newArr[j])
                {
                    cnt++;
                }
            }
        }
        //check the Puzzle is SolveAble
        //N이 홀수인 경우
        //inversion이 무조건 짝수이면 풀이가 가능함
        int n = (int)Mathf.Sqrt(size) % 2;
        if (n != 0)
        {
            //홀수라면...
            if (cnt % 2 == 0)
            {
                Debug.Log("이퍼즐은 풀이가 가능합니다..");
            }
            else
            {
                Debug.Log("이퍼즐은 풀이가 불가능합니다.. 다시 섞습니다.");
                //풀이가 불가능함으로 다시 섞기
                StartCoroutine(Shuffle());
            }
        }
        //N X N에서 N이 짝수인 경우
        else
        {
            //빈칸이 아래서부터 짝수번째에 있을 때
            //inversion이 짝수이면 풀이 가능
            if (blankSpaceRow % 2 == 0)
            {
                //홀수라면
                if (cnt % 2 != 0)
                {
                    Debug.Log("이퍼즐은 풀이가 가능합니다..");
                }
                else
                {
                    //다시 섞습니다.
                    Debug.Log("이퍼즐은 풀이가 불가능합니다.. 다시 섞습니다.");
                    StartCoroutine(Shuffle());
                }
            }
            //빈칸이 아래서부터 홀수번째에 있을 때
            else
            {
                //짝수라면
                if (cnt % 2 == 0)
                {
                    Debug.Log("이퍼즐은 풀이가 가능합니다..");
                }
                else
                {
                    //다시 섞는 함수를 호출
                    Debug.Log("이퍼즐은 풀이가 불가능합니다.. 다시 섞습니다.");
                    StartCoroutine(Shuffle());
                }
            }
        }
    }
}
