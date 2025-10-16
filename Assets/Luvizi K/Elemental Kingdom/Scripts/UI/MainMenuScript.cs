using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    public void VaoTran()
    {
        SceneManager.LoadScene(1);
    }
    public void RaMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ThoatGame()
    {
        Application.Quit();
    }
}
