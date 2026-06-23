using UnityEngine;
using UnityEngine.Assemblies;

public class player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float tiltSpeed = 5f;
    private bool playerMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 tiltVector = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z);
        Vector2 inputVector = new Vector2(0, 0);

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
            transform.localEulerAngles = new Vector3(45f, transform.localEulerAngles.y, transform.localEulerAngles.z);

        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        transform.position += (moveDir * Time.deltaTime * moveSpeed);
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        transform.localEulerAngles = new Vector3(45f, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}