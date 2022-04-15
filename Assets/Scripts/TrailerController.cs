using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TrailerController : MonoBehaviour
{    
    [SerializeField] private float m_TrailerDistance = 2.7f;
    [SerializeField] private int m_MaxTrailersCount = 2;
    [SerializeField] private int m_StartTrailerCount = 1;
    [SerializeField] private Trailer m_TrailerPrefab;

    private List<Trailer> m_Trailers = new List<Trailer>();
    private List<Transform> m_TrailersTransform = new List<Transform>();
    private List<float> m_Distances = new List<float>();
    private List<Vector3> m_Positions = new List<Vector3>();
    private List<Quaternion> m_Rotations = new List<Quaternion>();
    private Vector3 m_PrevPos;
    private float m_TotalLength = 0;
    private float m_MaxLength = 3;

    private void Start()
    {
        m_Distances.Add(0);
        m_Positions.Add(transform.position);
        m_Rotations.Add(transform.rotation);

        m_TotalLength = 0;
        m_PrevPos = transform.position;
        m_MaxLength = m_TrailerDistance * m_Trailers.Count + 1;

        for (int i = 0; i < m_StartTrailerCount; i++)
        {
            AddTrail();
        }
    }

    void Update()
    {
        UpdatePath();
        UpdateTrailers();
    }

    void UpdatePath()
    {
        float distance = Vector3.Distance(m_PrevPos, transform.position);
        if (distance > 0)
        {
            m_PrevPos = transform.position;
            m_TotalLength += distance;

            m_Positions.Insert(0, transform.position);
            m_Rotations.Insert(0, transform.rotation);
            m_Distances.Insert(0, distance);

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
            float totalDistance = 0;
            for (int i = 0; i < m_Distances.Count - 1; i++)
            {
                totalDistance += m_Distances[i];
                if (totalDistance >= distance)
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
        Trailer trailer = null;
        if (m_Trailers.Count > 0)
        {
            int lastIndex = m_Trailers.Count - 1;
            trailer = Instantiate(m_TrailerPrefab, m_Trailers[lastIndex].transform.position, m_TrailersTransform[lastIndex].transform.rotation);
        }
        else
        {
            trailer = Instantiate(m_TrailerPrefab, transform.position, transform.rotation);
        }
        m_Trailers.Add(trailer);
        m_TrailersTransform.Add(trailer.transform);
        m_MaxLength = m_TrailerDistance * m_Trailers.Count + 1;
    }

    public void RemoveTrail()
    {
        if (m_Trailers.Count > 0)
        {
            Trailer trailer = m_Trailers[m_Trailers.Count - 1];
            m_Trailers.RemoveAt(m_Trailers.Count - 1);
            m_TrailersTransform.RemoveAt(m_TrailersTransform.Count - 1);
            Destroy(trailer.gameObject);
            m_MaxLength = m_TrailerDistance * m_Trailers.Count + 1;
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 50, 20), m_Positions.Count.ToString());
    }

    private void OnDrawGizmos()
    {
        foreach (var item in m_Positions)
        {
            Gizmos.DrawSphere(item, 0.5f);
        }
    }
#endif
}
