using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    [SerializeField]
    private GameObject objPrefab = null;

    private GameObject obj;

    private ARRaycastManager raycastManager;

    private List<ARRaycastHit> raycastHitList = new List<ARRaycastHit>();


    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }


    void Update()
    {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            if (touch.phase != TouchPhase.Ended) {
                return;
            }

            if (obj == null) {
                if (raycastManager.Raycast(touch.position, raycastHitList, TrackableType.PlaneWithinPolygon)) {
                    Debug.Log("Raycast成功");

                    ///Vector3 posY = new Vector3(raycastHitList[0].pose.position.x, raycastHitList[0].pose.position.y + 0.3f, raycastHitList[0].pose.position.z + 1f);

                    Pose hitPose = raycastHitList[0].pose;

                    obj = Instantiate(objPrefab, hitPose.position, hitPose.rotation);
                } else {
                    Debug.Log("RayCast失敗");
                }
            } 
        }
    }
}
