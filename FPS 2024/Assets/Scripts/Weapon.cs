using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] WeaponData weaponData;
    [SerializeField] GameObject bulletImpact;
    [SerializeField] Transform firePoint;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    int currentMagazine;
    int ammo;
    float timeToShoot;
    bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        UpdateWeapon(weaponData);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0)) 
        {
            Fire();
        }
    }

    public void Fire()
    {
        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        // Verifica se pode disparar
        if (Time.time >= timeToShoot && !reloading)
        {
            // Define o tempo para o próximo disparo
            timeToShoot = Time.time + 1 / weaponData.FireRate;

            // Dispara projéteis com o tempo entre cada disparo
            for (int i = 0; i < weaponData.BulletsForShoot; i++)
            {
                if (currentMagazine > 0)
                {
                    // Reduz a munição do carregador
                    currentMagazine--;
                    Shoot();
                    yield return new WaitForSeconds(weaponData.TimeBetweenShoots);
                }
            }
        }
    }

    void Shoot()
    {
        // Variável para armazenar o que foi atingido
        RaycastHit hit;
        // Direção do disparo, considerando a dispersão da arma
        Vector3 direction = firePoint.forward + new Vector3(Random.Range(-weaponData.Spread, weaponData.Spread), Random.Range(-weaponData.Spread, weaponData.Spread), 0);
        // Verifica se acertou algo na direção do disparo dentro do alcance
        if(Physics.Raycast(firePoint.position, direction, out hit, weaponData.Range)) 
        {
            NetworkManager.instance.Instantiate("Prefabs/Grenade", hit.point, Quaternion.identity);
            // Desenha uma linha para visualizar o trajeto do projétil
            Debug.DrawLine(firePoint.position, direction * weaponData.Range);

            IDamageable damageable = hit.transform.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(weaponData.Damage);
            }
        }
    }

    void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        // Verifica se precisa recarregar e se há munição disponível
        if(currentMagazine < weaponData.MagazineCap && ammo > 0)
        {
            // Atualiza a munição no carregador e no inventário
            if (currentMagazine + ammo >= weaponData.MagazineCap)
            {
                ammo -= weaponData.MagazineCap - currentMagazine;
                currentMagazine = weaponData.MagazineCap;
            }
            else
            {
                currentMagazine += ammo;
                ammo = 0;
            }
            // Aguarda o tempo de recarga
            reloading = true;
            yield return new WaitForSeconds(weaponData.ReloadTime);
            reloading = false;
        }
    }

    void UpdateWeapon(WeaponData newWeapon)
    {
        weaponData = newWeapon;
        meshFilter.mesh = weaponData.Model;
        meshRenderer.material = weaponData.Material;

        currentMagazine = weaponData.MagazineCap;
    }
}