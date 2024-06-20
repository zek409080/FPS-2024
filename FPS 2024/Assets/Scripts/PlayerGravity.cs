using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    bool onGround;
    Vector3 velocity;
    [SerializeField]
    float jumpHeight;
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    float radius;
    CharacterController characterController;

    [SerializeField]
    LayerMask groundMask;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        CheckGround();
    }

    void CheckGround()
    {
        onGround = Physics.CheckSphere(transform.position + offset, radius, groundMask);

        if(onGround && velocity.y < 0)
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity += Physics.gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (onGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}
