using DG.Tweening;
using UnityEngine;

public class SelectUI : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 defaultPosition;
    [SerializeField] private float yOffset = -120f;
    [SerializeField] private float _move = 200f;
    
    private static SelectUI instance;

    public static SelectUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SelectUI>();
                if (instance == null)
                {
                    Debug.LogError("위치 지정 도움말 인스턴스가 존재하지 않습니다.");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        _rectTransform = transform.GetComponent<RectTransform>();
        defaultPosition = _rectTransform.anchoredPosition;
        _rectTransform.anchoredPosition = defaultPosition + Vector2.up * yOffset;
        
        if (instance != null && instance != this)
        {
            Debug.Log($"Another Instance ({instance.gameObject.name}) exists, Destroy This Instance({gameObject.name}).");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    
    public void ShowPanel()
    {
        _rectTransform.DOAnchorPosY(defaultPosition.y + _move, 0.6f).SetEase(Ease.OutQuad).SetUpdate(true) .OnComplete(() => gameObject.SetActive(true));
    }

    public void HidePanel()
    {
        _rectTransform.DOAnchorPosY(defaultPosition.y, 0.6f).SetEase(Ease.OutQuad).SetUpdate(true);
    }
}