using DG.Tweening;
using UnityEngine;

public class Trailer : MonoBehaviour
{
    [SerializeField] private float m_OffsetX;
    [SerializeField] private float m_OffsetY;
    [SerializeField] private float m_OffsetZ;
    [SerializeField] private Transform m_ItemRoot;
    [SerializeField] private int m_MaxCount = 8;

    public int ItemCount => m_ItemRoot.childCount;

    private float m_AnimationTime = 0.3f;

    public bool TryAddItem(Case item)
    {
        int count = m_ItemRoot.childCount;
        if (count < m_MaxCount)
        {
            int x = count % 4;
            int y = count / 8;
            int z = count / 4;

            Vector3 slotPosition = new Vector3(x * m_OffsetX, y * m_OffsetY, z * m_OffsetZ);

            Transform itemTransform = item.transform;
            itemTransform.SetParent(m_ItemRoot);
            itemTransform.DOLocalMove(slotPosition, m_AnimationTime);
            itemTransform.DOLocalRotate(Vector3.zero, m_AnimationTime);

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
