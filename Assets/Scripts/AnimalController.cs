using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimalController : MonoBehaviour
{
    private Animator anim;
    private Tween tween;

    public void MoveAnimal() {
        anim = GetComponent<Animator>();
        anim.SetTrigger("jump");
        tween = transform.DOMoveY(2.5f, 3.0f).SetEase(Ease.InBack)
            .OnComplete(() => {
                transform.DOMoveY(0, 3.0f).SetEase(Ease.InBack)
                    .OnComplete(() => {
                        Destroy(gameObject);
                    });
            });
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bullet")) {
            tween.Kill();

            anim.ResetTrigger("jump");
            anim.SetTrigger("stun");

            Destroy(other.gameObject);

            Destroy(gameObject, 1.0f);
        }
    }
}
