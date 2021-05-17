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
    /// ê›íË
    /// </summary>
    public void SetUpPlayer() {
        hp = maxHp;
        BulletCount = maxBullet;
    }

    /// <summary>
    /// HPåvéZ
    /// </summary>
    public void CalcHp(int amount) {
        hp = Mathf.Clamp(hp += amount, 0, maxHp);
        uiManagaer.UpdateDisplayLife(hp);

        if (hp <= 0) {
            Debug.Log("Game Over");
        }
    }

    /// <summary>
    /// íeä€â¡éZ
    /// </summary>
    /// <param name="amout"></param>
    public void CalcBulletCount(int amout) {

        BulletCount += amout;

        uiManagaer.UpdateDisplayBulletCount(BulletCount);
    }

    /// <summary>
    /// ÉäÉçÅ[Éh
    /// </summary>
    public IEnumerator ReloadBullet() {
        isReloading = true;

        // ÉäÉçÅ[Éh
        BulletCount = maxBullet;

        uiManagaer.UpdateDisplayBulletCount(BulletCount);

        // TODO SE

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
    }
}
