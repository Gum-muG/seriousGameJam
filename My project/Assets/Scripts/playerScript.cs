using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assemblies;

public class player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float tiltSpeed = 5f;
    private bool playerMoving = false;
    Vector3 tiltVector;
    Vector3 targetTilt;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tiltVector = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    // Update is called once per frame
    private void Update()
    {
        playerMoving = false;
        Vector2 inputVector = new Vector2(0, 0);
        Vector3 currentTilt = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);

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
        if (playerMoving)
        {
            targetTilt = new Vector3(tiltVector.x + inputVector.y * 20f, tiltVector.y, tiltVector.z + -inputVector.x * 20f);
            currentTilt = Quaternion.Slerp(Quaternion.Euler(currentTilt), Quaternion.Euler(targetTilt), Time.deltaTime * tiltSpeed).eulerAngles;
            Debug.Log(tiltVector);
            transform.localEulerAngles = currentTilt;
        }
        if (!playerMoving)
        {
            targetTilt = new Vector3(tiltVector.x, tiltVector.y, tiltVector.z);
            currentTilt = Quaternion.Slerp(Quaternion.Euler(currentTilt), Quaternion.Euler(targetTilt), Time.deltaTime * tiltSpeed).eulerAngles;
            transform.localEulerAngles = currentTilt;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        transform.position += (moveDir * Time.deltaTime * moveSpeed);
    }
    private void Damage(int damage)
    {
        GameManager.instance.playerHealth.Damage(damage);
    }
}