using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static soundManager instance;

    [SerializeField] private AudioSource soundObject;

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

    public void playSound(AudioClip audioClip, Transform spawn, float volume, bool loop)
    {
        AudioSource audioSource = Instantiate(soundObject, spawn.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.loop = loop;

        audioSource.Play();

        float clipLength = audioSource.clip.length;

        if (!loop) Destroy(audioSource.gameObject, clipLength);
        else DontDestroyOnLoad(audioSource.gameObject);
    }
}
