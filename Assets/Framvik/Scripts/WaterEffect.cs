using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour
{
    private Mesh mesh;

    List<Vector3> vertices = new List<Vector3>();
    List<int> indexes = new List<int>();
    //List<float> heights = new List<float>();
    float[] heights = new float[100*100];

    List<Vector2> uvs = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        for (int i = 0; i < 100 * 100; i++)
            heights[i] = 0;// heights.Add(0);
        buffer = new ComputeBuffer(100 * 100, 4);
        MakeWater();
    }
    public ComputeBuffer buffer;
    public ComputeShader waterShader;

    //Skapa en vattenyta
    void MakeWater() 
    {
        mesh.Clear();
        vertices.Clear();
        indexes.Clear();
        uvs.Clear();

        int x, y;
        for (y = 0; y < 100; y++)
        {
            for (x = 0; x < 100; x++)
            {
                Vector3 vector = new Vector3(x - 50, heights[y * 100 + x], y - 50);
                Vector2 uv = new Vector2(x / 100f, y / 100f);
                vertices.Add(vector);
                uvs.Add(uv);
            }
        }

        for (y = 0; y < 99; y++)
        {
            for (x = 0; x < 99; x++)
            {
                int a = y * 100 + x;
                int b = y * 100 + x + 1;
                int c = (y + 1) * 100 + x + 1;
                int d = (y + 1) * 100 + x;

                indexes.AddRange(new int[] { a, c, b });
                indexes.AddRange(new int[] { a, d, c });
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetIndices(indexes, MeshTopology.Triangles, 0);
        mesh.SetUVs(0, uvs);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void RefreshWater()
    {
        mesh.Clear();
        vertices.Clear();

        int x, y;
        for (y = 0; y < 100; y++)
        {
            for (x = 0; x < 100; x++)
            {
                Vector3 vector = new Vector3(x - 50, heights[y * 100 + x], y - 50);
                vertices.Add(vector);
            }
        }

        mesh.SetVertices(vertices);
        mesh.SetIndices(indexes, MeshTopology.Triangles, 0);
        mesh.SetUVs(0, uvs);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    int fix(int coord)
    {
        return Mathf.Max(Mathf.Min(coord, 99), 0);
    }

    float GetY(int x, int z)
    {
        return heights[fix(z) * 100 + fix(x)];
    }

    void UpdateWater()
    {
        int x, z;
        for (z = 0; z < 100; z++)
        {
            for (x = 0; x < 100; x++)
            {
                float y = (GetY(x - 1, z) + GetY(x + 1, z) + GetY(x, z - 1) + GetY(x, z + 1)) / 2f - GetY(x, z);
                heights[z * 100 + x] = y;
                heights[z * 100 + x] -= heights[z * 100 + x] / 32;

            }
        }
    }

    void Splash(Vector3 v)
    {
        for (int z = (int)v.z + 50 - 2; z < (int)v.z + 50 + 2; z++)
        {
            for (int x = (int)v.x + 50 - 2; x < (int)v.x + 50 + 2; x++)
            {
                heights[fix(z) * 100 + fix(x)] -= 5f;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach(var contact in collision.contacts)
        {
            Splash(contact.point);
            Debug.Log("Water!" + contact.point);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateWater();
    }

    float dt = 0;
    private void FixedUpdate()
    {
        dt += Time.fixedDeltaTime;
        if (dt > 0.1f)
        {
            UpdateWater();
            RefreshWater();
            GetComponent<MeshCollider>().sharedMesh = mesh;
            dt = 0;
        }
    }

    private void OnDestroy()
    {
        buffer.Dispose();
    }
}
