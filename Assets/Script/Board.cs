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
    [SerializeField]
    private Transform postion;
    
    //타일 오브젝트
    [SerializeField]
    private GameObject TilePrefab;

    [SerializeField]
    private int size;

    //타일 간 거리
    private float distance;

    //보드 길이
    private float boardLength;
    //빈 타일의 위치
    public GameObject EmptyTile { set; get; }

    private List<Tile> tileList = new List<Tile>();                                // 생성한 타일 정보 저장
   
    // Start is called before the first frame update
    void Start()
    {
        //보드 길이를 가져옵니다...
        boardLength = transform.localScale.x;
        //타일 간 거리 구하기...
        distance = boardLength / size;

        //타일 생성함수....
        Initialized();
        tileList.ForEach(tile => tile.SetCorrectPosition());
        StartCoroutine("Shuffle");
    }

    // Update is called once per frame


    //z는 양수 x음수부터 시작
    private void Initialized()
    {
        float xStart = - boardLength / 2;
        float yStart =  boardLength / 2;
        //y번째 타일
        for (int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                //타일 생성 위치 지정...
                GameObject clone = Instantiate(TilePrefab, postion);
                Vector3 vector3 = new Vector3();
                vector3.x = xStart + x * distance;
                vector3.y = yStart - y * distance;
                clone.transform.SetLocalPositionAndRotation(vector3, Quaternion.Euler(Vector3.zero));
                float scale = 1.0f / size;
                clone.transform.localScale = new Vector3(scale, scale, 1);
                Tile tile = clone.GetComponent<Tile>();
                Debug.Log(y * size + x + 1);
                tile.SetUp(this, (y * size + x + 1), size * size, x, y);
                //타일의 올바른 위치를 지정..
                tileList.Add(tile);
            }
        }
    }

    //움직이는 함수를 호출합니다...
    public void MoveTile(GameObject gameObject)
    {
        Vector3 EmptyTilePosition = EmptyTile.transform.localPosition;
        Debug.Log("빈 타일 위치" + EmptyTilePosition + "클릭된 타일 위치" + gameObject.transform.localPosition);
        Debug.Log("실제 타일간 거리..." + Vector3.Distance(EmptyTilePosition, gameObject.transform.localPosition) + "요구되는 타일간 거리" + distance);
        //만약 빈칸과의 거리가 타일의 거리와 같다면...
        if(Vector3.Distance(EmptyTilePosition, gameObject.transform.localPosition) == distance)
        {
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
        List<Tile> tiles = tileList.FindAll(x => x.IsCorrected == true);
        Debug.Log("Correct Count : " + tiles.Count);
        if (tiles.Count == size * size - 1)
        {
            Debug.Log("GameClear");
        }
    }

    public float GetDistance()
    {
        return distance;
    }

    public IEnumerator Shuffle()
    {
        float current = 0;
        float percent = 0;
        float time = 1.5f;
        tileList.ForEach(x => { Debug.Log(x.Numeric); });
        //리스트 초기화
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            int index1 = Random.Range(0, size * size);
            Vector3 first = tileList[index1].transform.localPosition;
            int index2 = -1;
            do
            {
                index2 = Random.Range(0, size * size);
            } while(index2 == index1);
            Vector3 second = tileList[index2].transform.localPosition;
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

        Transform[] transforms = this.GetComponentsInChildren<Transform>().Where(t => t != this.transform)
                             .ToArray();

        int[] newArr = new int[boardSize - 1];

        for (int y = 0; y < this.size; y++)
        {
            for (int x = 0; x < this.size; x++)
            {
                Tile tile = tileList.Find(tile => tile.GetCorrectPosition() == transforms[y * this.size + x].transform.localPosition);
                arr[tile.Numeric - 1] = y * this.size + x + 1;
                Debug.Log((tile.Numeric) + " " + (y * this.size + x + 1));
            }
        }

        newArr = arr.Where(val => val != boardSize).ToArray();

        //빈칸 인덱스 위치..
        int blankSpace = -1;
        //비어있는 빈칸의 행의 위치
        int blankSpaceRow = -1;

        blankSpace = arr.Where(val => val == boardSize).ToArray()[0];

        //인버젼 카운트
        int cnt = 0;
        //배열 크기
        int size = arr.Length;
        //빈칸 행을 구하는 로직
        //행 크기
        int rowCnt = (int)Mathf.Sqrt(size);
        int rowPosition = blankSpace / rowCnt;
        blankSpaceRow = rowCnt - rowPosition;

        for (int i = 0; i < size; i++)
        {
            Debug.Log(arr[i]);
        }
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
        Debug.Log(cnt);
        //check the Puzzle is SolveAble
        //N이 홀수인 경우
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
                StartCoroutine("Shuffle");
            }
        }
        //짝수인 경우
        else
        {
            //빈칸이 아래서부터 짝수번째에 있을 때
            Debug.Log(blankSpaceRow);
            if (blankSpaceRow % 2 == 0)
            {
                //홀수라면
                if (cnt % 2 != 0)
                {
                    Debug.Log("이퍼즐은 풀이가 가능합니다..");
                }
                else
                {
                    StartCoroutine("Shuffle");
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
                    StartCoroutine("Shuffle");
                }
            }
        }
    }
}
