using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Quad : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Vector3[] vertices;
    private int[] indices;
    private Vector2[] uv;
    // Start is called before the first frame update
    public void GenerateMesh()
    {
        this.AddComponent<MeshFilter>();
        meshFilter = GetComponent<MeshFilter>();
        //사각형을 이루는 4개의 정점 생성
        vertices = new Vector3[4]
        {
            new Vector3 (0,0,0),
            new Vector3 (0,1,0),
            new Vector3 (1,0,0),
            new Vector3 (1,1,0),
        };
        //정점을 3개 단위로 묶어 삼각형으로 표현
        indices = new int[6]
        {
            0,1,2,
            2,1,3
        };

        uv = new Vector2[4]
        {
            new Vector2(0,0),
            new Vector2(0,1),
            new Vector2(1,0),
            new Vector2(1,1)
        };

        //메시를 생성하고, 정점(vertices)를 설정한 후 meshFilter의 mesh에 등록
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = indices;
        meshFilter.mesh = mesh;
    }
}
