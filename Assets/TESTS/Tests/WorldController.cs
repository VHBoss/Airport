using System;
using UnityEngine;

public class WorldController : MonoBehaviour {

    public static Action OnSpherize;
    public static float Radius = 20;

    public float radius = 20;

    private SphereCollider meshCollider;

    private const float Deg2Rad = Mathf.PI / 180.0f;
    private const float Rad2Deg = 180.0f / Mathf.PI;

    void Start () {
        Radius = radius;
        meshCollider = GetComponent<SphereCollider>();
        meshCollider.radius = Radius;
        meshCollider.center = new Vector3(0, -Radius, 0);

        MeshFilter[] Meshes = GetComponentsInChildren<MeshFilter>();
        foreach (var item in Meshes)
        {
            item.gameObject.AddComponent<SpherizeObject_v2>();
        }

        OnSpherize?.Invoke();
    }

    public static Vector3 ObjectCoords(Vector3 input, float scale)
    {
        input /= scale;
        float rad = Radius + input.y;
        Vector3 coords = input * Rad2Deg / Radius * scale;
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
        if (meshCollider)
        {
            Radius = radius;
            meshCollider.radius = Radius;
            meshCollider.center = new Vector3(0, -Radius, 0);
            OnSpherize?.Invoke();
        }
    }

}
