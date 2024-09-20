using UnityEngine;

public class OpenWebsiteButton : MonoBehaviour
{
    // The URL you want to open
    private string url = "https://www.aptosfaucet.com/";

    // This method will be called when the button is clicked
    public void OpenAptosFaucet()
    {
        Application.OpenURL(url);
    }
}
