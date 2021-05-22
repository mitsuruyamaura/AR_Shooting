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
    private BossAction bossAction;

    [SerializeField]
    private int enemyNo;

    [SerializeField]
    private EnemyData enemyData;


    //public void MoveEnemy(float moveTime) {
    //    //anim = GetComponent<Animator>();
    //    //anim.SetTrigger("jump");
    //    //tween = transform.DOMoveY(2.5f, 3.0f).SetEase(Ease.InBack)
    //    //    .OnComplete(() => {
    //    //        transform.DOMoveY(0, 3.0f).SetEase(Ease.InBack)
    //    //            .OnComplete(() => {
    //    //                Destroy(gameObject);
    //    //            });
    //    //    });

    //    //tween = transform.DOMove(lookTarget.transform.position, 5.0f)
    //    //    .SetEase(Ease.Linear)
    //    //    .OnComplete(() => { Destroy(gameObject); });

    //    Sequence sequence = DOTween.Sequence();

    //    sequence.Append(transform.DOLocalMove(bossAction.moveTrans[0].localPosition, moveTime).SetEase(Ease.Linear));
    //    sequence.AppendInterval(1.0f);
    //    sequence.Append(transform.DOLocalMove(bossAction.moveTrans[1].localPosition, moveTime).SetEase(Ease.Linear));
    //    sequence.AppendInterval(1.0f);
    //    sequence.Append(transform.DOLocalMove(bossAction.moveTrans[2].localPosition, moveTime).SetEase(Ease.Linear));
    //    sequence.AppendInterval(1.0f).SetLoops(-1, LoopType.Restart);

    //    tween = sequence;
    //}

    /// <summary>
    /// 移動を一時停止
    /// </summary>
    public void PauseMove() {
        tween.Pause();
    }

    /// <summary>
    /// 移動を再開
    /// </summary>
    public void ResumeMove() {
        tween.Play();
    }

    /// <summary>
    /// 敵の設定
    /// </summary>
    /// <param name="playerObj"></param>
    /// <param name="gameManager"></param>
    /// <returns></returns>
    public IEnumerator SetUpEnemyController(GameObject playerObj, GameManager gameManager) {
        lookTarget = playerObj;
        this.gameManager = gameManager;

        SetUpEnemyData();

        //TryGetComponent(out agent);
        TryGetComponent(out anim);

        // TODO Type で分岐し、 Agent の時には AddComponet する

        //agent = gameObject.AddComponent<NavMeshAgent>();

        if (TryGetComponent(out agent)) {
            agent.destination = lookTarget.transform.position;
        }

        for (int i = 0; i < partsControllersList.Count; i++) {
            partsControllersList[i].SetUpPartsController(this);
        }

        if (enemyMoveType == EnemyMoveType.Agent) {

            agent.speed = enemyData.moveValue;
            anim.SetBool("Walk", true);
        }

        if (enemyMoveType == EnemyMoveType.Boss_0) {
            bossAction.SetUpBossAction(this);
            bossAction.MoveEnemy(enemyData.moveValue);

        }
        //MoveEnemy();

        yield return null;
    }

    /// <summary>
    /// 敵の情報をデータベースより取得して設定
    /// </summary>
    private void SetUpEnemyData() {
        enemyData = DataBaseManager.instance.GetEnemyData(enemyNo);

        hp = enemyData.hp;
        attackPower = enemyData.attackPower;
        attackInterval = enemyData.attackInterval;
        enemyMoveType = enemyData.enemyMoveType;
        point = enemyData.point;

        // TODO

    }


    void Update() {

        // カメラの方向を向ける
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

        if (player != null && !isAttack) {

            if (enemyMoveType == EnemyMoveType.Agent) {
                attackCoroutine = Attack(player);
            } else if (enemyMoveType == EnemyMoveType.Boss_0) {
                attackCoroutine = AttackBoss_0();
            }

            StartCoroutine(attackCoroutine);

            Debug.Log("プレイヤー感知済");
        } else {
            if (other.gameObject.TryGetComponent(out player)) {

                if (enemyMoveType == EnemyMoveType.Agent) {
                    attackCoroutine = Attack(player);
                } else if (enemyMoveType == EnemyMoveType.Boss_0) {
                    attackCoroutine = AttackBoss_0();
                }
               
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
    /// 攻撃
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
    /// ダメージ計算
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

            // 頭を打って倒した場合
            if (bodyPartType == BodyRegionType.Head) {

                // 頭を消す
                BodyRegionPartsController parts = partsControllersList.Find(x => x.GetBodyPartType() == bodyPartType);
                parts.gameObject.SetActive(false);

                point *= 3;
            }

            // スコア加算
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

        bossAction.capsuleCollider.enabled = false;

        yield return new WaitForSeconds(bossAction.waitInterval);

        bossAction.GenerateBulletShot(player.transform.position - transform.position, attackPower);

        yield return new WaitForSeconds(attackInterval);

        bossAction.capsuleCollider.enabled = true;

        isAttack = false;
    }

    public int GetAttackPower() {
        return attackPower;
    }
}
