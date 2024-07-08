using UnityEngine;

public class OpenWebsite : MonoBehaviour
{
    // Fungsi untuk membuka website dengan URL yang berbeda
    public void OpenURLFeedback()
    {
        Application.OpenURL("https://forms.gle/dt4YWeBaFrZSeiS2A");
    }

    public void OpenURLDonasi()
    {
        Application.OpenURL("https://saweria.co/resqhero");
    }

}
