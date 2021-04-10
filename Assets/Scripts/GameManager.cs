using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private EventTriggerPoint[] eventTriggerPoint;

    [SerializeField]
    private List<GameObject> enemiesList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> gimmicksList = new List<GameObject>();

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private PathDataSO pathDataSO;

    [SerializeField]
    private FieldAutoScroller fieldAutoScroller;

    [System.Serializable]
    public class RootData {
        public int rootNo;
        public List<PathData> pathDatasList = new List<PathData>();
    }

    [SerializeField]
    private List<RootData> rootDatasList = new List<RootData>();


    IEnumerator Start() {
        yield return StartCoroutine(PreparateGame());
        fieldAutoScroller.SetNextField(rootDatasList[0].pathDatasList);
    }

    /// <summary>
    /// ゲームの準備
    /// </summary>
    /// <returns></returns>
    private IEnumerator PreparateGame() {
        for (int i = 0; i < eventTriggerPoint.Length; i++) {
            eventTriggerPoint[i].SetUpEventTriggerPoint(this);
        }
        yield return null;
    }

    /// <summary>
    /// エネミーの生成イベント
    /// </summary>
    /// <param name="enemyEventData"></param>
    /// <param name="enemyEventTran"></param>
    public void GenerateEnemy(EventDataSO.EventData enemyEventData, Transform enemyEventTran) {
        GameObject enemy = Instantiate(enemyEventData.eventPrefab, enemyEventTran);
        enemy.GetComponent<AnimalController>().SetUpAnimalController(player);
        enemiesList.Add(enemy);
    }

    /// <summary>
    /// ギミックの生成イベント
    /// </summary>
    /// <param name="enemyEventData"></param>
    /// <param name="enemyEventTran"></param>
    public void GenerateGimmick(EventDataSO.EventData enemyEventData, Transform enemyEventTran) {
        GameObject enemy = Instantiate(enemyEventData.eventPrefab, enemyEventTran);
        gimmicksList.Add(enemy);
    }

    /// <summary>
    /// すべての敵の移動を一時停止
    /// </summary>
    public void StopMoveAllEnemies() {
        if (enemiesList.Count <= 0) {
            return;
        }

        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].GetComponent<AnimalController>().StopMoveAnimal();
        }
    }

    /// <summary>
    /// すべての敵の移動を再開
    /// </summary>
    public void ResumeMoveAllEnemies() {
        if (enemiesList.Count <= 0) {
            return;
        }

        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].GetComponent<AnimalController>().ResumeMoveAnimal();
        }
    }
}