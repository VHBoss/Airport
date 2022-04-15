using DG.Tweening;
using UnityEngine;

public class Case : MonoBehaviour
{
    private Collider m_Collider;
    private Animation m_Animation;
    private Transform m_Slot;
    private float m_MoveSpeed = 3f;
    private float m_JumpSpeed = 0.5f;

    public void Init(Transform slot, Vector3 endPoint)
    {
        m_Collider = GetComponent<Collider>();
        m_Animation = GetComponent<Animation>();

        m_Collider.enabled = false;
        m_Slot = slot;

        transform.DOMove(endPoint, m_MoveSpeed).SetEase(Ease.Linear).OnComplete(JumpToArea);
    }

    private void JumpToArea()
    {
        //m_Collider.enabled = true;
        //m_Animation.enabled = true;

        transform.DOMove(m_Slot.position, m_JumpSpeed).OnComplete(() =>
        {
            m_Collider.enabled = true;
            m_Animation.Play();
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TrailerController trailerController = other.GetComponent<TrailerController>();

            if (trailerController.TryAddItem(this))
            {
                m_Collider.enabled = false;
                m_Animation.enabled = false;
                transform.GetChild(0).localRotation = Quaternion.identity;
                transform.GetChild(0).localPosition = Vector3.zero;

                if (m_Slot)
                {
                    m_Slot.gameObject.SetActive(true);
                }
            }
        }
    }
}
