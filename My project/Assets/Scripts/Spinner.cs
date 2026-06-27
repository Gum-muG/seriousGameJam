using UnityEngine;

public class Spinner : MonoBehaviour
{
    private int playerLayer;
    [SerializeField] AudioClip sfx;
    [SerializeField] AudioClip healAmt;
    [SerializeField] AudioClip color;

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            player playerRef = other.gameObject.GetComponentInParent<player>();
            playerCombat combat = playerRef.GetComponent<playerCombat>();
            combat.Heal(10);
            soundManager.instance.playSound(sfx, transform, .5f, false);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1));
    }
}
