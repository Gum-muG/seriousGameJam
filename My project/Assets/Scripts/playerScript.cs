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
    private Vector2 lastInputVector = new Vector2(0, 0);


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
        Vector2 currentInputVector = new Vector2(0, 0);
        Vector3 currentTilt = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        spinY += spinSpeed * Time.deltaTime;


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
        if (Input.GetKeyDown(KeyCode.Space) && grounded){
            verticalVelocity = jumpForce;
            grounded = false;
        }

//dash and dive
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing){
            dashDirection = new Vector3(lastInputVector.x, 0f, lastInputVector.y).normalized;
            dashing = true;
            dashTimer = dashTime;

            if (grounded == false){
                float diveDownForce = -1.5f;
                dashDirection = new Vector3(lastInputVector.x, diveDownForce, lastInputVector.y).normalized;
            }
        }


    //Tilt SLERP
        if (playerMoving){
            targetTilt = new Vector3(tiltVector.x + currentInputVector.y * 20f, tiltVector.y, tiltVector.z + -currentInputVector.x * 20f);

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

        currentInputVector = currentInputVector.normalized;

        Vector3 moveDir = new Vector3(currentInputVector.x, 0f, currentInputVector.y);

        verticalVelocity -= gravity * Time.deltaTime;

        moveDir = new Vector3(currentInputVector.x, verticalVelocity, currentInputVector.y);

        if (dashing){
            transform.position += dashDirection * dashSpeed * Time.deltaTime;

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                dashing = false;
            }
        }
        else{
            transform.position += (moveDir * moveSpeed * Time.deltaTime);
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