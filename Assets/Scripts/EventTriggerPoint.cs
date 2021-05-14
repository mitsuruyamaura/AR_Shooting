using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerPoint : MonoBehaviour {

    private GameManager gameManager;

    [SerializeField, Header("発生するイベントの種類")]
    private EventType[] eventTypes;

    [SerializeField, Header("発生するイベントの番号")]
    private int[] eventNos;

    [SerializeField, Tooltip("イベントの生成地点")]
    private Transform[] eventTrans;

    [SerializeField, Header("発生するイベント")]
    private EventDataSO.EventData[] eventDatas;

    /// <summary>
    /// EventTriggerPoint の準備
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpEventTriggerPoint(GameManager gameManager) {
        this.gameManager = gameManager;

        eventDatas = new EventDataSO.EventData[eventTypes.Length];

        // イベントの種類に応じてスクリプタブル・オブジェクトからデータを検索
        for (int i = 0; i < eventTypes.Length; i++) {
            eventDatas[i] = DataBaseManager.instance.GetEventDataFromEventType(eventTypes[i], eventNos[i]);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("通過");

            // イベントの生成
            for (int i = 0; i < eventDatas.Length; i++) {
                switch (eventTypes[i]) {
                    case EventType.Enemy:
                        gameManager.GenerateEnemy(eventDatas[i], eventTrans[i]);
                        continue;
                    case EventType.Gimmick:
                        gameManager.GenerateGimmick(eventDatas[i], eventTrans[i]);
                        continue;

                    case EventType.Item:
                        gameManager.GenerateItem(eventDatas[i], eventTrans[i]);
                        break;
                }
            }
        }
    }
}