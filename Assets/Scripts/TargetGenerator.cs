using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGenerator : MonoBehaviour
{
    [SerializeField]
    private EnemyController animalPrefab;

    [SerializeField]
    private Transform animalTran;

    void Start() {
        //StartCoroutine(GenerateAnimals());
    }

    /// <summary>
    /// 対象物の生成
    /// </summary>
    /// <returns></returns>
    //public IEnumerator GenerateAnimals() {
        //while (true) {
        //    Instantiate(animalPrefab, animalTran).MoveEnemy();
        //    yield return new WaitForSeconds(3.0f);
        //}
    //}
}
