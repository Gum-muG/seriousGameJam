using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public HealthComponent playerHealth = new HealthComponent(20, 20);

    [SerializeField] private AudioClip music;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (soundManager.instance != null)
        {
            soundManager.instance.playSound(music, transform, 0.05f, true);
        }
    }
}