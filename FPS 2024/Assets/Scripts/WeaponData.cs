using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private float damage;
    [SerializeField] private float range;
    [SerializeField] private float fireRate;
    [SerializeField] private float spread;
    [SerializeField] private float reloadTime;
    [SerializeField] private float timeBetweenShoots;
    [SerializeField] private int magazineCap;
    [SerializeField] private int bulletsForShoot;
    [SerializeField] private bool automatic;
    [SerializeField] private bool scope;
    [SerializeField] private Mesh model;
    [SerializeField] private Material material;
    [SerializeField] private Vector3 weaponPosition;

    public float Damage { get => damage; }
    public float Range { get => range; }
    public float FireRate { get => fireRate; }
    public float Spread { get => spread; }
    public float ReloadTime { get => reloadTime; }
    public float TimeBetweenShoots { get => timeBetweenShoots; }
    public int MagazineCap { get => magazineCap; }
    public int BulletsForShoot { get => bulletsForShoot; }
    public bool Automatic { get => automatic; }
    public bool Scope { get => scope; }
    public Mesh Model { get => model; }
    public Material Material { get => material; }
    public Vector3 WeaponPosition { get => weaponPosition; }
}
