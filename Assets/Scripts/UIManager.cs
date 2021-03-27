using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtDebugMessage;

    [SerializeField]
    private Button btnStopMotion;

    [SerializeField]
    private FieldAutoScroller autoScroller;

    void Start() {
        btnStopMotion.onClick.AddListener(OnClickStopMotion);    
    }

    /// <summary>
    /// デバッグ内容を画面表示
    /// </summary>
    /// <param name="message"></param>
    public void DisplayDebug(string message) {
        txtDebugMessage.text = message;
    }

    private void OnClickStopMotion() {
        autoScroller.StopAndPlayMotion();
    }
}

