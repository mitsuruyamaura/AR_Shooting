using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerPoint : MonoBehaviour {

    private GameManager gameManager;

    [SerializeField, Header("��������C�x���g�̎��")]
    private EventType[] eventTypes;

    [SerializeField, Header("��������C�x���g�̔ԍ�")]
    private int[] eventNos;

    [SerializeField, Tooltip("�C�x���g�̐����n�_")]
    private Transform[] eventTrans;

    [SerializeField, Header("��������C�x���g")]
    private EventDataSO.EventData[] eventDatas;

    /// <summary>
    /// EventTriggerPoint �̏���
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpEventTriggerPoint(GameManager gameManager) {
        this.gameManager = gameManager;

        eventDatas = new EventDataSO.EventData[eventTypes.Length];

        // �C�x���g�̎�ނɉ����ăX�N���v�^�u���E�I�u�W�F�N�g����f�[�^������
        for (int i = 0; i < eventTypes.Length; i++) {
            eventDatas[i] = DataBaseManager.instance.GetEventDataFromEventType(eventTypes[i], eventNos[i]);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("�ʉ�");

            // �C�x���g�̐���
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