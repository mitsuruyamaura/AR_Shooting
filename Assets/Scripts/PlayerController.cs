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
    /// �ݒ�
    /// </summary>
    public void SetUpPlayer() {
        hp = maxHp;
        BulletCount = maxBullet;
    }

    /// <summary>
    /// HP�v�Z
    /// </summary>
    public void CalcHp(int amount) {
        hp = Mathf.Clamp(hp += amount, 0, maxHp);

        uiManagaer.UpdateDisplayLife(hp);

        if (hp <= 0) {
            Debug.Log("Game Over");
        }
    }

    /// <summary>
    /// �e�ۉ��Z
    /// </summary>
    /// <param name="amout"></param>
    public void CalcBulletCount(int amout) {

        BulletCount += amout;

        uiManagaer.UpdateDisplayBulletCount(BulletCount);
    }

    /// <summary>
    /// �����[�h
    /// </summary>
    public IEnumerator ReloadBullet() {
        isReloading = true;

        // �����[�h
        BulletCount = maxBullet;

        uiManagaer.UpdateDisplayBulletCount(BulletCount);

        // TODO SE

        yield return new WaitForSeconds(reloadTime);

        isReloading = false;
    }

    private void OnTriggerEnter(Collider other) {

        // �{�X��G�̍U���͈͂����m���Ȃ��悤�ɂ��邽�߂Ƀ^�O�ł����肷��
        if (other.gameObject.tag == "Bullet" && other.transform.parent.gameObject.TryGetComponent(out Bullet bullet)) {
            CalcHp(-bullet.attackPower);

            Destroy(other.gameObject);

            Debug.Log("�q�b�g");
        }
    }
}
