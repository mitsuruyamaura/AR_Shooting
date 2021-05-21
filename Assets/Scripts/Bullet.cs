using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float shotSpeed;

    public GameObject damageArea;

    public void Shot(Vector3 direction) {
        GetComponent<Rigidbody>().AddForce(direction * shotSpeed);

        Destroy(gameObject, 3.0f);
    }
}
