using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using TMPro;

public class FadeEffect : MonoBehaviour
{
    private Image panel;
    private TextMeshProUGUI text;
    private UnityAction onCompleteEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void FadePanel(float basealpha, float alpha, float duration)
    {
        TryGetComponent<Image>(out panel);

        Color color = panel.color;
        color.a = basealpha;
        panel.color = color;

        panel.DOColor(new Color(color.r, color.g, color.b, alpha), duration).OnComplete(() =>
        {
            if (onCompleteEvent != null)
                onCompleteEvent.Invoke();
        });
    }

    public void FadeText(float basealpha, float alpha, float duration)
    {
        TryGetComponent<TextMeshProUGUI>(out text);

        Color color = text.color;
        color.a = basealpha;
        text.color = color;

        text.DOColor(new Color(color.r, color.g, color.b, alpha), duration).OnComplete(() =>
        {
            if (onCompleteEvent != null)
                onCompleteEvent.Invoke();
        });
    }

    public FadeEffect OnComplete(UnityAction unityAction)
    {
        if (unityAction == null)
            return this;

        onCompleteEvent = null;
        onCompleteEvent = unityAction;

        return this;
    }
}
