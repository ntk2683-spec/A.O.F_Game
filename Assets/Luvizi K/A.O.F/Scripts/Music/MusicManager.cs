using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton: chỉ giữ 1 MusicManager duy nhất
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Không xoá khi load scene mới
            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Destroy(gameObject); // Nếu đã có MusicManager thì xoá cái mới
        }
    }
}
