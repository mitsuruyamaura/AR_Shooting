using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;

    private Bullet bullet;

    public float waitInterval;

    private EnemyController enemyController;

    /// <summary>
    /// íeÇÃê∂ê¨Ç∆î≠éÀ
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="enemyController"></param>
    public void GenerateBulletShot(Vector3 direction, float moveSpeed, EnemyController enemyController) {
        Debug.Log("bullet genenrate");

        this.enemyController = enemyController;

        bullet = Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);

        bullet.damageArea.GetComponent<Rigidbody>().AddForce(direction * moveSpeed);

        Destroy(bullet, 3.0f);
    }

    private void OnTriggerEnter(Collider other) {
        if (TryGetComponent(out PlayerController player)) {

            player.CalcHp(-enemyController.GetAttackPower());

            Debug.Log("ÉqÉbÉg");

            Destroy(bullet);
        }
    }
}
