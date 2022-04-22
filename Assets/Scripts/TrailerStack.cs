using DG.Tweening;
using System;
using UnityEngine;

public class TrailerStack : MonoBehaviour
{
    [SerializeField] private Transform m_ItemRoot;
    [SerializeField] private Transform m_Body;
    [SerializeField] private Transform m_Wheels;    
    [SerializeField] private float m_Offset = 0.285f;
    [SerializeField] private int m_MaxCount = 8;
    [SerializeField] private float m_TrailAppearSpeed = 0.5f;
    [SerializeField] private AnimationCurve m_AppearCurve;
    [SerializeField] private float m_TrailDisappearSpeed = 0.3f;
    [SerializeField] private AnimationCurve m_DisppearCurve;

    public int ItemCount => m_ItemRoot.childCount;

    private float m_AnimationTime = 0.3f;

    private void Awake()
    {
        OnSettingsChanged(SettingsReader.I.gameConfig);
        GameConfig.SettingsChanged += OnSettingsChanged;
    }

    private void OnDestroy()
    {
        GameConfig.SettingsChanged -= OnSettingsChanged;
    }

    private void OnSettingsChanged(GameConfig config)
    {
        m_MaxCount = config.MaxSuitcaseInTrail;
        m_AnimationTime = config.SuitcaseAnimationTime;
        m_TrailAppearSpeed = config.TrailAppearSpeed;
        m_TrailDisappearSpeed = config.TrailDisappearSpeed;
    }

    public void Init()
    {
        m_Body.localScale = Vector3.zero;
        m_Wheels.localScale = Vector3.zero;
        m_Body.DOScale(1, m_TrailAppearSpeed).SetEase(m_AppearCurve);
        m_Wheels.DOScale(1, m_TrailAppearSpeed).SetEase(m_AppearCurve);
    }

    public void Remove(Action OnRemoved)
    {
        m_Body.DOScale(0, m_TrailDisappearSpeed).SetEase(m_DisppearCurve);
        m_Wheels.DOScale(0, m_TrailDisappearSpeed).SetEase(m_DisppearCurve)
            .OnComplete(() => { 
                OnRemoved.Invoke();
                Destroy(gameObject);
            });
    }

    public bool TryAddItem(Case item)
    {
        if (m_ItemRoot.childCount < m_MaxCount)
        {
            Vector3 slotPosition = new Vector3(0, (m_ItemRoot.childCount + 1) * m_Offset, 0);

            Transform itemTransform = item.transform;
            itemTransform.SetParent(m_ItemRoot);
            itemTransform.DOLocalMove(slotPosition, m_AnimationTime);
            itemTransform.DOLocalRotate(new Vector3(0, 90, -90), m_AnimationTime);

            return true;
        }

        return false;
    }

    public Case GetItem()
    {
        if (m_ItemRoot.childCount > 0)
        {
            return m_ItemRoot.GetChild(m_ItemRoot.childCount - 1).GetComponent<Case>();
        }
        else
        {
            return null;
        }
    }
}
