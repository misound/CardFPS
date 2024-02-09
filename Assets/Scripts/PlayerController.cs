using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float MouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float gravity = -12f;
    [SerializeField] private float xRotation;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float radius;
    [SerializeField] private bool isGround;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] private float Jumpforce = 12;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        ViewRotation();
        Movement();
    }

    void ViewRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.unscaledDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerBody.right * x + playerBody.forward * z;

        characterController.Move(move * moveSpeed * Time.deltaTime);


        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        isGround = Physics.Raycast(playerBody.position, Vector3.down, radius, GroundLayer);

        if(isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(Jumpforce * -2f * gravity);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(playerBody.position, Vector3.down * radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Target>() != null)
        {
            GameMgr mgr = FindObjectOfType<GameMgr>();
            mgr.healthPoint--;
            mgr.SetHP();
        }
    }
}
