using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private EventTriggerPoint[] eventTriggerPoint;

    [SerializeField]
    private List<GameObject> enemiesList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> gimmicksList = new List<GameObject>();

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private PathDataSO pathDataSO;

    [SerializeField]
    private FieldAutoScroller fieldAutoScroller;

    [SerializeField]
    private UIManager uiManager;

    [System.Serializable]
    public class RootEventData {
        public int[] rootEventNos;
        public BranchDirectionType[] branchDirectionTypes;  // ����̕���
        public RootType rootType;
    }

    [SerializeField]
    private List<RootEventData> rootDatasList = new List<RootEventData>();

    private int currentRailCount;       // ���݂̐i�s��


    IEnumerator Start() {


        // �Q�[���̏���
        yield return StartCoroutine(PreparateGame());

        // ���̃��[�g�̊m�F�Ɛݒ�
        yield return StartCoroutine(CheckNextRootBranch());


    }

    /// <summary>
    /// �Q�[���̏���
    /// </summary>
    /// <returns></returns>
    private IEnumerator PreparateGame() {
        for (int i = 0; i < eventTriggerPoint.Length; i++) {
            eventTriggerPoint[i].SetUpEventTriggerPoint(this);
        }
        yield return null;
    }

    /// <summary>
    /// �G�l�~�[�̐����C�x���g
    /// </summary>
    /// <param name="enemyEventData"></param>
    /// <param name="enemyEventTran"></param>
    public void GenerateEnemy(EventDataSO.EventData enemyEventData, Transform enemyEventTran) {
        GameObject enemy = Instantiate(enemyEventData.eventPrefab, enemyEventTran);
        enemy.GetComponent<AnimalController>().SetUpAnimalController(player);
        enemiesList.Add(enemy);
    }

    /// <summary>
    /// �M�~�b�N�̐����C�x���g
    /// </summary>
    /// <param name="enemyEventData"></param>
    /// <param name="enemyEventTran"></param>
    public void GenerateGimmick(EventDataSO.EventData enemyEventData, Transform enemyEventTran) {
        GameObject enemy = Instantiate(enemyEventData.eventPrefab, enemyEventTran);
        gimmicksList.Add(enemy);
    }

    /// <summary>
    /// ���ׂĂ̓G�̈ړ����ꎞ��~
    /// </summary>
    public void StopMoveAllEnemies() {
        if (enemiesList.Count <= 0) {
            return;
        }

        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].GetComponent<AnimalController>().StopMoveAnimal();
        }
    }

    /// <summary>
    /// ���ׂĂ̓G�̈ړ����ĊJ
    /// </summary>
    public void ResumeMoveAllEnemies() {
        if (enemiesList.Count <= 0) {
            return;
        }

        for (int i = 0; i < enemiesList.Count; i++) {
            enemiesList[i].GetComponent<AnimalController>().ResumeMoveAnimal();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="checkRootNo"></param>
    /// <returns></returns>
    private List<PathData> GetPathDatasList(int checkRootNo) {
        return pathDataSO.rootDatasList.Find(x => x.rootNo == checkRootNo).pathDatasList;
    }

    /// <summary>
    /// ���[�g�̊m�F
    /// ���򂪂���ꍇ�ɂ͕���̖��{�^���𐶐�
    /// </summary>
    /// <returns></returns>
    public IEnumerator CheckNextRootBranch() {

        if (currentRailCount >= rootDatasList.Count) {
            // TODO �N���A����
            Debug.Log("�N���A");

            yield break;
        }

        // ���݂̃��[���J�E���g�� RootType ���m�F���āA���ɔ������郋�[�g�����߂�
        switch (rootDatasList[currentRailCount].rootType) {
            case RootType.Normal_Battle:
                // ���̃��[�g���P�Ȃ�
                if (rootDatasList[currentRailCount].rootEventNos.Length == 1) {
                    // �����I�Ƀ��[���ړ����J�n
                    fieldAutoScroller.SetNextField(GetPathDatasList(rootDatasList[currentRailCount].rootEventNos[0]));
                    Debug.Log("����Ȃ��̈ړ��J�n");
                } else {
                    // ���򂪂���ꍇ�A����C�x���g�𔭐������āA��ʏ�ɖ��̃{�^����\��
                    yield return StartCoroutine(uiManager.GenerateBranchButtons(rootDatasList[currentRailCount].rootEventNos, rootDatasList[currentRailCount].branchDirectionTypes));

                    // ��󂪉������܂őҋ@(while �ł�OK)
                    yield return new WaitUntil(() => uiManager.GetSubmitBranch().Item1 == true);

                    // �I����������̃��[�g��ݒ�
                    fieldAutoScroller.SetNextField(GetPathDatasList(uiManager.GetSubmitBranch().Item2));
                }
                
                break;

            case RootType.Boss_Battle:

                break;

            case RootType.Event:

                break;
        }

        
        // ���̂��߂ɃA�b�v
        currentRailCount++;
    }
}