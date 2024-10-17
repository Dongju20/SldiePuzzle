using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //초기화 위치...
    //여기선 타일의 중심좌표
    [SerializeField]
    private Transform postion;
    
    //타일 오브젝트
    [SerializeField]
    private GameObject TilePrefab;

    [SerializeField]
    private int size;

    private List<Tile> tiles;

    //타일 간 거리
    private float distance;

    //보드 길이
    private float boardLength;
    //빈 타일의 위치
    public Vector3 EmptyTilePosition { set; get; }
    // Start is called before the first frame update
    void Start()
    {
        //보드 길이를 가져옵니다...
        boardLength = transform.localScale.x;
        //타일 간 거리 구하기...
        distance = boardLength / size;

        //타일 생성함수....
        Initialized();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
                Tile tile = clone.GetComponent<Tile>();
                Debug.Log(y * size + x + 1);
                tile.SetUp( this ,(y * size  + x + 1), size * size);
            }
        }
    }

    //움직이는 함수를 호출합니다...
    public void MoveTile(GameObject gameObject)
    {
        Debug.Log("빈 타일 위치" + EmptyTilePosition + "클릭된 타일 위치" + gameObject.transform.localPosition);
        Debug.Log("실제 타일간 거리..." + Vector3.Distance(EmptyTilePosition, gameObject.transform.localPosition) + "요구되는 타일간 거리" + distance);
        //만약 빈칸과의 거리가 타일의 거리와 같다면...
        if(Vector3.Distance(EmptyTilePosition, gameObject.transform.localPosition) == distance)
        {
            Vector3 goalPostion = EmptyTilePosition;
            EmptyTilePosition = gameObject.GetComponent<Transform>().localPosition;
            gameObject.GetComponent<Tile>().Move(goalPostion);
        }
        else
        {
            Debug.Log("이동불가...");
        }
    }
}
