using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerPoint : MonoBehaviour {
    private GameManager gameManager;

    [SerializeField, Header("��������C�x���g�̎��")]
    private EventType[] eventTypes;

    [SerializeField]
    private int[] eventNos;

    //[SerializeField, Tooltip("�C�x���g�̐����n�_")]
    //private Transform[] eventTrans;

    [SerializeField, Header("��������C�x���g")]
    private EventDataSO.EventData[] eventDatas;

    public void SetUpEventTriggerPoint(GameManager gameManager) {
        this.gameManager = gameManager;

        eventDatas = new EventDataSO.EventData[eventTypes.Length];

        for (int i = 0; i < eventTypes.Length; i++) {
            eventDatas[i] = DataBaseManager.instance.GetEventDataFromEventType(eventTypes[i], eventNos[i]);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("�ʉ�");

            // �C�x���g�̐���
            gameManager.GenerateEvent(eventDatas, eventTypes);
        }
    }
}