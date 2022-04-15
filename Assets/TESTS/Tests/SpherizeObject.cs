using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherizeObject : MonoBehaviour
{
    private Vector3[] vertices;
    private Vector3[] result;
    private Mesh mesh;
    private MeshCollider meshCollider;

    private void Awake()
    {
        SpherizeWorld.OnSpherize += Spherize;

        meshCollider = GetComponent<MeshCollider>();
        MeshFilter mf = GetComponent<MeshFilter>();
        mesh = mf.mesh;
        vertices = mesh.vertices;
        result = new Vector3[vertices.Length];
    }

    private void OnDestroy()
    {
        SpherizeWorld.OnSpherize -= Spherize;
    }

    void Spherize()
    {
        for (int i = 0; i < vertices.Length; i++)
        {

            Vector3 v = transform.TransformPoint(vertices[i]);
            Vector3 r = SpherizeWorld.ObjectCoords(v, v.y);
            result[i] = transform.InverseTransformPoint(r);

        }

        mesh.vertices = result;
        //mesh.RecalculateNormals();
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
