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

    public void Fire(bool crouching)
    {
        StartCoroutine(FireCoroutine(crouching));
    }

    IEnumerator FireCoroutine(bool crouch)
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
                    Shoot(crouch);
                    yield return new WaitForSeconds(weaponData.TimeBetweenShoot);
                }
            }
        }
    }

    void Shoot(bool crouch)
    {
        // Variável para armazenar o que foi atingido
        RaycastHit hit;
        // Direção do disparo, considerando a dispersão da arma
        float newSpread = crouch ? weaponData.Spread / 2 : weaponData.Spread;
        Vector3 direction = firePoint.forward + new Vector3(Random.Range(-newSpread, newSpread), Random.Range(-newSpread, newSpread), 0);
        // Verifica se acertou algo na direção do disparo dentro do alcance
        if (Physics.Raycast(firePoint.position, direction, out hit, weaponData.Range))
        {
            Instantiate(bulletImpact, hit.point, Quaternion.identity);
            // Desenha uma linha para visualizar o trajeto do projétil
            Debug.DrawLine(firePoint.position, direction * weaponData.Range);
        }
    }

    public void Reload()
    {
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        // Verifica se precisa recarregar e se há munição disponível
        if (currentMagazine < weaponData.MaganizeCap && ammo > 0)
        {
            // Atualiza a munição no carregador e no inventário
            if (currentMagazine + ammo >= weaponData.MaganizeCap)
            {
                ammo -= weaponData.MaganizeCap - currentMagazine;
                currentMagazine = weaponData.MaganizeCap;
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

        currentMagazine = weaponData.MaganizeCap;
    }
}