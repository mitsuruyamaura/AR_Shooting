using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    [SerializeField]
    private EventDataSO enemyEventDataSO;

    [SerializeField]
    private EventDataSO gimmickDataSO;

    [SerializeField]
    private EventDataSO trapDataSO;

    [SerializeField]
    private EventDataSO itemDataSO;

    [SerializeField]
    private EventDataSO treasureDataSO;

    [SerializeField]
    private EnemyDataSO enemyDataSO;


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ƒCƒxƒ“ƒg‚Ìî•ñ‚ğæ“¾
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="eventNo"></param>
    /// <returns></returns>
    public EventDataSO.EventData GetEventDataFromEventType(EventType eventType, int eventNo) {
        switch (eventType) {
            case EventType.Enemy:
                return enemyEventDataSO.eventDatasList.Find(x => x.eventNo == eventNo);

            case EventType.Gimmick:
                return gimmickDataSO.eventDatasList.Find(x => x.eventNo == eventNo);

            case EventType.Trap:
                return trapDataSO.eventDatasList.Find(x => x.eventNo == eventNo);

            case EventType.Item:
                return itemDataSO.eventDatasList.Find(x => x.eventNo == eventNo);

            case EventType.Treasure:
                return treasureDataSO.eventDatasList.Find(x => x.eventNo == eventNo);

            default:
                return null;
        }
    }

    /// <summary>
    /// “G‚Ìî•ñ‚ğæ“¾
    /// </summary>
    /// <param name="searchEnemyNo"></param>
    /// <returns></returns>
    public EnemyData GetEnemyData(int searchEnemyNo) {
        return enemyDataSO.enemyDatasList.Find(x => x.enemyNo == searchEnemyNo);
    }
}
