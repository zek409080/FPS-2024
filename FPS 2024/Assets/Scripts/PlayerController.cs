using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    const float speed = 5;
    const float throwForceForward = 10, throwForceUp = 5;

    CharacterController characterController;
    PlayerGravity playerGravity;
    Transform camTransform;
    Weapon weapon;
    Vector3 direction;
    Vector2 camDirection;

    float verticalRotation;
    bool shooting, crounching;

    [SerializeField]
    GameObject grenadePrefab;
    [SerializeField]
    Transform throwPoint;
    [SerializeField, Range(0f, 500f)]
    float mouseSensitivity;

    bool controllerOn = true;

    [SerializeField]Camera cam;
    public float normalFov = 60f;
    public float aimFov = 30f;
    public float zoomSpeed = 10f;

    UIManager manager;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        camTransform = GetComponentInChildren<Camera>().transform;
        playerGravity = GetComponent<PlayerGravity>();
        weapon = GetComponentInChildren<Weapon>();
        manager = GetComponentInChildren<UIManager>();
    }

    [PunRPC]
    private void Initialize()
    {
        if(!photonView.IsMine)
        {
            GetComponentInChildren<Camera>().enabled = false;
            controllerOn = false;
        }
        cam = GetComponentInChildren<Camera>();
        cam.fieldOfView = normalFov;
    }

    // Update is called once per frame
    void Update()
    {
        if(controllerOn) 
        { 
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        camDirection.x = Input.GetAxis("Mouse X");
        camDirection.y = Input.GetAxis("Mouse Y");


        if (Input.GetMouseButton(1)) // Bot�o direito do mouse
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, aimFov, zoomSpeed * Time.deltaTime);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFov, zoomSpeed * Time.deltaTime);
        }
            if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Crounch();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shooting = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shooting = false;
        }

        Movement();
        Rotation();
        Fire();
        }
    }

    void Movement()
    {
        float newSpeed = crounching ? speed / 2 : speed;

        Vector3 velocity = (direction.x * transform.right + direction.z * transform.forward) * newSpeed * Time.deltaTime;

        characterController.Move(velocity);
    }

    void Rotation ()
    {
        camDirection *= mouseSensitivity * Time.deltaTime;

        verticalRotation -= camDirection.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        camTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(Vector3.up * camDirection.x);
    }
    void Fire()
    {
        if(shooting)
        {
            weapon.Fire();
        }
    }

    void Jump()
    {
        playerGravity.Jump();
    }

    void Crounch()
    {
        crounching = !crounching;
    }

    void ThrowGrenade()
    {
        Granade grenade = NetworkManager.instance.Instantiate("Prefabs/Grenade", throwPoint.position, camTransform.rotation).GetComponent<Granade>();
        Vector3 throwForce = transform.up * throwForceUp + camTransform.forward * throwForceForward;
        grenade.photonView.RPC("Initialize", RpcTarget.All, throwForce);
    }
}
