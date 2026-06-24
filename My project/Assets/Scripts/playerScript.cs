using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assemblies;

public class player : MonoBehaviour
{

//PLAYER STATS//

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float spinSpeed = 360f;
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashTime = 0.1f;
    [SerializeField] private float jumpForce = 3f;


//VECTORS//
    private Vector3 tiltVector;
    private Vector3 targetTilt;
    private Vector3 dashDirection;


//BOOLEANS//
    private bool playerMoving = false;
    private bool grounded = false;
    private bool dashing = false;


//MISC//
    [SerializeField] private Transform beyblade_mesh;
    private float spinY = 0;
    private float dashTimer = 0f;
    private Rigidbody rb;


    void Start()
    {
        tiltVector = transform.localEulerAngles;
        rb = GetComponent<Rigidbody>();
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
    }


    private void Update()
    {
    //VARIABLES

        playerMoving = false;
        Vector2 inputVector = Vector2.zero;
        Vector3 currentTilt = transform.localEulerAngles;
        spinY += spinSpeed * Time.deltaTime;


    //Inputs
//w
        if (Input.GetKey(KeyCode.W)){
            inputVector.y += 1;
            playerMoving = true;
        }
//a
        if (Input.GetKey(KeyCode.A)){
            inputVector.x -= 1;
            playerMoving = true;
        }
//s
        if (Input.GetKey(KeyCode.S)){
            inputVector.y -= 1;
            playerMoving = true;
        }
//d
        if (Input.GetKey(KeyCode.D)){
            inputVector.x += 1;
            playerMoving = true;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Damage(2);
        }

        if (playerMoving)
        {
            targetTilt = new Vector3(tiltVector.x + inputVector.y * 20f, tiltVector.y, tiltVector.z + -inputVector.x * 20f);
        }
        inputVector = inputVector.normalized;

//jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

//dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing && inputVector != Vector2.zero){
            dashDirection = new Vector3(inputVector.x, 0f, inputVector.y).normalized;
            dashing = true;
            dashTimer = dashTime;
        }


        //SLERP tilt

        if (playerMoving){
            targetTilt = new Vector3(
                tiltVector.x + inputVector.y * 20f,
                tiltVector.y,
                tiltVector.z - inputVector.x * 20f);

            currentTilt.x = Mathf.LerpAngle(
                currentTilt.x,
                targetTilt.x,
                Time.deltaTime * tiltSpeed);

            currentTilt.z = Mathf.LerpAngle(
                currentTilt.z,
                targetTilt.z,
                Time.deltaTime * tiltSpeed);

            transform.localEulerAngles = currentTilt;
        }

        if (!playerMoving){
            targetTilt = tiltVector;

            currentTilt.x = Mathf.LerpAngle(currentTilt.x, targetTilt.x, Time.deltaTime * tiltSpeed);

            currentTilt.z = Mathf.LerpAngle(currentTilt.z, targetTilt.z, Time.deltaTime * tiltSpeed);

            transform.localEulerAngles = currentTilt;
        }

        if (spinY >= 360f)
        {
            spinY -= 360f;
        }

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        Vector3 velocity = rb.linearVelocity;

        velocity.x = moveDir.x * moveSpeed;
        velocity.z = moveDir.z * moveSpeed;

        rb.linearVelocity = velocity;

        if (dashing){
            rb.AddForce(
                dashDirection * dashSpeed,
                ForceMode.Impulse);

            dashing = false;
        }

        beyblade_mesh.localEulerAngles = new Vector3(
            beyblade_mesh.localEulerAngles.x,
            spinY,
            beyblade_mesh.localEulerAngles.z);

        Debug.Log(moveSpeed);
    }

    private void OnCollisionStay(Collision collision){
        grounded = true;
    }

    private void OnCollisionExit(Collision collision){
        grounded = false;
    }

    private void Damage(int damage){
        GameManager.instance.playerHealth.Damage(damage);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
    }
}