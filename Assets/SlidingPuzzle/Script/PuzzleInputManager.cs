using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PuzzleInputManager : MonoBehaviour
{
    private Camera puzzleCamera;
    
    void Awake() {
        puzzleCamera = GetComponent<Camera>();
    }

    void Update()
    {
        UpdateInput();
    }

    private void UpdateInput()
    {
        //입력을 받을 때마다 Click함수를 호출
        if (Input.GetMouseButtonDown(0))
        {
            if (!PuzzleManager.GetInstance().GetIsShuffle()) {
                Click();
            }
        }
    }

    public void Click()
    {
        //화면에서 클릭한 위치로 레이를 생성
        Ray ray = puzzleCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //shootRay함수를 호출
        hit = ShootRay(ray);
    }

    private RaycastHit ShootRay(Ray ray)
    {
        float rayLength = 10.0f;
        Debug.DrawRay(ray.origin, ray.direction * rayLength, Color.red, 10.0f);

        RaycastHit[] raycastHits = Physics.RaycastAll(ray, rayLength, LayerMask.GetMask("SlidingPuzzle"));

        RaycastHit hit;
        // raycast 에서 맞은 물체가 SlidingPuzzle 레이어의 오브젝트이면...
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("SlidingPuzzle")))
        {
            Debug.Log("hit clickable object ...");

            // 클릭 가능한 오브젝트인지 확인 , IClickableObj 인터페이스를 상속받으면, 클릭 가능한 오브젝트임.
            if (hit.collider.TryGetComponent<IClickableObj>(out var clickableObj))
            {
                clickableObj.ClickObj();
            }
        }

        //테스트용 나중에 지우세요...
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("StartBtn")))
        {
            Debug.Log("hit clickable object ...");

            if (hit.collider.TryGetComponent<ActiveBtn>(out var clickableObj))
            {
                clickableObj.DoActive(true);
            }
        }
        return hit;
    }

 

}
