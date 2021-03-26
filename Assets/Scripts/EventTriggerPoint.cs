using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerPoint : MonoBehaviour {
    private GameManager gameManager;

    //[SerializeField, Header("発生するイベントの種類")]
    //private EventType[] eventTypes;

    //[SerializeField, Tooltip("イベントの生成地点")]
    //private Transform[] eventTrans;

    [SerializeField, Header("発生するイベント")]
    private EventDataSO.EventData[] eventDatas;

    public void SetUpEventTriggerPoint(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("通過");

            // イベントの生成
            gameManager.GenerateEvent(eventDatas);
        }
    }
}