using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MouseMovement")]
    [SerializeField] private float MouseSensitivity = 100f;
    [SerializeField] private float xRotation;
    [Header("PlayerMovement")]
    [Tooltip("玩家身體，放在父層")]
    [SerializeField] private Transform playerBody;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float gravity = -12f;
    [SerializeField] private float Jumpforce = 12;
    [Tooltip("角色移動")]
    [SerializeField] private Vector3 velocity;
    [Header("GroundCheck")]
    [Tooltip("檢查地板的射線範圍")]
    [SerializeField] private float radius;
    [SerializeField] private bool isGround;
    [SerializeField] LayerMask GroundLayer;

    [SerializeField] private CharacterController characterController;
    void Start()
    {
        //滑鼠鎖中間，且顯示關閉
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        ViewRotation();
        Movement();
    }
    /// <summary>
    /// 旋轉視角
    /// </summary>
    void ViewRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.unscaledDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
    /// <summary>
    /// 玩家移動
    /// </summary>
    void Movement()
    {
        //讀取輸入資訊
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //移動
        Vector3 move = playerBody.right * x + playerBody.forward * z;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        //重力
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        //檢查地板
        isGround = Physics.Raycast(playerBody.position, Vector3.down, radius, GroundLayer);
        //平時在地面上的重力
        if(isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //重力加速度公式
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
        //扣血
        if(other.gameObject.GetComponent<Target>() != null)
        {
            GameMgr mgr = FindObjectOfType<GameMgr>();
            mgr.healthPoint--;
            mgr.SetHP();
        }
    }
}
