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
        uiManagaer.ReLife(hp);

        if (hp <= 0) {
            Debug.Log("Game Over");
        }
    }

    /// <summary>
    /// �e�ۉ��Z
    /// </summary>
    /// <param name="amout"></param>
    public void GainBulletCount(int amout) {
        BulletCount += amout;
        
    }
}
