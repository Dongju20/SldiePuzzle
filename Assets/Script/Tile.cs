using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.UIElements;

public class Tile : MonoBehaviour
{
    private int numeric;
    private Vector3 correctPosition;
    [SerializeField]
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
        if(albedoTexture.GetType() == typeof(Sprite))
        {
            Debug.Log("타입을 바꿉니다...");
            albedoTexture = ConvertType(albedoTexture);
            Debug.Log(albedoTexture.GetType().Name);
        }
    }

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
            this.GetComponent<MeshRenderer>().enabled = false;
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
        material.SetTextureOffset("_MainTex", new Vector2(startX, startY));
        material.SetTextureScale("_MainTex", new Vector2 (sliceSize, sliceSize));
    }


    public void Move(Vector3 end)
    {
        StartCoroutine("MoveTo", end);
    }

    private IEnumerator MoveTo(Vector3 end)
    {
        Debug.Log("위치를 옮깁니다...?" + end);
        float current = 0;
        float percent = 0;
        float moveTime = 0.1f;
        Vector3 start = GetComponent<Transform>().localPosition;
        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / moveTime;
            GetComponent<Transform>().localPosition = Vector3.Lerp(start, end, percent);
            yield return null;
        }
        CheckCorrect();
        board.IsGameOver();
    }

    private void CheckCorrect()
    {
        Debug.Log("현재 위치" + this.transform.localPosition + "원래 있어야 하는 위치 " + correctPosition);
        IsCorrected = correctPosition == this.transform.localPosition ? true : false;
    }

    public void SetCorrectPosition()
    {
        correctPosition = GetComponent<Transform>().localPosition;
    }

    public Vector3 GetCorrectPosition()
    {
        return this.correctPosition;
    }
}
