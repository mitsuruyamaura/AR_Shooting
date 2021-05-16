using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private MissionTriggerPoint[] eventTriggerPoint;

    [SerializeField]
    private List<EnemyController> enemiesList = new List<EnemyController>();

    [SerializeField]
    private List<GameObject> gimmicksList = new List<GameObject>();

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

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private int currentMissionDuration;

    [SerializeField]
    private EventGenerator eventGenerator;


    public IEnumerator SetStart() {

        playerController.SetUpPlayer();

        eventGenerator.SetUpEventGenerator(this, playerController);

        uiManager.SetMaxLife(playerController.Hp);

        StartCoroutine(uiManager.GenerateLife(playerController.Hp));

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
            eventTriggerPoint[i].SetUpMissionTriggerPoint(this);
        }
        yield return null;
    }

    /// <summary>
    /// 敵の情報を List に追加
    /// </summary>
    /// <param name="enemyController"></param>
    public void AddEnemyList(EnemyController enemyController) {
        enemiesList.Add(enemyController);
    }

    /// <summary>
    /// ギミックの情報を List に追加
    /// </summary>
    /// <param name="gimmick"></param>
    public void AddGimmickList(GameObject gimmick) {
        gimmicksList.Add(gimmick);
    }

    /// <summary>
    /// すべての敵の移動を一時停止
    /// </summary>
    public void StopMoveAllEnemies() {
        if (enemiesList.Count <= 0) {
            return;
        }

        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].GetComponent<EnemyController>().PauseMove();
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
            enemiesList[i].GetComponent<EnemyController>().ResumeMove();
        }
    }

    /// <summary>
    /// PathData の List を取得
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

    /// <summary>
    /// 敵の情報を List から削除し、ミッション内の敵の残数を減らす
    /// </summary>
    /// <param name="enemy"></param>
    public void RemoveEnemyList(EnemyController enemy) {
        enemiesList.Remove(enemy);

        currentMissionDuration--;
    }

    /// <summary>
    /// ミッションの準備
    /// </summary>
    /// <param name="missionDuration"></param>
    /// <param name="clearConditionsType"></param>
    /// <param name="events"></param>
    /// <param name="eventTrans"></param>
    public void PreparateMission(int missionDuration, ClearConditionsType clearConditionsType, (EventType[] eventTypes, int[] eventNos) events, Transform[] eventTrans) {

        // カメラの移動停止
        fieldAutoScroller.StopAndPlayMotion();

        // ミッションの時間設定
        currentMissionDuration = missionDuration;

        // ミッション内の各イベントの生成(敵、ギミック、トラップ、アイテムなどを生成)
        eventGenerator.GenerateEvents(events, eventTrans);

        // ミッション開始
        StartCoroutine(StartMission(clearConditionsType));
    }

    /// <summary>
    /// ミッション開始
    /// </summary>
    /// <param name="clearConditionsType"></param>
    /// <returns></returns>
    private IEnumerator StartMission(ClearConditionsType clearConditionsType) {

        // ミッションの監視
        yield return StartCoroutine(ObservateMission(clearConditionsType));

        // ミッション終了
        EndMission();
    }

    /// <summary>
    /// ミッションの監視
    /// 各イベントの状態を監視
    /// </summary>
    /// <param name="clearConditionsType"></param>
    /// <returns></returns>
    private IEnumerator ObservateMission(ClearConditionsType clearConditionsType) {

        // クリア条件を満たすまで監視
        while (currentMissionDuration > 0) {

            // 残り時間を監視する場合
            if (clearConditionsType == ClearConditionsType.TimeUp) {

                // カウントダウン
                currentMissionDuration--;
            }

            yield return null;
        }

        Debug.Log("ミッション終了");
    }

    /// <summary>
    /// ミッション終了
    /// </summary>
    public void EndMission() {

        ClearEnemiesList();

        ClearGimmicksList();

        // カメラの移動再開
        fieldAutoScroller.StopAndPlayMotion();
    }

    /// <summary>
    /// 敵の List をクリア
    /// </summary>
    private void ClearEnemiesList() {

        if (enemiesList.Count > 0) {
            for (int i = 0; i < enemiesList.Count; i++) {
                Destroy(enemiesList[i]);
            }
        }

        enemiesList.Clear();
    }

    /// <summary>
    /// ギミックの List をクリア
    /// </summary>
    private void ClearGimmicksList() {

        if (gimmicksList.Count > 0) {
            for (int i = 0; i < gimmicksList.Count; i++) {
                Destroy(gimmicksList[i]);
            }
        }

        gimmicksList.Clear();
    }
}