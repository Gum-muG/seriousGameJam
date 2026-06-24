using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assemblies;

public class player : MonoBehaviour
{

//PLAYER STATS//
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float tiltAmount= 20f;
    [SerializeField] private float spinSpeed = 360f;


//REFERENCES//
    [SerializeField] private Transform beyblade_mesh;
    [SerializeField] private Transform spin_empty;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Transform playerCameraEmpty;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private MeshCollider collision;

    [SerializeField] private float gravity = 10f;
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float sensitivity = 3f;
    [SerializeField] private float cameraDistance = -9f;
    [SerializeField] private float spinImpactMultiplier = 0.02f;
    [SerializeField] private float bounceDecay = 5f;
    [SerializeField] private float bounceIntensity = .5f;
    [SerializeField] private float bounceHeight = 2f;


//VECTORS//
    private Vector3 tiltVector;
    private Vector3 targetTilt;
    private Vector3 dashDirection;
    private Vector3 currentPlayerVelocity;
    private Vector3 bounceVelocity;
    private Vector2 lastInputVector = new Vector2(0, 1);
    private Vector3 startingPosition;


//BOOLEANS//
    private bool playerMoving = false;
    private bool dashing = false;
    private bool playerAlive;


//VARIABLES//
    private float spinY = 0;
    private float verticalVelocity = 0f;
    private float dashTimer = 0f;
    private float lateralRotationSpeed;
    private float verticalRotationSpeed;
    private float cameraRotation = 0f;
    private Ray ray;
    
//LAYERS//
    private int wallLayer;
    private int enemyLayer;
    private int groundLayer;
    private int playerLayer;

//START
    void Start()
    {
        startingPosition = transform.position;
        tiltVector = new Vector3(spin_empty.localEulerAngles.x, spin_empty.localEulerAngles.y, spin_empty.localEulerAngles.z);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    //Assigning layerVariables
        wallLayer = LayerMask.NameToLayer("Wall");
        enemyLayer = LayerMask.NameToLayer("Enemy");
        groundLayer = LayerMask.NameToLayer("Ground");
        playerLayer = LayerMask.NameToLayer("Player");

    //Setting bools
        playerAlive = true;
    }


//UPDATE
    private void Update(){
    //Variables
        playerMoving = false;
        Vector2 currentInputVector = new Vector2(0, 0);
        Vector3 currentTilt = new Vector3(spin_empty.localEulerAngles.x, spin_empty.localEulerAngles.y, spin_empty.localEulerAngles.z);
        spinY += spinSpeed * Time.deltaTime;
        lateralRotationSpeed = Input.GetAxis("Mouse X") * sensitivity;
        verticalRotationSpeed = Input.GetAxis("Mouse Y") * sensitivity;


//Inputs

//w
        if (Input.GetKey(KeyCode.W) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))){
            currentInputVector.y += 1;
            lastInputVector.y = 1;
            lastInputVector.x = 0;
            playerMoving = true;
        }

//a
        if (Input.GetKey(KeyCode.A) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))){
            currentInputVector.x -= 1;
            lastInputVector.x = -1;
            lastInputVector.y = 0;
            playerMoving = true;
        }

//s
        if (Input.GetKey(KeyCode.S) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))){
            currentInputVector.y -= 1;
            lastInputVector.y = -1;
            lastInputVector.x = 0;
            playerMoving = true;
        }

//d
        if (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))){
            currentInputVector.x += 1;
            lastInputVector.x = 1;
            lastInputVector.y = 0;
            playerMoving = true;
        }

//up-left
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)){
            currentInputVector.y += 1;
            currentInputVector.x -= 1;
            lastInputVector.y = 1;
            lastInputVector.x = -1;
            playerMoving = true;
        }

//up-right
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)){
            currentInputVector.y += 1;
            currentInputVector.x += 1;
            lastInputVector.y = 1;
            lastInputVector.x = 1;
            playerMoving = true;
        }

//down-left
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)){
            currentInputVector.y -= 1;
            currentInputVector.x -= 1;
            lastInputVector.y = -1;
            lastInputVector.x = -1;
            playerMoving = true;
        }

//down-right
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)){
            currentInputVector.y -= 1;
            currentInputVector.x += 1;
            lastInputVector.y = -1;
            lastInputVector.x = 1;
            playerMoving = true;
        }


//self-damage
         if (Input.GetKeyDown(KeyCode.H)){
            Damage(2);
        }

//jump
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded){
            verticalVelocity = jumpForce;
        }

//dash and dive
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing){
            dashDirection =
                transform.forward * lastInputVector.y +
                transform.right * lastInputVector.x;

            dashDirection.y = 0f;
            dashDirection.Normalize();

            dashing = true;
            dashTimer = dashTime;

            if (!characterController.isGrounded){
                float diveDownForce = -1.5f;

                dashDirection =
                    transform.forward * lastInputVector.y +
                    transform.right * lastInputVector.x;

                dashDirection.y = diveDownForce;
                dashDirection.Normalize();
            }
        }


    //Tilt SLERP
        if (playerMoving){
            targetTilt = new Vector3(tiltVector.x + currentInputVector.y * tiltAmount, tiltVector.y, tiltVector.z + -currentInputVector.x * tiltAmount);

            currentTilt.x = Mathf.LerpAngle(currentTilt.x, targetTilt.x, Time.deltaTime * tiltSpeed);
            currentTilt.z = Mathf.LerpAngle(currentTilt.z, targetTilt.z, Time.deltaTime * tiltSpeed);

            spin_empty.localEulerAngles = currentTilt;
        }
        if (!playerMoving){
            targetTilt = new Vector3(tiltVector.x, tiltVector.y, tiltVector.z);

            currentTilt.x = Mathf.LerpAngle(currentTilt.x, targetTilt.x, Time.deltaTime * tiltSpeed);
            currentTilt.z = Mathf.LerpAngle(currentTilt.z, targetTilt.z, Time.deltaTime * tiltSpeed);


            spin_empty.localEulerAngles = currentTilt;
        }

        if (spinY >= 360f){
            spinY -= 360f;
        }

        currentInputVector = currentInputVector.normalized;

        Vector3 moveDir =
            transform.forward * currentInputVector.y +
            transform.right * currentInputVector.x;

        moveDir.y = 0f;
        moveDir.Normalize();

        if (characterController.isGrounded && verticalVelocity < 0f){
            verticalVelocity = -1f;
        }

        verticalVelocity -= gravity * Time.deltaTime;

        moveDir.y = verticalVelocity;


    //Dashing and Player Movement
        if (dashing) {
            currentPlayerVelocity = dashDirection * dashSpeed;
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                dashing = false;
            }
        }
        else {
            transform.Rotate(0f, lateralRotationSpeed, 0f);

            cameraRotation -= verticalRotationSpeed;
            cameraRotation = Mathf.Clamp(cameraRotation, -80f, 80f);

            ray = new Ray(playerCameraEmpty.position, -playerCamera.forward);
            if (Physics.SphereCast(ray, 0.2f, out RaycastHit hit, math.abs(cameraDistance), (1<<groundLayer)|(1<<wallLayer)))
            {
                playerCamera.localPosition = new Vector3(0,0,-hit.distance);
            }
            else
            {
                playerCamera.localPosition = new Vector3(0, 0, cameraDistance);
            }

            playerCameraEmpty.localEulerAngles = new Vector3(cameraRotation, 0f, 0f);

            currentPlayerVelocity = moveDir * moveSpeed;
        }

        bounceVelocity = Vector3.Lerp(bounceVelocity, Vector3.zero, Time.deltaTime * bounceDecay);

        Vector3 finalVelocity = currentPlayerVelocity + bounceVelocity;
        characterController.Move(finalVelocity * Time.deltaTime);
        spinSpeed = GameManager.instance.playerHealth.Health * 200;
    //Mesh Rotation
        if (spinSpeed > 0)
        {
            beyblade_mesh.localEulerAngles = new Vector3(-90 + (40 / Mathf.Pow(GameManager.instance.playerHealth.Health, 1.5f)), spinY, beyblade_mesh.localEulerAngles.z);
        }
    }


    //Bounce
    //Bounce
    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.layer == enemyLayer)
            {
                enemy enemyPlayer = other.gameObject.GetComponentInParent<enemy>();
                
                if (enemyPlayer != null)
                {
                    float collisionMoveSpeed = currentPlayerVelocity.magnitude + enemyPlayer.currentPlayerVelocity.magnitude;
                    float collisionSpinSpeed = spinSpeed * spinImpactMultiplier + enemyPlayer.spinSpeed * spinImpactMultiplier;
                    float collisionStrength = collisionMoveSpeed + collisionSpinSpeed;

                    Vector3 bounceDirection = transform.position - enemyPlayer.transform.position;

                    
                    if (!dashing) bounceDirection.y = 0f;
                    bounceDirection.Normalize();
                    if (dashing) bounceDirection.y *= bounceHeight;

                    bounceVelocity = bounceDirection * collisionStrength * bounceIntensity;
                    bounceDirection.y = 0f;
                    enemyPlayer.Damage(bounceVelocity, 2);
                    Damage(2);
                    dashing = false;
                }
                
            }
            else if (other.gameObject.layer == wallLayer)
            {
                float collisionMoveSpeed = currentPlayerVelocity.magnitude;
                float collisionSpinSpeed = spinSpeed * spinImpactMultiplier;
                float collisionStrength = collisionMoveSpeed + collisionSpinSpeed;

                ray = new Ray(collision.transform.position, (other.transform.position - collision.transform.position).normalized);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    
                    Vector3 bounceDirection = hit.normal;
                    bounceDirection.y = 0f;
                    bounceDirection.Normalize();

                    bounceVelocity = bounceDirection * collisionStrength;
                    Damage(2);
                    dashing = false;
                }
            }

            

    }
            

    private void respawnPlayer()
    {
        respawnManager.instance.triggerRespawnScreen(startingPosition);
    }

    private void Damage(int damage) {
        GameManager.instance.playerHealth.Damage(damage);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
        if (GameManager.instance.playerHealth.Health == 0)
        {
            respawnPlayer();
        } 
    }

  
}
