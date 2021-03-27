using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class FieldAutoScroller : MonoBehaviour
{
    [System.Serializable]
    public class PathData {
        public float scrollTime;
        public Transform pathTran;
    }

    [SerializeField]
    private List<PathData> pathDatasList = new List<PathData>();

    Tween tween;

    bool isPause;

    [SerializeField, Tooltip("一時停止可能回数")]
    private int stopMotionCount;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private GameManager gameManager;

    public Vector3 targetPos;
    public int currentTargetPathCount;

    IEnumerator Start() {
        yield return null;

        Vector3[] paths = pathDatasList.Select(x => x.pathTran.position).ToArray();
        float totalTime = pathDatasList.Select(x => x.scrollTime).Sum();

        Debug.Log(totalTime);
        tween = transform.DOPath(paths, totalTime).SetEase(Ease.Linear);

        uiManager.UpdateDisplayStopMotionCount(stopMotionCount);

        currentTargetPathCount = 1;
        targetPos = pathDatasList[currentTargetPathCount].pathTran.position;

        //transform.LookAt(pathDatasList[0].pathTran);

        //StartCoroutine(CheckPath());
    }

    private IEnumerator CheckPath() {
        for (int i = 0; i < pathDatasList.Count; i++) {
            targetPos = pathDatasList[i].pathTran.position;
            yield return new WaitUntil(() => transform.position == targetPos);            
        }
    }

    private void Update() {
        //if (targetPos.z <= transform.position.z && currentTargetPathCount < pathDatasList.Count - 1) {
        //    currentTargetPathCount++;
        //    targetPos = pathDatasList[currentTargetPathCount].pathTran.position;
        //}

        Vector3 direction = transform.position - pathDatasList[0].pathTran.position;
        direction.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);

        // 一時停止と再開処理
        if (Input.GetKeyDown(KeyCode.Space)) {
            StopAndPlayMotion();
        }
    }

    public void StopAndPlayMotion() {
        if (isPause) {
            transform.DOPlay();
            gameManager.ResumeMoveAllEnemies();

        } else if (!isPause && stopMotionCount > 0) {
            transform.DOPause();
            gameManager.StopMoveAllEnemies();

            stopMotionCount--;

            // 表示更新
            uiManager.UpdateDisplayStopMotionCount(stopMotionCount);
        }
        isPause = !isPause;
    }
}
