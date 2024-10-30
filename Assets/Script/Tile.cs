using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    //1 2 3 4 이런식으로 현재 타일의 정보를 나타냄
    private int numeric;
    //알맞은 타일의 위치를 나타냅니다..
    //얘는 생성될 때 초기화 한 후 그 뒤론 값이 안바뀜
    private Vector3 correctPosition;
    //내가 넣고 싶은 그림 오브젝트
    //Sprite Image든 상관없이 적용 가능하게 함
    private Object albedoTexture;
    public int Numeric{
        set {
            numeric = value;
        } 
        get => numeric;
    }
    private Board board;
    //현재 위치가 맞는지 안맞는지 확인..
    public bool IsCorrected { private set; get; } = false;
    public void CheckType()
    {
        //Sprite면 Texture로 바꿔야 함
        if(albedoTexture.GetType() == typeof(Sprite))
        {
            Debug.Log("타입을 바꿉니다...");
            //Sprite -> Texture를 담당하는 함수
            albedoTexture = ConvertType(albedoTexture);
            Debug.Log(albedoTexture.GetType().Name);
        }
    }

    //Sprite -> Texture로 바꿔주는 함수
    public Texture ConvertType(Object albedoTexture)
    {
        Sprite sprite = (Sprite)albedoTexture;
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

    //현재 타일이 올바른 위치에 있는가 확인 여부
    public bool IsCorrect { private set; get; } = false;

    public void SetUp(Board board ,int numeric, int hideNumeric, int xPosition, int yPosition)
    {
        this.board = board;
        CheckType();
        //순서를 설정합니다...
        Numeric = numeric;

        //만약 숨겨야 하는 타일이라면...
        if (Numeric == hideNumeric)
        {
            //렌더를 꺼 아예 안보이게 하여 마치 없는 것 처럼 표현함
            this.GetComponent<MeshRenderer>().enabled = false;
            //부모 오브젝트의 board에 빈 타일 정보를 등록함
            board.EmptyTile = this.gameObject;
        }
        //x포지션과 y포지션 및 블록크기
        //이때 블록크기 == hideNumeric
        //그림을 슬라이싱합니다...
        int slice = (int)Mathf.Sqrt(hideNumeric);
        float sliceSize = 1.0f / slice;
        float startX = (float)xPosition / slice;
        float startY = 1.0f - (float)(yPosition + 1) / slice;
        Debug.Log(slice + " " + startX + " " + startY);
        //머터리얼의 값을 교체합니다...
        Material material = new Material(Shader.Find("Standard"));
        Debug.Log(albedoTexture.GetType().Name);
        material.mainTexture = (Texture)albedoTexture;
        this.GetComponent<MeshRenderer>().material = material;
        //머터리얼 값을 설정함
        material.SetTextureOffset("_MainTex", new Vector2(startX, startY));
        material.SetTextureScale("_MainTex", new Vector2 (sliceSize, sliceSize));
    }

    //움직이는 함수 호출
    public void Move(Vector3 end)
    {
        StartCoroutine("MoveTo", end);
    }

    //Vetor3 end --> 옮길 위치
    private IEnumerator MoveTo(Vector3 end)
    {
        float current = 0;
        float percent = 0;
        float moveTime = 0.1f;
        //현재 내 위치에서
        Vector3 start = GetComponent<Transform>().localPosition;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;
            //start 위치에서 end위치로 옮김...
            //이때 부드럽게 움직이게 하기 위해 선형보간법 사용
            GetComponent<Transform>().localPosition = Vector3.Lerp(start, end, percent);
            yield return null;
        }
        //옮긴 위치가 원래 있어야 하는 위치인지 확인
        CheckCorrect();
        //그 다음 모든 타일이 원래 위치에 있는가 확인하는 함수
        board.IsGameOver();
    }

    //현재 위치와 원래 있어야하는 위치를 비교하여 IsCorrect를 true혹은 false로 설정하는 함수
    private void CheckCorrect()
    {
        Debug.Log("현재 위치" + this.transform.localPosition + "원래 있어야 하는 위치 " + correctPosition);
        IsCorrected = correctPosition == this.transform.localPosition ? true : false;
    }
    //원래 있어야 하는 위츠를 설정하는 함수
    public void SetCorrectPosition()
    {
        correctPosition = GetComponent<Transform>().localPosition;
    }

    public Vector3 GetCorrectPosition()
    {
        return this.correctPosition;
    }

    public void setAlbedoTexture(Object albedoTexture)
    {
        this.albedoTexture = albedoTexture;
    }
}
