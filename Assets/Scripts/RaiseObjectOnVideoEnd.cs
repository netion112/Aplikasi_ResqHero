using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class RaiseObjectOnVideoEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Referensi ke VideoPlayer
    public GameObject objectToRaise; // Referensi ke game object yang ingin dinaikkan
    public float raiseAmount = 1.0f; // Besar kenaikan pada sumbu Y
    public float raiseSpeed = 1.0f; // Kecepatan kenaikan

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd; // Berlangganan ke event video end
        }
        else
        {
            Debug.LogError("VideoPlayer is not assigned.");
        }

        if (objectToRaise == null)
        {
            objectToRaise = gameObject; // Default ke game object terlampir jika tidak ada yang ditentukan
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(RaiseObjectCoroutine()); // Memulai Coroutine untuk menaikkan objek
    }

    private IEnumerator RaiseObjectCoroutine()
    {
        float initialY = objectToRaise.transform.position.y;
        float targetY = initialY + raiseAmount;
        float elapsedTime = 0f;

        while (objectToRaise.transform.position.y < targetY)
        {
            elapsedTime += Time.deltaTime * raiseSpeed;
            float newY = Mathf.Lerp(initialY, targetY, elapsedTime);
            objectToRaise.transform.position = new Vector3(objectToRaise.transform.position.x, newY, objectToRaise.transform.position.z);
            yield return null;
        }

        // Pastikan posisi akhir benar-benar tepat
        objectToRaise.transform.position = new Vector3(objectToRaise.transform.position.x, targetY, objectToRaise.transform.position.z);
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd; // Berhenti berlangganan untuk mencegah memory leaks
        }
    }
}
