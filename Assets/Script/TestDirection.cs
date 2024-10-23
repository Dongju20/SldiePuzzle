using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDirection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] gameObjects = new GameObject[4];
        //���� ����ĳ����
        gameObjects[0] = RaycastInDirection(transform.right);
        gameObjects[1] = RaycastInDirection(-transform.right);
        gameObjects[2] = RaycastInDirection(transform.up);
        gameObjects[3] = RaycastInDirection(-transform.up);
    }

    GameObject RaycastInDirection(Vector3 direction)
    {
        Debug.Log("����Ÿ�� �˻�..");
        RaycastHit hit;
        float rayDistance = 10f;
        // direction���� ����ĳ��Ʈ �߻�
        Debug.DrawRay(transform.position, direction * rayDistance, Color.red, 10f);
        if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, LayerMask.GetMask("Tile")))
        {
            Debug.Log("hit Something...");
            return (hit.collider.gameObject);
        }
        else
            return null;
    }
}
