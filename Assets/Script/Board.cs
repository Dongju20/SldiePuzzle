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
    //�ʱ�ȭ ��ġ...
    //���⼱ Ÿ���� �߽���ǥ
    [SerializeField]
    private Transform postion;
    
    //Ÿ�� ������Ʈ
    [SerializeField]
    private GameObject TilePrefab;

    [SerializeField]
    private int size;

    //Ÿ�� �� �Ÿ�
    private float distance;

    //���� ����
    private float boardLength;
    //�� Ÿ���� ��ġ
    public GameObject EmptyTile { set; get; }

    private List<Tile> tileList = new List<Tile>();                                // ������ Ÿ�� ���� ����
   
    // Start is called before the first frame update
    void Start()
    {
        //���� ���̸� �����ɴϴ�...
        boardLength = transform.localScale.x;
        //Ÿ�� �� �Ÿ� ���ϱ�...
        distance = boardLength / size;

        //Ÿ�� �����Լ�....
        Initialized();
        tileList.ForEach(tile => tile.SetCorrectPosition());
        StartCoroutine("Shuffle");
    }

    // Update is called once per frame


    //z�� ��� x�������� ����
    private void Initialized()
    {
        float xStart = - boardLength / 2;
        float yStart =  boardLength / 2;
        //y��° Ÿ��
        for (int y = 0; y < size; y++)
        {
            for(int x = 0; x < size; x++)
            {
                //Ÿ�� ���� ��ġ ����...
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
                //Ÿ���� �ùٸ� ��ġ�� ����..
                tileList.Add(tile);
            }
        }
    }

    //�����̴� �Լ��� ȣ���մϴ�...
    public void MoveTile(GameObject gameObject)
    {
        Vector3 EmptyTilePosition = EmptyTile.transform.localPosition;
        Debug.Log("�� Ÿ�� ��ġ" + EmptyTilePosition + "Ŭ���� Ÿ�� ��ġ" + gameObject.transform.localPosition);
        Debug.Log("���� Ÿ�ϰ� �Ÿ�..." + Vector3.Distance(EmptyTilePosition, gameObject.transform.localPosition) + "�䱸�Ǵ� Ÿ�ϰ� �Ÿ�" + distance);
        //���� ��ĭ���� �Ÿ��� Ÿ���� �Ÿ��� ���ٸ�...
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
            Debug.Log("�̵��Ұ�...");
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
        //����Ʈ �ʱ�ȭ
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
        //Ǯ�̰� �������� Ȯ���� �ϴ� �Լ� ȣ��...
        CheckSolveAble();
    }

    public void CheckSolveAble()
    {
        int boardSize = this.size * this.size;
        int []arr = new int[boardSize];

        Transform[] transforms = this.GetComponentsInChildren<Transform>().Where(t => t != this.transform)
                             .ToArray(); ;
        for (int y = 0; y < this.size; y++)
        {
            for (int x = 0; x < this.size; x++)
            {
                Tile tile = tileList.Find(tile =>  tile.GetCorrectPosition() == transforms[y * this.size + x].transform.localPosition);
                arr[tile.Numeric - 1] = y * this.size + x + 1;
                Debug.Log((tile.Numeric) + " " + (y * this.size + x + 1));
            }
        }


        //��ĭ �ε��� ��ġ..
        int blankSpace = -1;
        //����ִ� ��ĭ�� ���� ��ġ
        int blankSpaceRow = -1;

        //�ι��� ī��Ʈ
        int cnt = 0;
        //�迭 ũ��
        int size = arr.Length;
        for(int i  = 0; i < size; i++)
        {
            Debug.Log(arr[i]);
        }
        for(int i = 0; i < size; i++)
        {
            //���� ��ĭ�̶��
            if (arr[i] == size)
            {
                //��ĭ ��ġ�� �������� Ȧ�� �� Ȥ�� ¦�� ������ ����..
                blankSpace = i;
                //��ĭ ���� ���ϴ� ����
                //�� ũ��
                int rowCnt = (int)Mathf.Sqrt(size);
                int rowPosition = i / rowCnt;
                blankSpaceRow = rowCnt - 1 - rowPosition;
            }
            for(int j = i + 1; j < size; j++) {
                if (j != blankSpace)
                {
                    if (arr[i] > arr[j])
                    {
                        cnt++;
                    }
                }
            }
        }
        Debug.Log(cnt);
        //check the Puzzle is SolveAble
        //N�� Ȧ���� ���
        int n = (int)Mathf.Sqrt(size) % 2;
        if (n!= 0)
        {
            //Ȧ�����...
            if (cnt % 2 == 0)
            {
                Debug.Log("�������� Ǯ�̰� �����մϴ�..");
            }
            else
            {
                Debug.Log("�������� Ǯ�̰� �Ұ����մϴ�.. �ٽ� �����ϴ�.");
                StartCoroutine("Shuffle");
            }
        }
    }
}
