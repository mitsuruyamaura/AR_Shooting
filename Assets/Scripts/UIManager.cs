using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtDebugMessage;


    //mi

    [SerializeField]
    private Button btnStopMotion;

    [SerializeField]
    private FieldAutoScroller autoScroller;

    [SerializeField]
    private Text txtStopMotionCount;

    [SerializeField]
    private SubmitBranchButton submitBranchButtonPrefab;

    [SerializeField]
    private List<SubmitBranchButton> submitBranchButtonsList = new List<SubmitBranchButton>();

    [SerializeField]
    private Transform rightBranchTran;

    [SerializeField]
    private Transform leftBranchTran;

    [SerializeField]
    private Transform centerBranchTran;

    [SerializeField]
    private bool isSubmitBranch;

    [SerializeField]
    private int submitBranchNo;

    [SerializeField]
    private Text txtARIntroduction;


    /// <summary>
    /// �f�o�b�O���e����ʕ\��
    /// </summary>
    /// <param name="message"></param>
    public void DisplayDebug(string message) {
        txtDebugMessage.text = message;
    }


    // mi


    void Start() {
        btnStopMotion.onClick.AddListener(OnClickStopMotion);    
    }


    /// <summary>
    /// �ꎞ��~�����̎��s
    /// </summary>
    private void OnClickStopMotion() {
        autoScroller.StopAndPlayMotion();
    }

    /// <summary>
    /// �ꎞ��~�ł���c��񐔂̕\���X�V
    /// </summary>
    /// <param name="stopMotionCount"></param>
    public void UpdateDisplayStopMotionCount(int stopMotionCount) {
        txtStopMotionCount.text = stopMotionCount.ToString();
    }

    /// <summary>
    /// ����̃{�^���쐬
    /// </summary>
    public IEnumerator GenerateBranchButtons(int[] branchNums, BranchDirectionType[] branchDirectionTypes) {

        isSubmitBranch = false;
        Debug.Log("����̃{�^���쐬");

        // ����̐������{�^���𐶐�
        for (int i = 0; i < branchNums.Length; i++) {

            // �{�^���̐����ʒu��ݒ�
            Transform branchTran = BranchDirectionType.Right == branchDirectionTypes[i] ? rightBranchTran : BranchDirectionType.Left == branchDirectionTypes[i] ? leftBranchTran : centerBranchTran;
            
            // �{�^������
            SubmitBranchButton submitBranchButton = Instantiate(submitBranchButtonPrefab, branchTran, false);

            // �{�^���ݒ�
            submitBranchButton.SetUpSubmitBranchButton(branchNums[i], this);

            // List �ɒǉ�
            submitBranchButtonsList.Add(submitBranchButton);
        }
        yield return null;
    }

    /// <summary>
    /// �����̌���
    /// </summary>
    /// <param name="rootNo"></param>
    public void SubmitBranch(int rootNo) {
        for (int i = 0; i < submitBranchButtonsList.Count; i++) {
            // ����̃{�^����񊈐������ďd���^�b�v��h�~
            submitBranchButtonsList[i].InactivateSubmitButton();
            Destroy(submitBranchButtonsList[i].gameObject);
        }
        submitBranchButtonsList.Clear();

        submitBranchNo = rootNo;
        isSubmitBranch = true;
    }

    /// <summary>
    /// ������̎擾
    /// </summary>
    /// <returns></returns>
    public (bool, int) GetSubmitBranch() {
        return (isSubmitBranch, submitBranchNo);
    }

    /// <summary>
    /// AR ���������̃��b�Z�[�W�\��
    /// </summary>
    public void DisplayARIntroduction(string message) {
        txtARIntroduction.text = message;
    }

    /// <summary>
    /// AR �����p�̃��b�Z�[�W�\���̃I��/�I�t�؂�ւ�
    /// </summary>
    /// <param name="isSwicth"></param>
    public void InactiveARIntroductionText(bool isSwicth) {
        txtARIntroduction.transform.parent.parent.gameObject.SetActive(isSwicth);
    }
}

