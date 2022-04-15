using DG.Tweening;
using UnityEngine;

public class CaseTerminal : MonoBehaviour
{
    [SerializeField] private Transform m_EndPoint;
    [SerializeField] private Case m_CasePrefab;
    [SerializeField] private PlaceArea m_Area;
    [SerializeField] private float m_FilingTime = 2f;
    [SerializeField] private float m_MoveTime = 3f;

    private float m_TotalTime = 0;

    void Start()
    {

    }

    private void Update()
    {
        if(Time.time > m_TotalTime)
        {
            m_TotalTime += m_FilingTime;
            CreateCase();
        }
    }

    private void CreateCase()
    {
        Transform slot = m_Area.GetEmptySlot();
        if(slot == null)
        {
            return;
        }

        Case suitcase = Instantiate(m_CasePrefab, transform.position, Quaternion.identity);
        suitcase.Init(slot, m_EndPoint.position);

        slot.gameObject.SetActive(false);
    }
}
