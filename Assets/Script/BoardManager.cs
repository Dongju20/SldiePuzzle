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
        float rayLength = 10.0f;
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red, 10.0f);
        
        RaycastHit hit;
        //raycast에서 맞은 물체가 클릭 가능한 오브젝트이면...
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("SlidingPuzzle"))) {
            Debug.Log("hit clickable object ...");

            IClickableObj clickableObj = hit.collider.GetComponent<IClickableObj>();
            if (clickableObj != null)
            {
                clickableObj.ClickObj();
            }
        }

        return hit;
    }

 

}
