using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int hp;

    [SerializeField]
    private int maxHp;

    private int bulletCount;

    public int maxBullet;

    public int bulletPower;

    public float shootInterval;

    public float shootRange;

    [SerializeField]
    private UIManager uiManagaer;

    public bool isReloadModeOn;

    public bool isReloading;

    public float reloadTime;


    public int BulletCount
    {
        set {
            bulletCount = Mathf.Clamp(value, 0, maxBullet);
        }

        get {
            return bulletCount;
        }
    }

    public int Hp
    {
        get { return hp; }
    }

    /// <summary>
    /// 設定
    /// </summary>
    public void SetUpPlayer() {
        hp = maxHp;
        BulletCount = maxBullet;
    }

    /// <summary>
    /// HP計算
    /// </summary>
    public void CalcHp(int amount) {
        hp = Mathf.Clamp(hp += amount, 0, maxHp);

        uiManagaer.UpdateDisplayLife(hp);

        if (hp <= 0) {
            Debug.Log("Game Over");
        }
    }

    /// <summary>
    /// 弾丸加算
    /// </summary>
    /// <param name="amout"></param>
    public void CalcBulletCount(int amout) {

        BulletCount += amout;

        uiManagaer.UpdateDisplayBulletCount(BulletCount);
    }

    /// <summary>
    /// リロード
    /// </summary>
    public IEnumerator ReloadBullet() {
        isReloading = true;

        // リロード
        BulletCount = maxBullet;

        uiManagaer.UpdateDisplayBulletCount(BulletCount);

        // TODO SE

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
    }

    private void OnTriggerEnter(Collider other) {

        // ボスや敵の攻撃範囲を感知しないようにするためにタグでも判定する
        if (other.gameObject.tag == "Bullet" && other.transform.parent.gameObject.TryGetComponent(out Bullet bullet)) {
            CalcHp(-bullet.attackPower);

            Destroy(other.gameObject);

            Debug.Log("ヒット");
        }
    }
}
