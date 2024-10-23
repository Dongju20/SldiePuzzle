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

    //섞을 횟수
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

            // 레이를 그리기 (초록색)
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red, 10.0f);

            Debug.Log("hit Something...");
            boardScript.MoveTile(hit.collider.gameObject);
        }

        return hit;
    }

    public void DoShuffle()
    {
        //예전에 섞었던 값
        int previous = 5;
        for(int i  = 0; i < cnt; i++)
        {
            Debug.Log(i + "번째 실행..");
            //현재 방향
            int current = Shuffle(previous);
        }
    }


    public int Shuffle(int previos)
    {
        int rand = Random.Range(0 , 4);
        //섞는 방향 지정..
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
        Debug.Log("레이캐스팅 방향" + direction);
        direction = direction.normalized;
        ray.direction = direction;
        shootRay(ray);
        return rand;
    }
}
