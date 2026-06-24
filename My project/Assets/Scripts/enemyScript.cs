using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assemblies;

public class enemy : MonoBehaviour
{

    public NavMeshAgent agent;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool inSightRange, inAttackRange;


//PLAYER STATS//
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float tiltAmount= 20f;
    [SerializeField] public float spinSpeed = 360f;


//REFERENCES//
    [SerializeField] private Transform beyblade_mesh;
    [SerializeField] private Transform spin_empty;
    [SerializeField] private Transform player;
    [SerializeField] private MeshCollider collision;

    [SerializeField] private float gravity = 10f;
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float bounceDecay = 5f;


//VECTORS//
    private Vector3 tiltVector;
    private Vector3 targetTilt;
    private Vector3 dashDirection;
    private Vector2 lastInputVector = new Vector2(0, 0);
    public Vector3 currentPlayerVelocity;
    private Vector3 bounceVelocity;


//BOOLEANS//
    private bool playerMoving = false;
    private bool grounded = true;
    private bool dashing = false;


//VARIABLES//
    private float spinY = 0;
    private float verticalVelocity = 0f;
    private float dashTimer = 0f;

    //LAYERS//
    private int wallLayer;
    private int enemyLayer;
    private int groundLayer;
    private int playerLayer;
    

//START
    void Start()
    {
        tiltVector = new Vector3(spin_empty.localEulerAngles.x, spin_empty.localEulerAngles.y, spin_empty.localEulerAngles.z);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
        //Assigning layerVariables
        wallLayer = LayerMask.NameToLayer("Wall");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        groundLayer = LayerMask.NameToLayer("Ground");
        playerLayer = LayerMask.NameToLayer("Player");
    }


//UPDATE
    private void Update(){
    //Variables
        Vector2 currentInputVector = agent.velocity.normalized;
        Vector3 currentTilt = new Vector3(spin_empty.localEulerAngles.x, spin_empty.localEulerAngles.y, spin_empty.localEulerAngles.z);
        spinY += spinSpeed * Time.deltaTime;

    //Tilt SLERP
        if (agent.velocity.magnitude > 0){
            targetTilt = new Vector3(tiltVector.x + currentInputVector.magnitude * 20f, tiltVector.y, tiltVector.z);

            currentTilt.x = Mathf.LerpAngle(currentTilt.x, targetTilt.x, Time.deltaTime * tiltSpeed);
            currentTilt.z = Mathf.LerpAngle(currentTilt.z, targetTilt.z, Time.deltaTime * tiltSpeed);

            spin_empty.localEulerAngles = currentTilt;
        }
        else {
            targetTilt = new Vector3(tiltVector.x, tiltVector.y, tiltVector.z);

            currentTilt.x = Mathf.LerpAngle(currentTilt.x, targetTilt.x, Time.deltaTime * tiltSpeed);
            currentTilt.z = Mathf.LerpAngle(currentTilt.z, targetTilt.z, Time.deltaTime * tiltSpeed);

            spin_empty.localEulerAngles = currentTilt;
        }

        if (spinY >= 360f){
            spinY -= 360f;
        }

        currentInputVector = currentInputVector.normalized;

        Vector3 moveDir = new Vector3(currentInputVector.x, 0f, currentInputVector.y);
        

        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing && currentInputVector != Vector2.zero)
        {
            dashDirection = new Vector3(currentInputVector.x, 0f, currentInputVector.y).normalized;
            dashing = true;
            dashTimer = dashTime;
        }

        verticalVelocity -= gravity * Time.deltaTime;

        moveDir = new Vector3(currentInputVector.x, verticalVelocity, currentInputVector.y);



        if (spinSpeed > 0)
        {
            beyblade_mesh.localEulerAngles = new Vector3(-90 + (40/math.pow(GameManager.instance.playerHealth.Health, 1.5f)), spinY, beyblade_mesh.localEulerAngles.z);
        }

        bounceVelocity = Vector3.Lerp(bounceVelocity, Vector3.zero, Time.deltaTime * bounceDecay);
        

        inSightRange = Physics.CheckSphere(transform.position, sightRange, 1<<playerLayer);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, 1<<playerLayer);

        if (inSightRange) ChasePlayer(); 
        else playerMoving = false;
        agent.Move(-bounceVelocity/200);
    }
    public void KnockBack(Vector3 bounceVelocity)
    {
        this.bounceVelocity = bounceVelocity;
    }
    private void Damage(int damage) {
        GameManager.instance.playerHealth.Damage(damage);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
        spinSpeed = GameManager.instance.playerHealth.Health * 200;
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        playerMoving = true;
    }
    private void AttackPlayer()
    {
        alreadyAttacked = true;
        
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}