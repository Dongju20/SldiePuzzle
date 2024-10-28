using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class BoardManager : MonoBehaviour
{
    void Update()
    {
        updateInput();
    }

    private void updateInput()
    {
        //입력을 받을 때마다 Click함수를 호출
        if (Input.GetMouseButtonDown(0))
        {
            Click();
        }
    }

    public void Click()
    {
        //화면에서 클릭한 위치로 레이를 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //shootRay함수를 호출
        hit = shootRay(ray);
    }

    private RaycastHit shootRay(Ray ray)
    {
        RaycastHit hit;
        //raycast에서 맞은 물체가 Tile이라면...
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tile")))
        {
            float rayLength = 100.0f;

            // 레이를 그리기 (초록색)
            Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red, 10.0f);

            Debug.Log("hit Something...");
            /*
             * boardScript의 MoveTile함수를 호출
             * 이때 Board boardScript = hit.collider.GetComponentInParent<Board>();를 사용한 이유는 
             * 슬라이드 퍼즐이 여러개 생성될 때를 상정하여 만들었습니다
             * 현재 타일의 부모 보드 정보를 가져오기 위함임
             */
            Board boardScript = hit.collider.GetComponentInParent<Board>();
            boardScript.MoveTile(hit.collider.gameObject);
        }

        return hit;
    }

 

}
