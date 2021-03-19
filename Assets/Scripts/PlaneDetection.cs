using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneDetection : MonoBehaviour
{
    private ARPlaneManager arPlaneManager;
    private bool isPlaneVisible = true;

    private void Awake() {
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    void Update()
    {
        foreach (var plane in arPlaneManager.trackables) {
            plane.gameObject.SetActive(isPlaneVisible);
        }
    }

    /// <summary>
    /// ARPlane の表示オンオフ切り替え
    /// </summary>
    public void SetAllPlaneActivate(bool isSwitch) {
        isPlaneVisible = isSwitch;
    }
}
