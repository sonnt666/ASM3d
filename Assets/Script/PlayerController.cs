using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private float horizontal;
    private float vertical;
    public float rotationSpeed = 5f; // Tốc độ xoay
    private string currentAnim;
    private bool isGround;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private Transform Camera;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        RotateCharacter();

        Jump();

    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (horizontal != 0 || vertical != 0)
        {
            changeAnim("Walk");
            Vector3 move = new Vector3(horizontal, 0, vertical).normalized;
            move = Camera.transform.TransformDirection(move);
            rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);
        }
        else if (isGround)
        {
            rb.velocity = Vector3.zero;
            changeAnim("Idle");
        }

    }

    private void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {

            changeAnim("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }

    }
    void RotateCharacter()
    {


        // Kiểm tra nếu có đầu vào di chuyển
        if (horizontal != 0 || vertical != 0)
        {
            // Tính toán hướng di chuyển
            Vector3 direction = new Vector3(horizontal, 0, vertical);
            Vector3 CurrentLookDirection = Camera.transform.TransformDirection(direction);
            CurrentLookDirection.y = 0;
            CurrentLookDirection.Normalize();
            // Tạo góc quay từ hướng di chuyển
            Quaternion targetRotation = Quaternion.LookRotation(CurrentLookDirection);

            // Xoay nhân vật một cách mượt mà
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    private void changeAnim(string AnimName)
    {
        if (currentAnim != AnimName)
        {
            anim.ResetTrigger(AnimName);
            currentAnim = AnimName;
            anim.SetTrigger(currentAnim);

        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
}
