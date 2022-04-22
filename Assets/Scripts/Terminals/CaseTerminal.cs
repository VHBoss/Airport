using DG.Tweening;
using UnityEngine;

public class CaseTerminal : MonoBehaviour
{
    [SerializeField] private Transform m_StartPoint;
    [SerializeField] private Transform m_EndPoint;
    [SerializeField] private Case m_CasePrefab;
    [SerializeField] private PlaceArea m_Area;
    [SerializeField] private float m_FilingTime = 2f;

    private float m_TotalTime = 0;
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
        m_FilingTime = config.NewCaseEverySec;
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

        Case suitcase = Instantiate(m_CasePrefab, m_StartPoint.position, Quaternion.identity);
        suitcase.Load(slot, m_EndPoint.position);

        slot.gameObject.SetActive(false);
    }
}
