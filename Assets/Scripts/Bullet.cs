using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float shotSpeed;

    public void Shot(Vector3 direction) {
        GetComponent<Rigidbody>().AddForce(direction * shotSpeed);

        Destroy(gameObject, 3.0f);
    }
}
