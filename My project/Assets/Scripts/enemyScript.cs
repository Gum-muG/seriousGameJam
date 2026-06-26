using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public NavMeshAgent agent;

    public float sightRange, attackRange;
    public bool inSightRange, inAttackRange;

    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] public float spinSpeed = 360f;

    [SerializeField] private Transform beyblade_mesh;
    [SerializeField] private Transform spin_empty;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private MeshCollider collision;
    [SerializeField] private Transform canvas;
    [SerializeField] private EnemyHealth canvasScript;

    [SerializeField] private float gravity = 10f;
    [SerializeField] private float bounceDecay = 5f;

    private Vector3 tiltVector;
    private Vector3 targetTilt;
    public Vector3 currentPlayerVelocity;
    private Vector3 bounceVelocity;

    private bool playerMoving = false;

    private float spinY = 0;
    private float verticalVelocity = 0f;

    private HealthComponent health = new HealthComponent(10, 10);

    private int wallLayer;
    private int enemyLayer;
    private int groundLayer;
    private int playerLayer;

    private bool wobbled;
    private float wobbleTimer;
    private int wobbleDamage;
    private bool wobbleWait;

    private void Start()
    {
        findPlayer();
        findPlayerCamera();

        tiltVector = new Vector3(
            spin_empty.localEulerAngles.x,
            spin_empty.localEulerAngles.y,
            spin_empty.localEulerAngles.z
        );

        if (HUD.instance != null && GameManager.instance != null)
        {
            HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
        }

        wallLayer = LayerMask.NameToLayer("Wall");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        groundLayer = LayerMask.NameToLayer("Ground");
        playerLayer = LayerMask.NameToLayer("Player");

        if (canvasScript != null)
        {
            canvasScript.setHealth(health.Health);
        }
    }

    private void Update()
    {
        Vector2 currentInputVector = agent.velocity.normalized;
        Vector3 currentTilt = new Vector3(
            spin_empty.localEulerAngles.x,
            spin_empty.localEulerAngles.y,
            spin_empty.localEulerAngles.z
        );

        spinY += spinSpeed * Time.deltaTime;

        if (agent.velocity.magnitude > 0)
        {
            targetTilt = new Vector3(
                tiltVector.x + currentInputVector.magnitude * 20f,
                tiltVector.y,
                tiltVector.z
            );
        }
        else
        {
            targetTilt = new Vector3(tiltVector.x, tiltVector.y, tiltVector.z);
        }

        currentTilt.x = Mathf.LerpAngle(currentTilt.x, targetTilt.x, Time.deltaTime * tiltSpeed);
        currentTilt.z = Mathf.LerpAngle(currentTilt.z, targetTilt.z, Time.deltaTime * tiltSpeed);
        spin_empty.localEulerAngles = currentTilt;

        if (spinY >= 360f)
        {
            spinY -= 360f;
        }

        verticalVelocity -= gravity * Time.deltaTime;

        updateHealthCanvas();

        spinSpeed = health.Health * 200;

        if (health.Health > 0)
        {
            beyblade_mesh.localEulerAngles = new Vector3(
                40 / Mathf.Pow(health.Health, 1.5f),
                spinY,
                beyblade_mesh.localEulerAngles.z
            );
        }

        bounceVelocity = Vector3.Lerp(bounceVelocity, Vector3.zero, Time.deltaTime * bounceDecay);

        inSightRange = Physics.CheckSphere(transform.position, sightRange, 1 << playerLayer);

        if (inSightRange)
        {
            ChasePlayer();
        }
        else
        {
            playerMoving = false;
        }

        agent.Move(-bounceVelocity / 200);

        handleWobble();
    }

    private void findPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void findPlayerCamera()
    {
        Camera cam = Camera.main;

        if (cam != null)
        {
            playerCamera = cam.transform;
        }
    }

    private void updateHealthCanvas()
    {
        if (playerCamera == null)
        {
            findPlayerCamera();
        }

        if (playerCamera == null || canvas == null)
            return;

        canvas.LookAt(playerCamera.position);
        canvas.Rotate(0, 180, 0);
    }

    private void ChasePlayer()
    {
        if (player == null)
        {
            findPlayer();
        }

        if (player == null)
            return;

        agent.SetDestination(player.position);
        playerMoving = true;
    }

    private void handleWobble()
    {
        if (!wobbled)
            return;

        wobbleTimer -= Time.deltaTime;

        if (!wobbleWait)
        {
            wobbleWait = true;
            Damage(wobbleDamage);
            Invoke("resetWobbleWait", 1f);
        }

        if (wobbleTimer <= 0f)
        {
            wobbled = false;
        }
    }

    private void Die()
    {
        Destroy(gameObject);

        if (levelRewardManager.instance != null)
        {
            levelRewardManager.instance.checkLevelCleared();
        }
    }

    public void Damage(int damage)
    {
        health.Damage(damage);

        if (canvasScript != null)
        {
            canvasScript.setHealth(health.Health);
        }

        if (health.Health <= 0)
        {
            Die();
        }
    }

    public void Damage(Vector3 bounceVelocity, int damage)
    {
        this.bounceVelocity = bounceVelocity;

        health.Damage(damage);

        if (canvasScript != null)
        {
            canvasScript.setHealth(health.Health);
        }

        if (health.Health <= 0)
        {
            Die();
        }
    }

    public void applyWobble(float duration, int damagePerTick)
    {
        wobbled = true;
        wobbleTimer = duration;
        wobbleDamage = damagePerTick;
    }

    private void resetWobbleWait()
    {
        wobbleWait = false;
    }
}