using System;
using UnityEngine;

public class SpherizeWorld : MonoBehaviour {

    public static Action OnSpherize;
    public static float Radius = 20;
    public static float Shift = 1;

    public float radius = 20;
    public float scale = 10;

    private Vector3[] vertices;
    private Vector3[] result;
    private Mesh mesh;
    private SphereCollider meshCollider;

    private const float Deg2Rad = Mathf.PI / 180.0f;
    private const float Rad2Deg = 180.0f / Mathf.PI;

    void Start () {
        Radius = radius;
        //scale = transform.localScale.x;
        //transform.localScale = Vector3.one;

        meshCollider = GetComponent<SphereCollider>();
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;
        vertices = mesh.vertices;
        result = new Vector3[vertices.Length];

        Spherize();
        SpherizeChildren();
    }

    void SpherizeChildren()
    {
        foreach (Transform t in transform)
        {
            t.gameObject.AddComponent<SpherizeObject>();
        }
    }

    void Spherize_bak()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            result[i] = Coords(vertices[i] * scale);
        }

        mesh.vertices = result;
        //mesh.RecalculateNormals();
        //meshCollider.sharedMesh = mesh;
        meshCollider.radius = Radius;
        meshCollider.center = new Vector3(0, -Radius, 0);

        OnSpherize?.Invoke();
    }

    void Spherize()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            result[i] = Coords(vertices[i]);
        }

        mesh.vertices = result;
        meshCollider.radius = Radius;
        meshCollider.center = new Vector3(0, -Radius, 0);

        OnSpherize?.Invoke();
    }

    public static Vector3 Coords(Vector3 input) 
    {
        Vector3 coords = input * Rad2Deg / Radius;
        var cosLat = Mathf.Cos(coords.z * Deg2Rad);
        var sinLat = Mathf.Sin(coords.z * Deg2Rad);
        var cosLon = Mathf.Cos(coords.x * Deg2Rad);
        var sinLon = Mathf.Sin(coords.x * Deg2Rad);

        coords.y = Radius * cosLat * cosLon - Radius;
        coords.x = Radius * cosLat * sinLon;
        coords.z = Radius * sinLat;

        return coords;
    }

    public static Vector3 ObjectCoords(Vector3 input, float posY)
    {
        float rad = Radius + posY;
        Vector3 coords = input * Rad2Deg / Radius;
        var cosLat = Mathf.Cos(coords.z * Deg2Rad);
        var sinLat = Mathf.Sin(coords.z * Deg2Rad);
        var cosLon = Mathf.Cos(coords.x * Deg2Rad);
        var sinLon = Mathf.Sin(coords.x * Deg2Rad);

        coords.y = rad * cosLat * cosLon - Radius;
        coords.x = rad * cosLat * sinLon;
        coords.z = rad * sinLat;

        return coords;
    }

    private void OnValidate()
    {
        if (mesh != null)
        {
            Radius = radius;
            Spherize();
        }
    }

}
