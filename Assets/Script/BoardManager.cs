using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] board;

    private Board[] boardScript;

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
            Board boardScript = hit.collider.GetComponentInParent<Board>();
            boardScript.MoveTile(hit.collider.gameObject);
        }

        return hit;
    }

 

}
