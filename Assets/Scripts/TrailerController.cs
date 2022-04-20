using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TrailerController : MonoBehaviour
{    
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_TrailerDistance = 1.6f;
    [SerializeField] private int m_MaxTrailersCount = 2;
    [SerializeField] private int m_DebugStartTrailerCount = 1;
    [SerializeField] private TrailerStack m_TrailerPrefab;

    private List<TrailerStack> m_Trailers = new List<TrailerStack>();
    private List<Transform> m_TrailersTransform = new List<Transform>();
    private List<float> m_Distances = new List<float>();
    private List<Vector3> m_Positions = new List<Vector3>();
    private List<Quaternion> m_Rotations = new List<Quaternion>();
    private Vector3 m_PrevPos;
    private float m_TotalLength = 0;
    private float m_MaxLength = 0;
    private float m_Distance = 0;

    private void Awake()
    {
        GameConfig.SettingsChanged += OnSettingsChanged;
    }

    private void OnDestroy()
    {
        GameConfig.SettingsChanged -= OnSettingsChanged;
    }

    private void OnSettingsChanged(GameConfig config)
    {
        m_TrailerDistance = config.TrailerDistance;
        m_MaxTrailersCount = config.MaxTrailers;
        m_DebugStartTrailerCount = config.StartTrailers;
    }

    private void Start()
    {
        if(m_Target == null)
        {
            m_Target = transform;
        }

        m_Distances.Add(0);
        m_Positions.Add(m_Target.position);
        m_Rotations.Add(m_Target.rotation);

        m_TotalLength = 0;
        m_PrevPos = m_Target.position;
        m_MaxLength = m_TrailerDistance * (m_Trailers.Count + 2);

        for (int i = 0; i < m_DebugStartTrailerCount; i++)
        {
            AddTrailDebug();
        }
    }

    void Update()
    {
        UpdatePath();
        UpdateTrailers();
    }

    void UpdatePath()
    {
        m_Distance = Vector3.Distance(m_PrevPos, m_Target.position);
        if (m_Distance > 1)
        {
            m_PrevPos = m_Target.position;
            m_TotalLength += m_Distance;

            m_Positions.Insert(0, m_Target.position);
            m_Rotations.Insert(0, m_Target.rotation);
            m_Distances.Insert(0, m_Distance);
            m_Distance = 0;

            if (m_TotalLength > m_MaxLength)
            {
                int index = m_Positions.Count - 1;
                m_TotalLength -= m_Distances[index];
                m_Rotations.RemoveAt(index);
                m_Distances.RemoveAt(index);
                m_Positions.RemoveAt(index);
            }
        }        
    }

    void UpdateTrailers()
    {
        for (int j = 0; j < m_Trailers.Count; j++)
        {
            float distance = m_TrailerDistance * (j + 1);
            float totalDistance = m_Distance;

            for (int i = 0; i < m_Positions.Count - 1; i++)
            {
                totalDistance += m_Distances[i];
                if (totalDistance > distance)
                {
                    float val = (totalDistance - distance) / m_Distances[i];
                    m_TrailersTransform[j].position = Vector3.Lerp(m_Positions[i + 1], m_Positions[i], val);
                    m_TrailersTransform[j].rotation = Quaternion.Slerp(m_Rotations[i + 1], m_Rotations[i], val);
                    //m_TrailersTransform[j].rotation = Quaternion.LookRotation(m_Positions[i] - m_Positions[i + 1]);
                    break;
                }
            }
        }
    }

    public bool TryAddItem(Case item)
    {
        foreach (var trailer in m_Trailers)
        {
            if (trailer.TryAddItem(item))
            {
                return true;
            }
        }

        if (TryAddTrailer())
        {
            if (m_Trailers[m_Trailers.Count - 1].TryAddItem(item))
            {
                return true;
            }
        }

        return false;
    }

    private bool TryAddTrailer()
    {
        if(m_Trailers.Count < m_MaxTrailersCount)
        {
            AddTrail();
            return true;
        }
        return false;
    }

    public Case GetItem()
    {
        for (int i = m_Trailers.Count - 1; i >= 0; i--)
        {
            Case item = m_Trailers[i].GetItem();
            if (item != null)
            {
                item.transform.SetParent(null);

                if(m_Trailers[i].ItemCount == 0)
                {
                    StartCoroutine(WiatAndRemoveTrailer());
                }
                
                return item;
            }
        }

        return null;
    }

    IEnumerator WiatAndRemoveTrailer()
    {
        yield return new WaitForSeconds(0.2f);
        RemoveTrail();
    }

    public void AddTrail()
    {
        TrailerStack trailer = Instantiate(m_TrailerPrefab);
        SetPosition(trailer.transform);
        m_Trailers.Add(trailer);
        m_TrailersTransform.Add(trailer.transform);
        m_MaxLength = m_TrailerDistance * (m_Trailers.Count + 2);
    }

    private void SetPosition(Transform trailer)
    {
        float distance = m_TrailerDistance * (m_Trailers.Count + 1);
        float totalDistance = m_Distance;
        for (int i = 0; i < m_Distances.Count - 1; i++)
        {
            totalDistance += m_Distances[i];
            if (totalDistance >= distance)
            {
                float val = (totalDistance - distance) / m_Distances[i];
                trailer.position = Vector3.Lerp(m_Positions[i + 1], m_Positions[i], val);
                trailer.rotation = Quaternion.Slerp(m_Rotations[i + 1], m_Rotations[i], val);
                break;
            }
        }
    }

    public void RemoveTrail()
    {
        if (m_Trailers.Count > 0)
        {
            TrailerStack trailer = m_Trailers[m_Trailers.Count - 1];
            m_Trailers.RemoveAt(m_Trailers.Count - 1);
            m_TrailersTransform.RemoveAt(m_TrailersTransform.Count - 1);
            Destroy(trailer.gameObject);
            m_MaxLength = m_TrailerDistance * (m_Trailers.Count + 2);
        }
    }

    public void AddTrailDebug()
    {
        TrailerStack trailer = null;
        if (m_Trailers.Count > 0)
        {
            int lastIndex = m_Trailers.Count - 1;
            trailer = Instantiate(m_TrailerPrefab, m_Trailers[lastIndex].transform.position, m_TrailersTransform[lastIndex].transform.rotation);
        }
        else
        {
            trailer = Instantiate(m_TrailerPrefab, m_Target.position - m_Target.forward * m_TrailerDistance, m_Target.rotation);
        }
        m_Trailers.Add(trailer);
        m_TrailersTransform.Add(trailer.transform);
        m_MaxLength = m_TrailerDistance * (m_Trailers.Count + 2);
    }

#if UNITY_EDITOR
    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 10, 50, 20), m_Positions.Count.ToString());
    //}

    private void OnDrawGizmos()
    {
        foreach (var item in m_Positions)
        {
            Gizmos.DrawSphere(item, 0.5f);
        }
    }
#endif
}
