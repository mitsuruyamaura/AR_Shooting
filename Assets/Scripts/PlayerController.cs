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

    public int BulletCount
    {
        set {
            bulletCount = Mathf.Clamp(value, 0, maxBullet);
        }

        get {
            return bulletCount;
        }
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

        if (hp <= 0) {
            Debug.Log("Game Over");
        }
    }

    /// <summary>
    /// íeä€â¡éZ
    /// </summary>
    /// <param name="amout"></param>
    public void GainBulletCount(int amout) {
        BulletCount += amout;
    }
}
