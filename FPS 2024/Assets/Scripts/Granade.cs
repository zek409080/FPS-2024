using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviourPun
{
    [PunRPC]
    void Initialize(Vector3 throwForce)
    {
        GetComponent<Rigidbody>().AddForce(throwForce, ForceMode.Impulse);
        Destroy(gameObject, 10);
    }
}
