using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class EnemyMoveData
{
    [LabelText("�G�l�~�[�̏���ʒu��\�����邩")]
    public bool _isDrawTargetsPosition = false;

    [LabelText("�G�l�~�[�̃v���n�u")]
    public GameObject _enemyPrefab;

    [LabelText("�G�l�~�[�̃X�|�[���ʒu")]
    public Vector3 _spawnPoint = Vector3.zero;

    [LabelText("��������� / -1��I�ԂƖ����ɐ���")]
    public int _mamGenerateCnt = 0;

    [LabelText("�V�[���ɑ��݂ł���ő吔")]
    public int _maxEnemyCnt = 0;

    [LabelText("�G�l�~�[�̋���")]
    [SerializeReference] public IMovePatternEnemy _movePatern;
}

public interface IMovePatternEnemy
{
    public Vector3[] GetAllTargets();
    public (Vector3 position, Vector3 direction) GetNextTarget();
    /// <summary>
    /// �G�l�~�[�̎��̖ړI�n�Ɉڍs����ۂ̉�]�𐧌䂷��
    /// </summary>
    /// <param name="rotation">���̖ړI�n�ɂĐݒ肳��Ă������</param>
    /// <param name="transform">�G�l�~�[��transform</param>
    /// <param name="token">�L�����Z���p�g�[�N��</param>
    /// <returns></returns>
    public UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token);
}