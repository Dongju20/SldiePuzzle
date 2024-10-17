using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject board;

    private Board boardScript;
    public void Start()
    {
        boardScript = board.GetComponent<Board>();
        Debug.Log(boardScript);
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
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tile")))
        {
            float rayLength = 100.0f;

            // 레이를 그리기 (초록색)
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.green, 2.0f);

            Debug.Log("hit Something...");
            boardScript.MoveTile(hit.collider.gameObject);
        }
    }
}
