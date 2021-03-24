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

    IEnumerator Start() {

        Vector3[] paths = pathDatasList.Select(x => x.pathTran.position).ToArray();
        float totalTime = pathDatasList.Select(x => x.scrollTime).Sum();

        Debug.Log(totalTime);
        tween = transform.DOPath(paths, totalTime);

        yield return null;
    }

    private void Update() {

        // ˆê’â~‚ÆÄŠJˆ—
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isPause) {
                transform.DOPlay();
            } else {
                transform.DOPause();
            }
            isPause = !isPause;
        }
    }
}
