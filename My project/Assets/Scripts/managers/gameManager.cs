using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int currentLevel = 1;

    public HealthComponent playerHealth = new HealthComponent(20, 20);

    [SerializeField] private AudioClip music;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void advanceLevel()
    {
        currentLevel++;
        Debug.Log("Advanced to level " + currentLevel);
    }

    private void Start()
    {
        if (soundManager.instance != null)
        {
            soundManager.instance.playSound(music, transform, 0.05f, true);
        }
    }
}