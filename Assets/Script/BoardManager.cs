using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject board;

    private Board boardScript;

    //���� Ƚ��
    [SerializeField] private int cnt;
    public void Start()
    {
        boardScript = board.GetComponent<Board>();
    }
    void Update()
    {
        updateInput();
    }

    private void updateInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    public void Click()
    {
        Debug.Log("Mouse down...");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        hit = shootRay(ray);
    }

    private RaycastHit shootRay(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tile")))
        {
            float rayLength = 100.0f;

            // ���̸� �׸��� (�ʷϻ�)
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red, 10.0f);

            Debug.Log("hit Something...");
            boardScript.MoveTile(hit.collider.gameObject);
        }

        return hit;
    }

    public void DoShuffle()
    {
        //������ ������ ��
        int previous = 5;
        for(int i  = 0; i < cnt; i++)
        {
            Debug.Log(i + "��° ����..");
            //���� ����
            int current = Shuffle(previous);
        }
    }


    public int Shuffle(int previos)
    {
        int rand = Random.Range(0 , 4);
        //���� ���� ����..
        float distance = boardScript.GetDistance();
        Vector3 point = Vector3.zero;
        switch (rand)
        {
            case 0:
                point = new Vector3(distance, 0, 0);
                break;
            case 1:
                point = new Vector3(-distance, 0, 0);
                break;
                case 2:
                point = new Vector3(0, 0, distance);
                break; 
            case 3:
                point = new Vector3(0,0,-distance);
                break;
        }

        Ray ray = new Ray();
        Vector3 direction = boardScript.EmptyTile.transform.position - (this.transform.position - point);
        ray.origin = this.transform.position;
        Debug.Log("����ĳ���� ����" + direction);
        direction = direction.normalized;
        ray.direction = direction;
        shootRay(ray);
        return rand;
    }
}
