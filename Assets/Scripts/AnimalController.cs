using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimalController : MonoBehaviour
{
    public void MoveAnimal() {
        transform.DOMoveY(10, 3.0f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
}
