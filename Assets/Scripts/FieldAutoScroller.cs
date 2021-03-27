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

    [SerializeField, Tooltip("�ꎞ��~�\��")]
    private int stopMotionCount;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private GameManager gameManager;
 

    IEnumerator Start() {
        yield return null;

        Vector3[] paths = pathDatasList.Select(x => x.pathTran.position).ToArray();
        float totalTime = pathDatasList.Select(x => x.scrollTime).Sum();

        Debug.Log(totalTime);
        tween = transform.DOPath(paths, totalTime).SetEase(Ease.Linear);

        uiManager.UpdateDisplayStopMotionCount(stopMotionCount);
    }

    private void Update() {

        // �ꎞ��~�ƍĊJ����
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

            // �\���X�V
            uiManager.UpdateDisplayStopMotionCount(stopMotionCount);
        }
        isPause = !isPause;
    }
}
