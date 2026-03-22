using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private Player player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Transform aim;

    private void Start()
    {
        player = GetComponent<Player>();
        player.controlls.Character.Fire.performed += context => Shoot();
    }
    private void Shoot()
    {
        

        GameObject newBullet= Instantiate(bulletPrefab,gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        newBullet.GetComponent<Rigidbody>().velocity = bulletDirection() * bulletSpeed;

        Destroy(newBullet,10);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }

    private Vector3 bulletDirection()
    {
        Vector3 direction=(aim.position - gunPoint.position).normalized;

        direction.y = 0;

        weaponHolder.LookAt(aim);
        gunPoint.LookAt(aim);
        return direction;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(weaponHolder.position,weaponHolder.position + weaponHolder.forward *25);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(gunPoint.position, gunPoint.position + bulletDirection() *25);
    }
}
