using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject bulletImpact;
    [SerializeField] WeaponModel weaponModel;
    [SerializeField] int magazine, bullet, bulletMax;
    float fireTimer;
    bool onFire;

    private void Start()
    {
        firePoint = GetComponent<Transform>();
        MeshFilter meshFiler = GetComponentInChildren<MeshFilter>();
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        fireTimer = weaponModel.FireRate;
        magazine = weaponModel.MaganizeCap;
        bullet = bulletMax;
    }

    private void Update()
    {
        Fire();
        if (Input.GetKey(KeyCode.Mouse0)) 
        {
            onFire = true;
        }
        if (Input.GetKeyDown(KeyCode.R) && Time.time > weaponModel.ReloadTime && magazine < weaponModel.MaganizeCap)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }
    void Fire()
    {
        StartCoroutine(FireCoroutine());
    }

    private IEnumerator FireCoroutine()
    {
        if (onFire && bullet >=1) 
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0)
            {
                Shoot();
                bullet = bullet - 1;
                onFire = false;
                yield return new WaitForSeconds(fireTimer);
                fireTimer = weaponModel.FireRate;
            }
            
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        if (bullet <= 0)
        {
            magazine--;
            yield return new WaitForSeconds(weaponModel.ReloadTime);
            bullet = bulletMax;
        }

    }
    void Shoot()
    {
        RaycastHit hit;

        //float newSpread = crounching ? weaponModel.Spread / 2 : weaponModel.Spread;

        Vector3 direction = new Vector3(Random.Range(-weaponModel.Spread, weaponModel.Spread), 
            Random.Range(-weaponModel.Spread, weaponModel.Spread),0) + transform.forward;

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

