using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private EventTriggerPoint[] eventTriggerPoint;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private List<GameObject> enemiesList = new List<GameObject>();

    IEnumerator Start() {
        yield return StartCoroutine(PreparateGame());
    }

    private IEnumerator PreparateGame() {
        for (int i = 0; i < eventTriggerPoint.Length; i++) {
            eventTriggerPoint[i].SetUpEventTriggerPoint(this);
        }
        yield return null;
    }

    public void GenerateEvent(EventDataSO.EventData[] eventDatas) {

        // �C�x���g�̎�ނɉ����ăX�N���v�^�u���E�I�u�W�F�N�g����f�[�^������

        for (int i = 0; i < eventDatas.Length; i++) {
            GameObject enemy = Instantiate(enemyPrefab, eventDatas[i].eventTran);
            enemiesList.Add(enemy);
        }
    }
}