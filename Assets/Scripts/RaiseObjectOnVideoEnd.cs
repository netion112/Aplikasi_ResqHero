using UnityEngine;
using UnityEngine.Video;

public class RaiseObjectOnVideoEnd : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Referensi ke VideoPlayer
    public GameObject objectToMove; // Referensi ke game object yang ingin dipindahkan
    public float moveAmount = 1.0f; // Besar perpindahan pada sumbu Y
    public float returnSpeed = 0.5f; // Kecepatan pengembalian posisi

    private Vector3 originalPosition; // Posisi awal game object
    private bool shouldReturn = false; // Apakah harus mengembalikan posisi

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.started += OnVideoStart;
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("VideoPlayer is not assigned.");
        }

        if (objectToMove == null)
        {
            objectToMove = gameObject; // Default to the attached game object if none specified
        }

        originalPosition = objectToMove.transform.position; // Simpan posisi awal
    }

    private void Update()
    {
        if (shouldReturn)
        {
            // Kembalikan posisi game object perlahan ke posisi semula
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, originalPosition, returnSpeed * Time.deltaTime);

            // Jika posisinya sudah hampir sama dengan posisi semula, hentikan pengembalian
            if (Vector3.Distance(objectToMove.transform.position, originalPosition) < 0.01f)
            {
                objectToMove.transform.position = originalPosition;
                shouldReturn = false;
            }
        }
    }

    private void OnVideoStart(VideoPlayer vp)
    {
        // Turunkan posisi game object pada sumbu Y
        Vector3 newPosition = objectToMove.transform.position;
        newPosition.y -= moveAmount;
        objectToMove.transform.position = newPosition;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Set flag untuk mengembalikan posisi game object ke posisi semula
        shouldReturn = true;
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.started -= OnVideoStart;
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}