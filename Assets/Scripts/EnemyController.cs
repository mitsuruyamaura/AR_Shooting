using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    private Tween tween;

    private GameObject lookTarget;

    [SerializeField]
    private int hp;

    public void MoveEnemy() {
        //anim = GetComponent<Animator>();
        //anim.SetTrigger("jump");
        //tween = transform.DOMoveY(2.5f, 3.0f).SetEase(Ease.InBack)
        //    .OnComplete(() => {
        //        transform.DOMoveY(0, 3.0f).SetEase(Ease.InBack)
        //            .OnComplete(() => {
        //                Destroy(gameObject);
        //            });
        //    });

        tween = transform.DOMove(lookTarget.transform.position, 5.0f)
            .SetEase(Ease.Linear)
            .OnComplete(() => { Destroy(gameObject); });
    }

    /// <summary>
    /// �ړ����ꎞ��~
    /// </summary>
    public void PauseMove() {
        tween.Pause();
    }

    /// <summary>
    /// �ړ����ĊJ
    /// </summary>
    public void ResumeMove() {
        tween.Play();
    }

    /// <summary>
    /// �G�̐ݒ�
    /// </summary>
    /// <param name="player"></param>
    public void SetUpEnemyController(GameObject player) {
        lookTarget = player;

        MoveEnemy();
    }

    void Update() {

        // �J�����̕�����������
        if (lookTarget) {
            Vector3 direction = lookTarget.transform.position - transform.position;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
        }    
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

    /// <summary>
    /// �_���[�W�v�Z
    /// </summary>
    /// <param name="damage"></param>
    public void CalcDamage(int damage) {
        hp -= damage;

        if (hp <= 0) {
            Destroy(gameObject);
        }
    }
}