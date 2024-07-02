using UnityEngine;
using UnityEngine.Video;

public class VideoComplete : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Assign the VideoPlayer component in the Inspector
    public GameObject objectToActivate;  // Assign the GameObject you want to activate in the Inspector

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;  // Subscribe to the video end event
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);  // Activate the game object when the video ends
        }
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;  // Unsubscribe from the event when the script is destroyed
        }
    }
}
