using UnityEngine;

[ExecuteInEditMode]
public class CurvedWorld : MonoBehaviour {

    [Range(0,1)]
    public float SpherizeX = 0;
    [Range(0, 1)]
    public float SpherizeY = 0;

    private Vector3 curvature = new Vector3(0.01f, 0.01f, 0);
    private Vector3 result = Vector3.zero;
    private int CurvatureID = Shader.PropertyToID("_Curvature");

    void Update()
    {
        Shader.SetGlobalVector(CurvatureID, result);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        result = new Vector3(0.01f * SpherizeX, 0.01f * SpherizeY, 0);
    }
#endif
}
