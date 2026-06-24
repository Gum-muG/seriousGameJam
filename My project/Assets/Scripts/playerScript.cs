using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assemblies;

public class player : MonoBehaviour
{

//PLAYER STATS//
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float spinSpeed = 360f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float jumpForce = 3f;


//VECTORS//
    private Vector3 tiltVector;
    private Vector3 targetTilt;
    private Vector3 dashDirection;


//BOOLEANS//
    private bool playerMoving = false;
    private bool grounded = true;
    private bool dashing = false;


//VARIABLES//
    private float spinY = 0;
    private float verticalVelocity = 0f;
    private float dashTimer = 0f;


//MISC//
    [SerializeField] private Transform beyblade_mesh;
    

//START
    void Start()
    {
        tiltVector = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
    }


//UPDATE
    private void Update(){
    //Variables
        playerMoving = false;
        Vector2 inputVector = new Vector2(0, 0);
        Vector3 currentTilt = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        spinY += spinSpeed * Time.deltaTime;


    //Inputs
        if (Input.GetKey(KeyCode.W)){
            inputVector.y += 1;
            playerMoving = true;
        }

        if (Input.GetKey(KeyCode.A)){
            inputVector.x -= 1;
            playerMoving = true;
        }

        if (Input.GetKey(KeyCode.S)){
            inputVector.y -= 1;
            playerMoving = true;
        }

        if (Input.GetKey(KeyCode.D)){
            inputVector.x += 1;
            playerMoving = true;
        }

         if (Input.GetKeyDown(KeyCode.H)){
            Damage(2);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded){
            verticalVelocity = jumpForce;
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing && inputVector != Vector2.zero){
            dashDirection = new Vector3(inputVector.x, 0f, inputVector.y).normalized;
            dashing = true;
            dashTimer = dashTime;
        }


    //Tilt SLERP
        if (playerMoving){
            targetTilt = new Vector3(tiltVector.x + inputVector.y * 20f, tiltVector.y, tiltVector.z + -inputVector.x * 20f);

            currentTilt.x = Mathf.LerpAngle(currentTilt.x, targetTilt.x, Time.deltaTime * tiltSpeed);
            currentTilt.z = Mathf.LerpAngle(currentTilt.z, targetTilt.z, Time.deltaTime * tiltSpeed);

            transform.localEulerAngles = currentTilt;
        }
        if (!playerMoving){
            targetTilt = new Vector3(tiltVector.x, tiltVector.y, tiltVector.z);

            currentTilt.x = Mathf.LerpAngle(currentTilt.x, targetTilt.x, Time.deltaTime * tiltSpeed);
            currentTilt.z = Mathf.LerpAngle(currentTilt.z, targetTilt.z, Time.deltaTime * tiltSpeed);

            transform.localEulerAngles = currentTilt;
        }

        if (spinY >= 360f){
            spinY -= 360f;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        verticalVelocity -= gravity * Time.deltaTime;

        moveDir = new Vector3(inputVector.x, verticalVelocity, inputVector.y);

        if (dashing){
            transform.position += dashDirection * dashSpeed * Time.deltaTime;

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                dashing = false;
            }
        }
        else{
            transform.position += (moveDir * Time.deltaTime * moveSpeed);
        }


        if (transform.position.y <= 0f){
            Vector3 currentPosition = transform.position;
            currentPosition.y = 0f;
            transform.position = currentPosition;

            verticalVelocity = 0f;
            grounded = true;
        }

        beyblade_mesh.localEulerAngles = new Vector3(beyblade_mesh.localEulerAngles.x, spinY, beyblade_mesh.localEulerAngles.z);

    }
    private void Damage(int damage){
        GameManager.instance.playerHealth.Damage(damage);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
    }
}