using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ChooseCharacter()
    {
        SceneManager.LoadScene(2);
    }
    public void ChooseWeapon()
    {
        SceneManager.LoadScene(3);
    }
    public void ChooseSpecialSkill()
    {
        SceneManager.LoadScene(4);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}