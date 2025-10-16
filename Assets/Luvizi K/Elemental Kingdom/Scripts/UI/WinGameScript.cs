using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGameScript : MonoBehaviour
{
    public static WinGameScript Instance;  // singleton

    private void Awake()
    {
        // Đảm bảo chỉ có 1 instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // không bị destroy khi load scene khác
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void WinGame()
    {
        // Load scene Win (ví dụ scene index = 2)
        SceneManager.LoadScene(2);

        // Hoặc nếu bạn muốn show UI thay vì load scene thì có thể bật UI win ở đây
        // winUI.SetActive(true);
    }
}
