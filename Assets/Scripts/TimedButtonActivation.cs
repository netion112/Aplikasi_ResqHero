using UnityEngine;
using UnityEngine.UI;

public class TimedButtonActivation : MonoBehaviour
{
    [SerializeField]
    private Button activationButton; // The button that triggers the activation

    [SerializeField]
    private GameObject targetObject; // The GameObject to be activated

    [SerializeField]
    private Button pauseButton; // The pause button inside the targetObject

    [SerializeField]
    private Button playButton; // The play button inside the targetObject

    private bool isPaused = false;
    private float activationDuration = 3.0f;
    private float timer = 0.0f;

    void Start()
    {
        if (activationButton != null)
        {
            activationButton.onClick.AddListener(ActivateObject);
        }

        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(PauseTimer);
        }

        if (playButton != null)
        {
            playButton.onClick.AddListener(ResumeTimer);
        }

        targetObject.SetActive(false); // Ensure the targetObject is initially inactive
    }

    void Update()
    {
        if (targetObject.activeSelf && !isPaused)
        {
            timer += Time.deltaTime;
            if (timer >= activationDuration)
            {
                DeactivateObject();
            }
        }
    }

    private void ActivateObject()
    {
        targetObject.SetActive(true);
        timer = 0.0f;
        isPaused = false; // Ensure timer is not paused when activated
    }

    private void DeactivateObject()
    {
        targetObject.SetActive(false);
        timer = 0.0f;
    }

    private void PauseTimer()
    {
        isPaused = true;
    }

    private void ResumeTimer()
    {
        isPaused = false;
    }
}
