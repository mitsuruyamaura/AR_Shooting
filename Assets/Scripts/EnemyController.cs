using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using System;
using System.Linq;

public class EnemyController : EventBase<int>
{
    private Animator anim;
    private Tween tween;

    private GameObject lookTarget;

    [SerializeField]
    private int hp;

    [SerializeField]
    private int attackPower;

    [SerializeField]
    private List<BodyRegionPartsController> partsControllersList = new List<BodyRegionPartsController>();

    private NavMeshAgent agent;

    private bool isAttack;

    private float attackInterval = 3.0f;

    private PlayerController player;

    private GameManager gameManager;

    private IEnumerator attackCoroutine;

    private int point = 100;

    private bool isDead;



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
    /// <param name="playerObj"></param>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    public IEnumerator SetUpEnemyController(GameObject playerObj, GameManager gameManager) {
        lookTarget = playerObj;
        this.gameManager = gameManager;

        //TryGetComponent(out agent);
        TryGetComponent(out anim);

        if (TryGetComponent(out agent)) {
            agent.destination = lookTarget.transform.position;
        }

        for (int i = 0; i < partsControllersList.Count; i++) {
            partsControllersList[i].SetUpPartsController(this);
        }

        anim.SetBool("Walk", true);

        yield return null;

        //MoveEnemy();
    }


    void Update() {

        // �J�����̕�����������
        if (lookTarget) {
            Vector3 direction = lookTarget.transform.position - transform.position;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
        }

        if (lookTarget != null && agent != null) {
            agent.destination = lookTarget.transform.position;
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

    private void OnTriggerStay(Collider other) {
        if (isAttack) {
            return;
        }

        if (player != null) {
            StartCoroutine(attackCoroutine);
        } else {
            if (other.gameObject.TryGetComponent(out player)) {
                attackCoroutine = Attack(player);
                StartCoroutine(attackCoroutine);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (player != null) {
            player = null;
            StopCoroutine(attackCoroutine);
            isAttack = false;
        }
    }

    /// <summary>
    /// �U��
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack(PlayerController player) {
        isAttack = true;

        player.CalcHp(-attackPower);

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackInterval);

        isAttack = false;
    }

    /// <summary>
    /// �_���[�W�v�Z
    /// </summary>
    /// <param name="damage"></param>
    public void CalcDamage(int damage, BodyRegionType bodyPartType = BodyRegionType.Boby) {
        if (isDead) {
            return;
        }

        hp -= damage;

        anim.ResetTrigger("Attack");

        if (hp <= 0) {
            isDead = true;

            anim.SetBool("Walk", false);

            anim.SetBool("Down", true);

            gameManager.RemoveEnemyList(this);       

            // ����ł��ē|�����ꍇ
            if (bodyPartType == BodyRegionType.Head) {

                // ��������
                BodyRegionPartsController parts = partsControllersList.Find(x => x.GetBodyPartType() == bodyPartType);
                parts.gameObject.SetActive(false);

                point *= 3;
            }

            // �X�R�A���Z
            GameData.instance.scoreReactiveProperty.Value += point;

            Destroy(gameObject, 1.5f);
        } else {
            anim.SetTrigger("Damage");
        }
    }

    public override void TriggerEvent(int value) {
        CalcDamage(value);
    }
}
