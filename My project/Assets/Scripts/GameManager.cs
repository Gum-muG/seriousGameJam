using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    public HealthComponent playerHealth = new HealthComponent(20, 20);

    [SerializeField] AudioClip music;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        soundManager.instance.playSound(music, transform, .05f, true);
    }
}
