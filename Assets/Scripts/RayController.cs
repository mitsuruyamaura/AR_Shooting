using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController : MonoBehaviour
{
    public Vector3 muzzleFlashScale;

    public GameObject muzzleFlashPrefab;

    public GameObject hitEffectPrefab;

    private bool isShooting;

    private GameObject muzzleFlashObj;
    private GameObject hitEffectObj;

    private GameObject target;
    private EnemyController enemy;

    [SerializeField]
    private int[] layerMasks;

    [SerializeField]
    private string[] layerMasksStr;

    private EventBase<int> eventBase;

    [SerializeField]
    private PlayerController playerController;


    void Start()
    {
        layerMasksStr = new string[layerMasks.Length];
        for (int i = 0; i < layerMasks.Length; i++) {
            layerMasksStr[i] = LayerMask.LayerToName(layerMasks[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.BulletCount > 0 && Input.GetMouseButton(0)) {

            // 発射時間の計測
            StartCoroutine(ShootTimer());
        }
    }

    private IEnumerator ShootTimer() {
        if (!isShooting) {
            isShooting = true;

            if (muzzleFlashObj == null) {
                muzzleFlashObj = Instantiate(muzzleFlashPrefab, transform.position, transform.rotation);
                muzzleFlashObj.transform.SetParent(gameObject.transform);
                muzzleFlashObj.transform.localScale = muzzleFlashScale;

            } else {
                muzzleFlashObj.SetActive(true);
            }

            Shoot();

            yield return new WaitForSeconds(playerController.shootInterval);

            muzzleFlashObj.SetActive(false);

            if (hitEffectObj != null) {
                hitEffectObj.SetActive(false);
            }

            isShooting = false;

        } else {
            yield return null;
        }


    }

    private void Shoot() {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction, Color.blue, 3.0f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, playerController.shootRange, LayerMask.GetMask(layerMasksStr))) {

            Debug.Log(hit.collider.gameObject.name);

            // 同じ対象を攻撃しているか確認。対象がいなかったか、同じ対象でない場合
            if (target == null || target != hit.collider.gameObject) {

                // クラスを継承して使うようにして、TryGetComponent の処理を Base を取得して統一する
                target = hit.collider.gameObject;

                if (target.TryGetComponent(out eventBase)) {
                    eventBase.TriggerEvent(playerController.bulletPower);

                    // 演出
                    PlayHitEffect(hit.point, hit.normal);
                }

                //// ダメージ処理
                //if (target.TryGetComponent(out enemy)) {
                //    enemy.CalcDamage(playerController.bulletPower);

                //    // 演出
                //    PlayHitEffect(hit.point, hit.normal);
                //}
                //　同じ対象の場合
            } else {
                if (target.TryGetComponent(out eventBase)) {
                    eventBase.TriggerEvent(playerController.bulletPower);

                    // 演出
                    PlayHitEffect(hit.point, hit.normal);

                }
                //if (target.TryGetComponent(out enemy)) {
                //    enemy.CalcDamage(playerController.bulletPower);

                //    // 演出
                //    PlayHitEffect(hit.point, hit.normal);
                //}
            }
        }

        playerController.BulletCount--;
    }

    /// <summary>
    /// ヒット演出
    /// </summary>
    /// <param name="effectPos"></param>
    /// <param name="surfacePos"></param>
    private void PlayHitEffect(Vector3 effectPos, Vector3 surfacePos) {
        if (hitEffectObj == null) {
            hitEffectObj = Instantiate(hitEffectPrefab, effectPos, Quaternion.identity);
        } else {
            hitEffectObj.transform.position = effectPos;
            hitEffectObj.transform.rotation = Quaternion.FromToRotation(Vector3.forward, surfacePos);

            hitEffectObj.SetActive(true);
        }
    }
}
