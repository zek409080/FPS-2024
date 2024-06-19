using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon")]
public class WeaponData : ScriptableObject
{ 
    [SerializeField] float damage;
    [SerializeField] float range;
    [SerializeField] float fireRate;
    [SerializeField] float spread;
    [SerializeField] float reloadTime;
    [SerializeField] float timeBetweenShoots;
    [SerializeField] int maganizeCap;
    [SerializeField] int bulletsForShoot;
    [SerializeField] bool scope;
    [SerializeField] bool automatic;
    [SerializeField] Mesh model;
    [SerializeField] Material material;
    [SerializeField] Vector3 weaponPosition;

    public float Damage { get => damage; set => damage = value; }
    public float Range { get => range; set => range = value; }
    public float FireRate { get => fireRate; set => fireRate = value; }
    public float Spread { get => spread; set => spread = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public float TimeBetweenShoot { get => timeBetweenShoots; set => timeBetweenShoots = value; }
    public int BulletsForShoot { get => bulletsForShoot; set => bulletsForShoot = value; }
    public int MaganizeCap { get => maganizeCap; set => maganizeCap = value; }
    public bool Scope { get => scope; set => scope = value; }
    public bool Automatic { get => automatic; set => automatic = value; }
    public Mesh Model { get => model; set => model = value; }
    public Material Material { get => material; set => material = value; }
    public Vector3 WeaponPosition { get => weaponPosition; set => weaponPosition = value; }
}


