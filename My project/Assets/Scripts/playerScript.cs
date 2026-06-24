using System.Collections.Generic;
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

    [SerializeField] private float gravity = 10f;
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float jumpForce = 3f;


//VECTORS//
    private Vector3 tiltVector;
    private Vector3 targetTilt;
    private Vector3 dashDirection;
    private Vector2 lastInputVector = new Vector2(0, 1);


//BOOLEANS//
    private bool playerMoving = false;
    private bool grounded = true;
    private bool dashing = false;


//VARIABLES//
    private float spinY = 0;
    private float verticalVelocity = 0f;
    private float dashTimer = 0f;
    private float lateralRotationSpeed;
    private float verticalRotationSpeed;
    

//START
    void Start()
    {
        tiltVector = new Vector3(spin_empty.localEulerAngles.x, spin_empty.localEulerAngles.y, spin_empty.localEulerAngles.z);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
    }


//UPDATE
    private void Update(){
    //Variables
        playerMoving = false;
        Vector2 currentInputVector = new Vector2(0, 0);
        Vector3 currentTilt = new Vector3(spin_empty.localEulerAngles.x, spin_empty.localEulerAngles.y, spin_empty.localEulerAngles.z);
        spinY += spinSpeed * Time.deltaTime;
        lateralRotationSpeed = Input.GetAxis("Mouse X");
        verticalRotationSpeed = Input.GetAxis("Mouse Y");


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
            grounded = false;
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
            grounded = true;
        }

        verticalVelocity -= gravity * Time.deltaTime;

        moveDir.y = verticalVelocity;

        if (dashing) {
            characterController.Move(dashDirection * dashSpeed * Time.deltaTime);

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                dashing = false;
            }
        }

        else {
            transform.Rotate(0f, lateralRotationSpeed, 0f);
            characterController.Move(moveDir * moveSpeed * Time.deltaTime);      
        }

        beyblade_mesh.localEulerAngles = new Vector3(beyblade_mesh.localEulerAngles.x, spinY, beyblade_mesh.localEulerAngles.z);

    }
    private void Damage(int damage) {
        GameManager.instance.playerHealth.Damage(damage);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
        spinSpeed = GameManager.instance.playerHealth.Health * 100;
    }
}