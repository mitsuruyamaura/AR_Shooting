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
    /// ˆÚ“®‚ğˆê’â~
    /// </summary>
    public void PauseMove() {
        tween.Pause();
    }

    /// <summary>
    /// ˆÚ“®‚ğÄŠJ
    /// </summary>
    public void ResumeMove() {
        tween.Play();
    }

    /// <summary>
    /// “G‚Ìİ’è
    /// </summary>
    /// <param name="playerObj"></param>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    public IEnumerator SetUpEnemyController(GameObject playerObj, GameManager gameManager) {
        lookTarget = playerObj;
        this.gameManager = gameManager;

        TryGetComponent(out agent);
        TryGetComponent(out anim);

        agent.destination = lookTarget.transform.position;

        for (int i = 0; i < partsControllersList.Count; i++) {
            partsControllersList[i].SetUpPartsController(this);
        }

        yield return null;

        //MoveEnemy();
    }


    void Update() {

        // ƒJƒƒ‰‚Ì•ûŒü‚ğŒü‚¯‚é
        if (lookTarget) {
            Vector3 direction = lookTarget.transform.position - transform.position;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
        }

        if (lookTarget != null) {
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
    /// UŒ‚
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
    /// ƒ_ƒ[ƒWŒvZ
    /// </summary>
    /// <param name="damage"></param>
    public void CalcDamage(int damage, BodyRegionType bodyPartType = BodyRegionType.Boby) {
        hp -= damage;

        if (hp <= 0) {

            anim.ResetTrigger("Attack");

            anim.SetBool("Down", true);

            gameManager.RemoveEnemyList(this);

            // “ª‚ğ‘Å‚Á‚Ä“|‚µ‚½ê‡
            if (bodyPartType == BodyRegionType.Head) {

                // “ª‚ğÁ‚·
                BodyRegionPartsController parts = partsControllersList.Find(x => x.GetBodyPartType() == bodyPartType);
                parts.gameObject.SetActive(false);
            }

            Destroy(gameObject, 1.5f);
        }
    }

    public override void TriggerEvent(int value) {
        CalcDamage(value);
    }
}
