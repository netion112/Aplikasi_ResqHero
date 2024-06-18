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
    private RectTransform canvasRectTransform;
    private float videoAspectRatio;

    void Start()
    {
        fullscreenButton.onClick.AddListener(SetFullscreen);
        minimizeButton.onClick.AddListener(SetMinimize);

        // Save the original size, position, and rotation
        originalSize = videoImage.rectTransform.sizeDelta;
        originalPosition = videoImage.rectTransform.localPosition;
        originalRotation = videoImage.rectTransform.localRotation;

        // Get Canvas RectTransform
        canvasRectTransform = GetComponent<RectTransform>();

        // Wait for the video to prepare and then calculate the aspect ratio
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare();
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        videoAspectRatio = (float)videoPlayer.texture.width / (float)videoPlayer.texture.height;
    }

    void SetFullscreen()
    {
        // Calculate new size to match Main Camera and change aspect ratio to 16:9
        float screenWidth = mainCamera.pixelWidth;
        float screenHeight = mainCamera.pixelHeight;
        float screenAspectRatio = screenWidth / screenHeight;

        if (screenAspectRatio > 16.0f / 9.0f)
        {
            videoImage.rectTransform.sizeDelta = new Vector2(screenWidth, screenWidth * (9.0f / 16.0f));
        }
        else
        {
            videoImage.rectTransform.sizeDelta = new Vector2(screenHeight * (16.0f / 9.0f), screenHeight);
        }

        // Rotate Raw Image
        videoImage.rectTransform.localRotation = Quaternion.Euler(0, 0, -90);
    }

    void SetMinimize()
    {
        // Restore to original size, position, and rotation
        videoImage.rectTransform.sizeDelta = originalSize;
        videoImage.rectTransform.localPosition = originalPosition;
        videoImage.rectTransform.localRotation = originalRotation;
    }
}
