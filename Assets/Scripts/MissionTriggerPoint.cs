using UnityEngine;

public class MissionTriggerPoint : MonoBehaviour {

    [Header("ミッションのクリア条件")]
    public ClearConditionsType clearConditionsType;

    [Header("ミッションクリアのための敵の残数/残り時間")]
    public int missionDuration;

    [SerializeField, Header("発生するイベントの種類")]
    private EventType[] eventTypes;

    [SerializeField, Header("発生するイベントの番号")]
    private int[] eventNos;

    [SerializeField, Tooltip("イベントの生成地点")]
    private Transform[] eventTrans;

    private BoxCollider boxCollider;
    private GameManager gameManager;

    /// <summary>
    /// EventTriggerPoint の準備
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpMissionTriggerPoint(GameManager gameManager) {
        this.gameManager = gameManager;

        TryGetComponent(out boxCollider);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Debug.Log("通過");

            // ミッション発生の重複判定防止
            boxCollider.enabled = false;
            
            // ミッション開始の準備
            gameManager.PreparateMission(missionDuration, clearConditionsType, (eventTypes, eventNos), eventTrans);
        }
    }
}