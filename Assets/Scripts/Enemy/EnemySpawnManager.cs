using Alchemy.Inspector;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �G�l�~�[�̐����Ǘ��A�������A���󂯎���
/// </summary>
public class EnemySpawnManager : MonoBehaviour
{
    [LabelText("���̃V�[���Ő�������G�l�~�[�̃f�[�^���X�g")]
    [SerializeField] private List<EnemyData> _enemyData = new List<EnemyData>();
    /// <summary>�@���݂̃G�l�~�[�̐����^�C�}�[���܂Ƃ߂��z��@</summary>
    private float[] _spawnTimerArr = new float[0];
    /// <summary>�@�G�l�~�[�̐����񐔂��܂Ƃ߂��z��@</summary>
    private int[] _enemyGenerateCountArr = new int[0];
    /// <summary>�@���݂̃V�[����̃G�l�~�[�����݂��邩���܂Ƃ߂��z��@</summary>
    private bool[] _isEnemyExistArr = new bool[0];

    [LabelText("�G�l�~�[�̃X�|�[���ʒu��\������Box�̐F")]
    [FoldoutGroup("Gizmos Settings")][SerializeField] private Color _spawnPointColor = new Color(1, 0, 0, 0.7f);
    [LabelText("�G�l�~�[�̃X�|�[���ʒu��\������Box�̑傫��")]
    [FoldoutGroup("Gizmos Settings")][SerializeField] private Vector3 _spawnPointScale = Vector3.one;

    [LabelText("�G�l�~�[�̈ړ��ڕW����\������Box�̐F")]
    [FoldoutGroup("Gizmos Settings")][SerializeField] private Color _targetPointColor = new Color(1, 0, 1, 0.5f);
    [LabelText("�G�l�~�[�̈ړ��ڕW��\������Box�̑傫��")]
    [FoldoutGroup("Gizmos Settings")][SerializeField] private Vector3 _targetPointScale = Vector3.one;
    private void Start()
    {
        //�����G�l�~�[�f�[�^������Ȃ��A��������EnemyPrefabLoader�����݂��Ȃ���Ύ��g���폜
        if (_enemyData.Count == 0 || EnemyPrefabLoader.Instance == null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        //���ꂼ��z���������
        _spawnTimerArr = new float[_enemyData.Count];
        _enemyGenerateCountArr = new int[_enemyData.Count];
        _isEnemyExistArr = new bool[_enemyData.Count];
        //�ŏ��͂����ɃX�|�[������悤�Ƀ^�C�}�[��������
        for (int i = 0; i < _enemyData.Count; i++)
        {
            _spawnTimerArr[i] = _enemyData[i].GenerateInterval - 1;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _enemyData.Count; i++)
        {
            //�^�C�}�[���Z
            _spawnTimerArr[i] += Time.deltaTime;
            if (IsSpawnableEnemy(i))
            {
                //���ۂɃ��[�h����
                GameObject? loadedObject = EnemyPrefabLoader.Instance.GetEnemyPrefab(_enemyData[i].Enemy);
                //null�łȂ���΃G�l�~�[������������
                if (loadedObject != null)
                {
                    InitializeSpawnedEnemy(loadedObject, i);
                }
            }
        }
    }
    /// <summary>
    /// �G�l�~�[���X�|�[���ł��邩����
    /// </summary>
    private bool IsSpawnableEnemy(int index)
    {
        return _enemyData[index].SpawnablePlayerDistanceSquare >= (PlayerManager.Instance.transform.position - _enemyData[index].SpawnPoint).sqrMagnitude
                && !_isEnemyExistArr[index]
                && (_enemyData[index].MaxGenerateCnt <= -1  || _enemyGenerateCountArr[index] < _enemyData[index].MaxGenerateCnt )
                && _spawnTimerArr[index] >= _enemyData[index].GenerateInterval;
    }

    private void InitializeSpawnedEnemy(GameObject enemyObject, int index)
    {
        //�G�l�~�[����
        GameObject instantiatedObject = GameObject.Instantiate(enemyObject);
        //�X�|�[���ɔ����l�̕ϓ�
        _enemyGenerateCountArr[index]++;
        _isEnemyExistArr[index] = true;
        _spawnTimerArr[index] = 0;
        //�G�l�~�[�����̏�����
        instantiatedObject.transform.position = _enemyData[index].SpawnPoint;
        instantiatedObject.transform.rotation *= Quaternion.AngleAxis(_enemyData[index].SpawnedEnemyRotationY, Vector3.up);
        EnemyBase enemyBase = instantiatedObject.GetComponent<EnemyBase>();
        if (!enemyBase)
        {
            enemyBase = instantiatedObject.AddComponent<EnemyBase>();
        }
        //�G�l�~�[���j�������ۂɍs��������o�^
        enemyBase._disposeAction += RegisterAction;
        enemyBase.GetNextPosition += _enemyData[0].MovePatern.GetNextTarget;
        enemyBase.GetNextGoalAction += _enemyData[0].MovePatern.NextTargetActionAsync;

        void RegisterAction()
        {
            _isEnemyExistArr[index] = false;
            _enemyData[index].MovePatern.Dispose(); 
            enemyBase.GetNextPosition -= _enemyData[0].MovePatern.GetNextTarget;
            enemyBase.GetNextGoalAction -= _enemyData[0].MovePatern.NextTargetActionAsync;
            enemyBase._disposeAction -= RegisterAction;
        }

        enemyBase.Initialize();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _enemyData.Count; i++)
        {
            if (_enemyData[i].IsDrawTargetsPosition)
            {
                //�G�l�~�[�̃X�|�[���|�C���g�̕`��
                Gizmos.color = _spawnPointColor;
                Vector3 centerSpawnPoint = _enemyData[i].SpawnPoint + Vector3.up * _spawnPointScale.y * 0.5f;
                Gizmos.DrawCube(centerSpawnPoint, _spawnPointScale);
                //�G�l�~�[�̐��ʂ�`��
                Gizmos.DrawLine(centerSpawnPoint, centerSpawnPoint + Quaternion.AngleAxis(_enemyData[i].SpawnedEnemyRotationY, Vector3.up) * Vector3.forward);
                //null�`�F�b�N
                if (_enemyData[i].MovePatern == null) { continue; };
                //�G�l�~�[�̏���n�̕`��
                Gizmos.color = _targetPointColor;
                foreach ((Vector3 position, float rotationEuler) targetData in _enemyData[i].MovePatern.GetAllTargets())
                {
                    Vector3 centerTarget = targetData.position + Vector3.up * _targetPointScale.y * 0.5f;
                    Gizmos.DrawCube(centerTarget, _targetPointScale);
                    Gizmos.DrawLine(centerTarget, centerTarget + Quaternion.AngleAxis(targetData.rotationEuler, Vector3.up) * Vector3.forward);
                }
            }
        }
    }
#endif
}
