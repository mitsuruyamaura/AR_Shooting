using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using System;
using System.Linq;

public enum EnemyMoveType {
    Agent,
    Boss_0,
    Boss_1
}

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

    public EnemyMoveType enemyMoveType;

    [SerializeField]
    private Transform[] moveTrans;

    [SerializeField]
    private BossAction bossAction;


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

        //tween = transform.DOMove(lookTarget.transform.position, 5.0f)
        //    .SetEase(Ease.Linear)
        //    .OnComplete(() => { Destroy(gameObject); });

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOLocalMove(moveTrans[0].localPosition, 3.0f).SetEase(Ease.Linear));
        sequence.AppendInterval(1.0f);
        sequence.Append(transform.DOLocalMove(moveTrans[1].localPosition, 3.0f).SetEase(Ease.Linear));
        sequence.AppendInterval(1.0f);
        sequence.Append(transform.DOLocalMove(moveTrans[2].localPosition, 3.0f).SetEase(Ease.Linear));
        sequence.AppendInterval(1.0f).SetLoops(-1, LoopType.Restart);

        tween = sequence;
    }

    /// <summary>
    /// à⁄ìÆÇàÍéûí‚é~
    /// </summary>
    public void PauseMove() {
        tween.Pause();
    }

    /// <summary>
    /// à⁄ìÆÇçƒäJ
    /// </summary>
    public void ResumeMove() {
        tween.Play();
    }

    /// <summary>
    /// ìGÇÃê›íË
    /// </summary>
    /// <param name="playerObj"></param>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    public IEnumerator SetUpEnemyController(GameObject playerObj, GameManager gameManager) {
        lookTarget = playerObj;
        this.gameManager = gameManager;

        //TryGetComponent(out agent);
        TryGetComponent(out anim);

        // TODO Type Ç≈ï™äÚÇµÅA Agent ÇÃéûÇ…ÇÕ AddComponet Ç∑ÇÈ

        //agent = gameObject.AddComponent<NavMeshAgent>();

        if (TryGetComponent(out agent)) {
            agent.destination = lookTarget.transform.position;
        }

        for (int i = 0; i < partsControllersList.Count; i++) {
            partsControllersList[i].SetUpPartsController(this);
        }

        anim.SetBool("Walk", true);

        yield return null;

        if (enemyMoveType == EnemyMoveType.Boss_0) {
            MoveEnemy();
        }
        //MoveEnemy();
    }


    void Update() {

        // ÉJÉÅÉâÇÃï˚å¸Çå¸ÇØÇÈ
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

                if (enemyMoveType == EnemyMoveType.Agent) {
                    attackCoroutine = Attack(player);
                } else if (enemyMoveType == EnemyMoveType.Boss_0) {
                    attackCoroutine = AttackBoss_0();
                }

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
    /// çUåÇ
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
    /// É_ÉÅÅ[ÉWåvéZ
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

            // ì™Çë≈Ç¡Çƒì|ÇµÇΩèÍçá
            if (bodyPartType == BodyRegionType.Head) {

                // ì™Çè¡Ç∑
                BodyRegionPartsController parts = partsControllersList.Find(x => x.GetBodyPartType() == bodyPartType);
                parts.gameObject.SetActive(false);

                point *= 3;
            }

            // ÉXÉRÉAâ¡éZ
            GameData.instance.scoreReactiveProperty.Value += point;

            Destroy(gameObject, 1.5f);
        } else {
            anim.SetTrigger("Damage");
        }
    }

    public override void TriggerEvent(int value) {
        CalcDamage(value);
    }

    private IEnumerator AttackBoss_0() {
        isAttack = true;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(bossAction.waitInterval);

        bossAction.GenerateBulletShot(transform.forward, 10, this);

        yield return new WaitForSeconds(attackInterval);

        isAttack = false;
    }

    public int GetAttackPower() {
        return attackPower;
    }
}
