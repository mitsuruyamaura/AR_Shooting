using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab;

    public float waitInterval;

    private EnemyController enemyController;

    /// <summary>
    /// íeÇÃê∂ê¨Ç∆î≠éÀ
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="enemyController"></param>
    public void GenerateBulletShot(Vector3 direction, float moveSpeed, EnemyController enemyController) {
        this.enemyController = enemyController;

        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(direction * moveSpeed);

        Destroy(gameObject, 3.0f);
    }

    private void OnTriggerEnter(Collider other) {
        if (TryGetComponent(out PlayerController player)) {

            player.CalcHp(-enemyController.GetAttackPower());

            Debug.Log("ÉqÉbÉg");

            Destroy(gameObject);
        }
    }
}
