using UnityEngine;

public class PuzzleCameraResolution : MonoBehaviour
{
    public bool isAutoSize; // 해상도 자동 지정 여부. (기기의 해상도 사용.).
    
    public int width;   // 설정할 너비.
    public int height;  // 설정할 높이.

    private int curDeviceWidth;     //  현재 기기 너비.
    private int curDeviceHeight;    // 현재 기기 높이.
    
    private Camera puzzleCamera;    // 퍼즐 카메라.

    private void Awake() {
        puzzleCamera = GetComponent<Camera>();  // 카메라 컴포넌트 가져옴.
    }
    
    // 초기 화면 해상도 맞춤.
    void Start()
    {
        curDeviceWidth = Screen.width;      // 현재 기기 너비 저장.
        curDeviceHeight = Screen.height;    // 현재 기기 높이 저장.

        SetResolution();    // 초기 해상도 설정.
    }

    // 화면 크기 변경 시 해상도 맞춤.
    private void Update()
    {
        int deviceWidth = Screen.width; // 기기 너비 저장.
        int deviceHeight = Screen.height; // 기기 높이 저장.

        // 현재 기기의 너비나 높이가 변경되면,
        if (curDeviceWidth != deviceWidth || curDeviceHeight != deviceHeight)
        {
            // 변경된 너비/높이로 재설정하고,
            curDeviceWidth = deviceWidth;
            curDeviceHeight = deviceHeight;

            SetResolution(); // 창 크기 변경시에도 게임 해상도 고정.
        }
    }

    // 해상도 설정 메소드.
    private void SetResolution()
    {
        if (isAutoSize) // 자동 지정 여부 true 시, 현재 기기 해상도 가져옴. 
        {
            width = curDeviceWidth;
            height = curDeviceHeight;
        }

        Screen.SetResolution(width, (int)((float)curDeviceWidth / curDeviceHeight * width), true); // SetResolution 함수 제대로 사용하기.

        if ((float)width / height < (float)curDeviceWidth / curDeviceHeight) // 기기의 해상도 비가 더 큰 경우.
        {
            float newWidth = width * curDeviceHeight / (float)(height * curDeviceWidth); // 새로운 너비.
            puzzleCamera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용.
        }
        else // 게임의 해상도 비가 더 큰 경우.
        {
            float newHeight = height * curDeviceWidth / (float)(width * curDeviceHeight); // 새로운 높이.
            puzzleCamera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용.
        }
    }
}