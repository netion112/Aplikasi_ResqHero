using Nobi.UiRoundedCorners;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoControl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RawImage videoImage;
    public Button fullscreenButton;
    public Button minimizeButton;
    public Camera mainCamera;

    private Vector2 originalSize;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;
    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;
    private Vector2 originalPivot;
    private float originalRadius;

    private ImageWithRoundedCorners roundedCornersScript;

    void Start()
    {
        fullscreenButton.onClick.AddListener(SetFullscreen);
        minimizeButton.onClick.AddListener(SetMinimize);

        // Save the original size, position, rotation, scale, anchors, and pivot
        originalSize = videoImage.rectTransform.sizeDelta;
        originalPosition = videoImage.rectTransform.localPosition;
        originalRotation = videoImage.rectTransform.localRotation;
        originalScale = videoImage.rectTransform.localScale;
        originalAnchorMin = videoImage.rectTransform.anchorMin;
        originalAnchorMax = videoImage.rectTransform.anchorMax;
        originalPivot = videoImage.rectTransform.pivot;

        // Get the ImageWithRoundedCorners script
        roundedCornersScript = videoImage.GetComponent<ImageWithRoundedCorners>();

        // Save the original radius
        if (roundedCornersScript != null)
        {
            originalRadius = roundedCornersScript.radius;
        }
    }

    void SetFullscreen()
    {
        // Set anchors to stretch across the canvas
        videoImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        videoImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        videoImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);

        // Center the video image
        videoImage.rectTransform.anchoredPosition = Vector2.zero;

        // Maintain original size
        videoImage.rectTransform.sizeDelta = originalSize;

        // Rotate Raw Image on the z-axis by -90 degrees
        videoImage.rectTransform.localRotation = Quaternion.Euler(0, 0, -90);

        // Scale the Raw Image to 0.313
        videoImage.rectTransform.localScale = new Vector3(0.313f, 0.313f, 0.313f);

        // Set radius to 0 and refresh the image if the script is attached
        if (roundedCornersScript != null)
        {
            roundedCornersScript.radius = 0f;
            roundedCornersScript.Refresh();
        }
    }

    void SetMinimize()
    {
        // Restore to original size, position, rotation, scale, anchors, and pivot
        videoImage.rectTransform.sizeDelta = originalSize;
        videoImage.rectTransform.localPosition = originalPosition;
        videoImage.rectTransform.localRotation = originalRotation;
        videoImage.rectTransform.localScale = originalScale;
        videoImage.rectTransform.anchorMin = originalAnchorMin;
        videoImage.rectTransform.anchorMax = originalAnchorMax;
        videoImage.rectTransform.pivot = originalPivot;

        // Restore the original radius and refresh the image if the script is attached
        if (roundedCornersScript != null)
        {
            roundedCornersScript.radius = originalRadius;
            roundedCornersScript.Refresh();
        }
    }
}
