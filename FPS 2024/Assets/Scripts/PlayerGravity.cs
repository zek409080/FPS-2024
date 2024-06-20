using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    bool onGround;
    [SerializeField]float radius;
    [SerializeField]Vector3 offset;
    [SerializeField]LayerMask mask;
    float jumpHeight;
    Vector3 velocity;
    CharacterController characterController;

    private void Update()
    {
        characterController = GetComponent<CharacterController>();
        CheckGround();
    }
    void CheckGround()
    {
        onGround = Physics.CheckSphere(transform.position + offset, radius, mask);
        if (onGround ) 
        {
            velocity = Vector3.zero;
        }
        else 
        {
            velocity = Physics.gravity;
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
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + offset, radius);
    }
}
