using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRegionPartsController : MonoBehaviour
{
    [SerializeField, Header("部位の設定")]
    private BodyRegionType bodyPartType;

    private EnemyController enemyController;


    public void SetUpPartsController(EnemyController enemyController) {
        this.enemyController = enemyController;
    }

    public BodyRegionType GetBodyPartType() {
        return bodyPartType;
    }

    /// <summary>
    /// 部位ごとにダメージの値を計算
    /// </summary>
    /// <param name="damage"></param>
    public void CalcDamageParts(int damage) {

        var lastDamage = bodyPartType switch {
            BodyRegionType.Head => damage * 5,
            _ => damage * 1,
        };

        enemyController.CalcDamage(lastDamage, bodyPartType);
    }
}
