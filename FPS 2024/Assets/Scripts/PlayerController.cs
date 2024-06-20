using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    PlayerGravity playerGravity;
    [SerializeField] Transform camTransform;
    Weapon weapon;
    Vector3 direction, camDirection;

    const float speedbase = 5;
    float speed;
    bool run;

    float verticalRotation;
    bool shooting, crouching;

    [SerializeField] GameObject grenadePrefab;
    [SerializeField] Transform throwPoint;
    float throwForceUP = 5;
    float throwForceForward = 5;
   
    [SerializeField]
    [Range(1, 500)]
    float mouseSensitivity;

    private void Awake()
    {
        speed = speedbase;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        camTransform = transform.GetChild(0).GetComponent<Transform>();
        playerGravity = GetComponent<PlayerGravity>();
        weapon = GetComponentInChildren<Weapon>();
    }

    private void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        camDirection.x = Input.GetAxis("Mouse X");
        camDirection.y = Input.GetAxis("Mouse Y");


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            Debug.Log("space");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
            Debug.Log("G");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            crouching = !crouching;
            Crouch();
            Debug.Log("c");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
            Debug.Log("R");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
            Debug.Log("mouse esquerdo baixo");
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shooting = false;
            Debug.Log("mouse esquerdo cima");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = speedbase * 1.5f;
            run = true;
            Debug.Log("shift esquerdo baixo");
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = speedbase;
            run = false;
            Debug.Log("shift esquerdo cima");
        }

        Movement();
        Rotation();
    }

    private void Movement()
    {
        float newSpeed = crouching ? speed / 2 : speed;
        Vector3 move = transform.right * direction.x + transform.forward * direction.z;
        characterController.Move(move * newSpeed * Time.deltaTime);
    }

    private void Rotation()
    {
        camDirection *= mouseSensitivity * Time.deltaTime;

        verticalRotation -= camDirection.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        camTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(Vector3.up * camDirection.x);
    }

    private void Fire()
    {
        if (shooting)
        {
            weapon.Fire(crouching);
        }
    }

    private void Jump()
    {
        playerGravity.Jump();
    }

    private void Crouch()
    {
        if (crouching)
        {
            camTransform.localPosition = Vector3.zero;
        }
        else
        {
            camTransform.localPosition = new Vector3(0, 0.5f);
        }
    }

    private void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, camTransform.rotation);

        Vector3 throwForce = transform.up * throwForceUP + camTransform.forward * throwForceForward;
        grenade.GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.Impulse);

        Destroy(grenade, 10);
    }
}

