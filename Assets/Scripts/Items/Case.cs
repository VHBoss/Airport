using DG.Tweening;
using UnityEngine;

public class Case : MonoBehaviour
{
    private Collider m_Collider;
    private Animation m_Animation;
    private Transform m_Slot;
    private float m_MoveTime = 3f;
    private float m_JumpTime = 0.5f;

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
        m_MoveTime = config.CaseMoveTime;
        m_JumpTime = config.CaseJumpTime;
    }

    public void Load(Transform slot, Vector3 endPoint)
    {
        m_Collider = GetComponent<Collider>();
        m_Animation = GetComponent<Animation>();

        m_Collider.enabled = false;
        m_Slot = slot;

        transform.DOMove(endPoint, m_MoveTime).SetEase(Ease.Linear).OnComplete(JumpToArea);
    }

    private void JumpToArea()
    {
        transform.DOMove(m_Slot.position, m_JumpTime).OnComplete(() =>
        {
            m_Collider.enabled = true;
            m_Animation.Play();
        });
    }

    public void Unload(Transform startPoint, Transform endPoint, float trapSpeed)
    {
        DOTween.Sequence()
            .Append(transform.DOMove(startPoint.position, m_JumpTime))
            .Join(transform.DORotate(startPoint.eulerAngles, m_JumpTime))
            .Append(transform.DOMove(endPoint.position, trapSpeed).SetEase(Ease.Linear))
            .OnComplete(()=> Destroy(gameObject));
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
