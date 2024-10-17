using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private int numeric;
    private Vector3 correctPosition;
    public int Numeric{
        set {
            numeric = value;
        } 
        get => numeric;
    }

    public bool IsCorrect { private set; get; } = false;

    public void SetUp(Board board ,int numeric, int hideNumeric)
    {
        //순서를 설정합니다...
        Numeric = numeric;

        //만약 숨겨야 하는 타일이라면...
        if (Numeric == hideNumeric)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            board.EmptyTilePosition = this.transform.localPosition;
        }
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
        while (percent < 1) {
            current += Time.deltaTime;
            percent = current / moveTime;
            GetComponent<Transform>().localPosition = Vector3.Lerp(start, end, percent);
            yield return null;
        }
    }
}
