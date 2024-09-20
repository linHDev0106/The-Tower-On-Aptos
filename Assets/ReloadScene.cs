using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    // This method will reload Scene 0
    public void ReloadSceneZero()
    {
        SceneManager.LoadScene(0); // 0 refers to the scene index in Build Settings
    }
}
