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
    /// ARPlane �̕\���I���I�t�؂�ւ�
    /// </summary>
    public void SetAllPlaneActivate(bool isSwitch) {
        isPlaneVisible = isSwitch;
    }
}
