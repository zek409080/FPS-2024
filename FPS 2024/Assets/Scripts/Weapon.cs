using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletImpact;
    [SerializeField] WeaponModel weaponModel;
    [SerializeField] int magazine;

    float fireTimer;

    bool fire = false;

    int contador;

    private void Start()
    {
        firePoint = GetComponent<Transform>();
        MeshFilter meshFiler = GetComponentInChildren<MeshFilter>();
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        fireTimer = weaponModel.FireRate;
        magazine = weaponModel.MaganizeCap;
    }

    void Fire(bool crounchin)
    {
        StartCoroutine(FireCoroutine(crounchin));
    }

    private IEnumerator FireCoroutine(bool crounchin)
    {
        if (Time.time > fireTimer) 
        {
            fireTimer =  Time.time + 1 / weaponModel.FireRate;

            for (int i = 0; i < weaponModel.BulletsForShoot; i++)
            {
                Shoot(crounchin);
                yield return new WaitForSeconds(weaponModel.BulletsForShoot);
            }
        }
    }

    void Shoot(bool crounching)
    {
        RaycastHit hit;

        float newSpread = crounching ? weaponModel.Spread / 2 : weaponModel.Spread;

        Vector3 direction = new Vector3(Random.Range(-newSpread, newSpread), Random.Range(newSpread, newSpread),0) + transform.forward;

        if (Physics.Raycast(firePoint.position, direction,out hit, weaponModel.Range))
        {
            Collider obj = hit.transform.GetComponent<Collider>();
            if (obj != null) 
            {
                Debug.Log(obj.gameObject.name);
                Instantiate(bulletImpact, hit.point, Quaternion.LookRotation(hit.normal));  
            }
        }
        Debug.DrawLine(firePoint.position, firePoint.position + direction * weaponModel.Range);
    }
}
