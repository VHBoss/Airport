using DG.Tweening;
using UnityEngine;

public class TrailerStack : MonoBehaviour
{
    [SerializeField] private Transform m_ItemRoot;
    [SerializeField] private float m_Offset = 0.5f;
    [SerializeField] private int m_MaxCount = 8;

    public int ItemCount => m_ItemRoot.childCount;

    private float m_AnimationTime = 0.3f;

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
