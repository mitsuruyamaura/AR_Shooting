using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    [SerializeField]
    private GameObject objPrefab = null;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private GameObject field;

    private PlaneDetection planeDetection;
    
    private GameObject obj;

    private ARRaycastManager raycastManager;

    private List<ARRaycastHit> raycastHitList = new List<ARRaycastHit>();



    public enum ARState {
        None,
        Tracking,     // 平面感知中
        Wait,         // 待機。どこのステートにも属さない状態
        Ready,        // ゲーム準備中
        Play,         // ゲーム中
        GameUp,       // ゲーム終了

    }

    public ARState currentARState;


    void Awake() {
        raycastManager = GetComponent<ARRaycastManager>();

        planeDetection = GetComponent<PlaneDetection>();

        //currentARState = ARState.Tracking;
    }


    void Update()
    {
        if (currentARState == ARState.None) {
            return;
        }

        if (Input.touchCount < 0) {
            return;        
        }

        if (currentARState == ARState.Tracking) {
            // 平面感知
            TrackingPlane();
        } else if (currentARState == ARState.Ready) {
            currentARState = ARState.Wait;
            uiManager.DisplayDebug(currentARState.ToString());

            // ゲーム開始の準備
            StartCoroutine(PraparateGameReady());
        } else if (currentARState == ARState.Play) {
            uiManager.DisplayDebug(currentARState.ToString());
        }
    }

    /// <summary>
    /// 平面感知とPlane生成
    /// </summary>
    private void TrackingPlane() {
        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Ended) {
            return;
        }

        if (raycastManager.Raycast(touch.position, raycastHitList, TrackableType.PlaneWithinPolygon)) {
            Pose hitPose = raycastHitList[0].pose;

            if (obj == null) {
                uiManager.DisplayDebug("Raycast 成功");
                //obj = Instantiate(objPrefab, hitPose.position, hitPose.rotation);

                field.SetActive(true);

                currentARState = ARState.Ready;
            } else {
                uiManager.DisplayDebug("Raycast 済");
                obj.transform.position = hitPose.position;
            }
        } else {
            uiManager.DisplayDebug("RayCast 失敗");
        }
    }

    /// <summary>
    /// ゲーム開始の準備
    /// </summary>
    private IEnumerator PraparateGameReady() {

        // TODO 準備処理を書く

        yield return new WaitForSeconds(2.0f);

        currentARState = ARState.Play;

        // 平面検知を非表示
        planeDetection.SetAllPlaneActivate(false);
    }
}
