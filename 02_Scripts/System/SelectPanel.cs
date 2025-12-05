using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectPanel : MonoBehaviour
{
    private SelectUI selectUI;
    private static SelectPanel instance;
    public GameObject _selectPanel;

    public static SelectPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<SelectPanel>(FindObjectsInactive.Include);
                if (instance == null)
                {
                    Debug.LogError("스킬 위치 선택 판넬 인스턴스가 존재하지 않습니다.");
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        selectUI = SelectUI.Instance;

        if (instance != null && instance != this)
        {
            Debug.Log(
                $"Another Instance ({instance.gameObject.name}) is exits, Destroy This Instance({gameObject.name}).");
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void Show()
    {
        _selectPanel.SetActive(true);
        selectUI.ShowPanel();
    }

    public void Hide()
    {
        _selectPanel.SetActive(false);
        selectUI.HidePanel();
    }
}