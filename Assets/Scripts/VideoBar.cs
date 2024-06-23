using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoBar : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private VideoPlayer videoPlayer;
    private Image progress;

    private void Awake()
    {
        progress = GetComponent<Image>();
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer is not assigned!");
        }
        if (progress == null)
        {
            Debug.LogError("Image component is not found on this GameObject!");
        }
    }

    private void Update()
    {
        if (videoPlayer != null && videoPlayer.frameCount > 0)
            progress.fillAmount = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
    }

    public void OnDrag(PointerEventData eventData)
    {
        TrySkip(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TrySkip(eventData);
    }

    private void TrySkip(PointerEventData eventData)
    {
        if (progress == null)
        {
            Debug.LogError("Progress Image is not assigned!");
            return;
        }

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(progress.rectTransform, eventData.position, null, out localPoint))
        {
            float pct = Mathf.InverseLerp(progress.rectTransform.rect.xMin, progress.rectTransform.rect.xMax, localPoint.x);
            SkipToPercent(pct);
        }
    }

    private void SkipToPercent(float pct)
    {
        if (videoPlayer != null)
        {
            var frame = (long)(videoPlayer.frameCount * pct);
            videoPlayer.frame = frame;
        }
    }
}
