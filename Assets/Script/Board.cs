using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    //�ʱ�ȭ ��ġ...
    //���⼱ Ÿ���� �߽���ǥ
    [SerializeField]
    private Transform postion;
    
    //Ÿ�� ������Ʈ
    [SerializeField]
    private GameObject TilePrefab;

    [SerializeField]
    private int size;

    private List<Tile> tiles;

    //Ÿ�� �� �Ÿ�
    private float distance;

    //���� ����
    private float boardLength;
    //�� Ÿ���� ��ġ
    public Vector3 EmptyTilePosition { set; get; }
    // Start is called before the first frame update
    void Start()
    {
        //���� ���̸� �����ɴϴ�...
        boardLength = transform.localScale.x;
        //Ÿ�� �� �Ÿ� ���ϱ�...
        distance = boardLength / size;

        //Ÿ�� �����Լ�....
        Initialized();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
                Tile tile = clone.GetComponent<Tile>();
                Debug.Log(y * size + x + 1);
                tile.SetUp( this ,(y * size  + x + 1), size * size);
            }
        }
    }

    //�����̴� �Լ��� ȣ���մϴ�...
    public void MoveTile(GameObject gameObject)
    {
        Debug.Log("�� Ÿ�� ��ġ" + EmptyTilePosition + "Ŭ���� Ÿ�� ��ġ" + gameObject.transform.localPosition);
        Debug.Log("���� Ÿ�ϰ� �Ÿ�..." + Vector3.Distance(EmptyTilePosition, gameObject.transform.localPosition) + "�䱸�Ǵ� Ÿ�ϰ� �Ÿ�" + distance);
        //���� ��ĭ���� �Ÿ��� Ÿ���� �Ÿ��� ���ٸ�...
        if(Vector3.Distance(EmptyTilePosition, gameObject.transform.localPosition) == distance)
        {
            Vector3 goalPostion = EmptyTilePosition;
            EmptyTilePosition = gameObject.GetComponent<Transform>().localPosition;
            gameObject.GetComponent<Tile>().Move(goalPostion);
        }
        else
        {
            Debug.Log("�̵��Ұ�...");
        }
    }
}
