using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGenerator : MonoBehaviour
{
    [SerializeField]
    private AnimalController animalPrefab;

    [SerializeField]
    private Transform animalTran;

    void Start() {
        StartCoroutine(GenerateAnimals());
    }

    /// <summary>
    /// ëŒè€ï®ÇÃê∂ê¨
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateAnimals() {
        while (true) {
            Instantiate(animalPrefab, animalTran).MoveAnimal();
            yield return new WaitForSeconds(3.0f);
        }
    }
}
