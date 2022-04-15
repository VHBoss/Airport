using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherizeObject_v2 : MonoBehaviour
{
    public float scale = 1;

    private Vector3[] vertices;
    private Vector3[] result;
    private Mesh mesh;
    private MeshCollider meshCollider;

    private void Awake()
    {
        WorldController.OnSpherize += Spherize;

        meshCollider = GetComponent<MeshCollider>();
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;
        vertices = mesh.vertices;
        result = new Vector3[vertices.Length];
    }

    private void OnDestroy()
    {
        WorldController.OnSpherize -= Spherize;
    }

    void Spherize()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = transform.TransformPoint(vertices[i]);
            Vector3 r = WorldController.ObjectCoords(v, transform.localScale.x);
            result[i] = transform.InverseTransformPoint(r);
        }

        mesh.vertices = result;
        mesh.RecalculateBounds();
        //meshCollider.sharedMesh = mesh;
    }

    private void OnValidate()
    {
        if (mesh != null)
        {
            Spherize();
        }
    }
}
