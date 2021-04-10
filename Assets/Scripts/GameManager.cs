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

    [SerializeField]
    private UIManager uiManager;

    [System.Serializable]
    public class RootEventData {
        public int[] rootEventNos;
        public BranchDirectionType[] branchDirectionTypes;  // 分岐の方向
        public RootType rootType;
    }

    [SerializeField]
    private List<RootEventData> rootDatasList = new List<RootEventData>();

    private int currentRailCount;       // 現在の進行状況


    IEnumerator Start() {


        // ゲームの準備
        yield return StartCoroutine(PreparateGame());

        // 次のルートの確認と設定
        yield return StartCoroutine(CheckNextRootBranch());


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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="checkRootNo"></param>
    /// <returns></returns>
    private List<PathData> GetPathDatasList(int checkRootNo) {
        return pathDataSO.rootDatasList.Find(x => x.rootNo == checkRootNo).pathDatasList;
    }

    /// <summary>
    /// ルートの確認
    /// 分岐がある場合には分岐の矢印ボタンを生成
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckNextRootBranch() {

        if (currentRailCount >= rootDatasList.Count) {
            // TODO クリア判定
            Debug.Log("クリア");

            yield break;
        }

        // 現在のレールカウントの RootType を確認して、次に発生するルートを決める
        switch (rootDatasList[currentRailCount].rootType) {
            case RootType.Normal_Battle:
                // 次のルートが１つなら
                if (rootDatasList[currentRailCount].rootEventNos.Length == 1) {
                    // 自動的にレール移動を開始
                    fieldAutoScroller.SetNextField(GetPathDatasList(rootDatasList[currentRailCount].rootEventNos[0]));
                    Debug.Log("分岐なしの移動開始");
                } else {
                    // 分岐がある場合、分岐イベントを発生させて、画面上に矢印のボタンを表示
                    yield return StartCoroutine(uiManager.GenerateBranchButtons(rootDatasList[currentRailCount].rootEventNos, rootDatasList[currentRailCount].branchDirectionTypes));

                    // 矢印が押されるまで待機(while でもOK)
                    yield return new WaitUntil(() => uiManager.GetSubmitBranch().Item1 == true);

                    // 選択した分岐のルートを設定
                    fieldAutoScroller.SetNextField(GetPathDatasList(uiManager.GetSubmitBranch().Item2));
                }
                
                break;

            case RootType.Boss_Battle:

                break;

            case RootType.Event:

                break;
        }

        
        // 次のためにアップ
        currentRailCount++;
    }
}