using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private int hp;

    public void CalcDamage(int damage) {

        hp -= damage;

        if (hp <= 0) {
            Debug.Log("Game Over");
        }
    }
}
