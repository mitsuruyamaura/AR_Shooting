using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [SerializeField]
    private Bullet bulletPrefab = null;

    [SerializeField]
    private ARManager arManager;

    void Update()
    {
        if (arManager.currentARState == ARManager.ARState.Play) {
            if (Input.GetMouseButtonDown(0)) {
                GenerateBullet();
            }
        }
    }

    private void GenerateBullet() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 direction = ray.direction;

        Bullet bullet = Instantiate(bulletPrefab);
        bullet.Shot(direction);
    }
}
