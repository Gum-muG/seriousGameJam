using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assemblies;

public class player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float tiltSpeed = 5f;
    [SerializeField] private float tiltAmount= 20f;
    [SerializeField] private float spinSpeed = 360f;


    private bool playerMoving = false;


    Vector3 tiltVector;
    Vector3 targetTilt;


    [SerializeField] private Transform beyblade_mesh;
    [SerializeField] private Transform spin_empty;
    [SerializeField] private CharacterController characterController;
    private float spinY = 0;
    private float verticalVelocity = 0f;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float jumpForce = 3f;
    private bool grounded = true;


    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashTime = 0.1f;
    private bool dashing = false;
    private float dashTimer = 0f;
    private Vector3 dashDirection;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tiltVector = new Vector3(spin_empty.localEulerAngles.x, spin_empty.localEulerAngles.y, spin_empty.localEulerAngles.z);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
    }

    // Update is called once per frame
    private void Update()
    {
        playerMoving = false;
        Vector2 inputVector = new Vector2(0, 0);
        Vector3 currentTilt = new Vector3(spin_empty.localEulerAngles.x, spin_empty.localEulerAngles.y, spin_empty.localEulerAngles.z);
        spinY += spinSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1;
            playerMoving = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1;
            playerMoving = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y -= 1;
            playerMoving = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
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

            currentTilt.x = Mathf.LerpAngle(currentTilt.x, targetTilt.x, Time.deltaTime * tiltSpeed);
            currentTilt.z = Mathf.LerpAngle(currentTilt.z, targetTilt.z, Time.deltaTime * tiltSpeed);

            spin_empty.localEulerAngles = currentTilt;
        }

        if (!playerMoving)
        {
            targetTilt = new Vector3(tiltVector.x, tiltVector.y, tiltVector.z);

            currentTilt.x = Mathf.LerpAngle(currentTilt.x, targetTilt.x, Time.deltaTime * tiltSpeed);
            currentTilt.z = Mathf.LerpAngle(currentTilt.z, targetTilt.z, Time.deltaTime * tiltSpeed);

            spin_empty.localEulerAngles = currentTilt;
        }

        if (spinY >= 360f)
        {
            spinY -= 360f;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
    
        if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            verticalVelocity = jumpForce;
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing && inputVector != Vector2.zero)
        {
            dashDirection = new Vector3(inputVector.x, 0f, inputVector.y).normalized;
            dashing = true;
            dashTimer = dashTime;
        }

        verticalVelocity -= gravity * Time.deltaTime;

        moveDir = new Vector3(inputVector.x, verticalVelocity, inputVector.y);

        if (dashing)
        {
            transform.position += dashDirection * dashSpeed * Time.deltaTime;

            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0f)
            {
                dashing = false;
            }
        }
        else
        {
            characterController.Move(moveDir * moveSpeed * Time.deltaTime);
        }


        beyblade_mesh.localEulerAngles = new Vector3(beyblade_mesh.localEulerAngles.x, spinY, beyblade_mesh.localEulerAngles.z);

    }
    private void Damage(int damage)
    {
        GameManager.instance.playerHealth.Damage(damage);
        HUD.instance.SetHealth(GameManager.instance.playerHealth.Health);
        spinSpeed = GameManager.instance.playerHealth.Health * 100;
    }
}