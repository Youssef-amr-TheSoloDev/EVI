using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AppearOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMPro.TextMeshProUGUI targetText;
    [SerializeField] private float Epsilon = 0.1f, TransitionSpeed = 2f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(SetFontAlpha(1));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(SetFontAlpha(0));
    }

    private IEnumerator SetFontAlpha(float alpha)
    {
        yield return new WaitUntil(() =>
        {
            targetText.color = Color.Lerp(targetText.color, new Color(targetText.color.r, targetText.color.g, targetText.color.b, alpha), Time.deltaTime * TransitionSpeed);
            return Mathf.Abs(alpha - targetText.color.a) < Epsilon;
        });
        targetText.color = new Color(targetText.color.r, targetText.color.g, targetText.color.b, alpha);
    }
}
