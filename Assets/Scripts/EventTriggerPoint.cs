using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerPoint : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField, Header("��������C�x���g�̎��")]
    private EventType[] eventTypes;

    [SerializeField, Tooltip("�C�x���g�̐����n�_")]
    private Transform[] eventTrans;

    [SerializeField]
    private EventData[] eventData;

    public void SetUpEventTriggerPoint(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("�ʉ�");

            // �C�x���g�̐���
            gameManager.GenerateEvent(eventTypes, eventTrans);
        }
    }
}
