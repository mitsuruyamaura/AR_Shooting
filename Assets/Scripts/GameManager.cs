using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private EventTriggerPoint[] eventTriggerPoint;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject gimmickPrefab;

    [SerializeField]
    private List<GameObject> enemiesList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> gimmicksList = new List<GameObject>();


    IEnumerator Start() {
        yield return StartCoroutine(PreparateGame());
    }

    private IEnumerator PreparateGame() {
        for (int i = 0; i < eventTriggerPoint.Length; i++) {
            eventTriggerPoint[i].SetUpEventTriggerPoint(this);
        }
        yield return null;
    }

    public void GenerateEvent(EventDataSO.EventData[] eventDatas, EventType[] eventTypes) {

        // イベントの種類に応じてスクリプタブル・オブジェクトからデータを検索
       
        for (int i = 0; i < eventDatas.Length; i++) {
            switch (eventTypes[i]) {
                case EventType.Enemy:
                    GameObject enemy = Instantiate(enemyPrefab, eventDatas[i].eventTran);
                    enemiesList.Add(enemy);

                    continue;

                case EventType.Gimmick:
                    GameObject gimmick = Instantiate(enemyPrefab, eventDatas[i].eventTran);
                    gimmicksList.Add(gimmick);
                    continue;
            }
        }
    }
}