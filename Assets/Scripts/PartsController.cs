using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsController : MonoBehaviour
{
    [SerializeField, Header("���ʂ̐ݒ�")]
    private BodyPartType bodyPartType;

    private EnemyController enemyController;


    public void SetUpPartsController(EnemyController enemyController) {
        this.enemyController = enemyController;
    }

    public BodyPartType GetBodyPartType() {
        return bodyPartType;
    }

    /// <summary>
    /// ���ʂ��ƂɃ_���[�W�̒l���v�Z
    /// </summary>
    /// <param name="damage"></param>
    public void CalcDamageParts(int damage) {

        var lastDamage = bodyPartType switch {
            BodyPartType.Head => damage * 5,
            _ => damage * 1,
        };

        enemyController.CalcDamage(lastDamage, bodyPartType);
    }
}
