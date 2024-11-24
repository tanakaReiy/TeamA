using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
#if UNITY_EDITOR
    [LabelText("�G�l�~�[�̏���ʒu��\�����邩")]
    public bool IsDrawTargetsPosition = false;
#endif
    /// <summary>
    /// �����`���̂���Enum���g�p���������Ɋ��S�Ɉڍs������폜����܂�
    /// </summary>
    [LabelText("�G�l�~�[�̃v���n�u")]
    public GameObject EnemyPrefab;

    [LabelText("�G�l�~�[�̎��")]
    public EnemyEnum Enemy;

    [LabelText("�G�l�~�[�̃X�|�[���ʒu")]
    public Vector3 SpawnPoint = Vector3.zero;

    [LabelText("�G�l�~�[�̃X�|�[�����̌���")]
    public float SpawnedEnemyRotationY = 0;

    [LabelText("��������� / -1��I�ԂƖ����ɐ���")]
    public int MaxGenerateCnt = 0;

    [LabelText("�����ɂ����鎞��")]
    public float GenerateInterval = 5;

    [LabelText("�����\�ȃv���C���[�Ƃ̋����̓��")]
    public float SpawnablePlayerDistanceSquare = 256;

    [LabelText("�G�l�~�[�̋���")]
    [SerializeReference] public IMovePatternEnemy MovePatern;
}

public interface IMovePatternEnemy
{
    public (Vector3 position, float direction)[] GetAllTargets();
    public (Vector3 position, float direction) GetNextTarget();

    public void OnDisposed();
    /// <summary>
    /// �G�l�~�[�̎��̖ړI�n�Ɉڍs����ۂ̉�]�𐧌䂷��
    /// </summary>
    /// <param name="rotation">���̖ړI�n�ɂĐݒ肳��Ă������</param>
    /// <param name="transform">�G�l�~�[��transform</param>
    /// <param name="token">�L�����Z���p�g�[�N��</param>
    /// <returns></returns>
    public UniTask NextTargetActionAsync(Quaternion rotation, Transform transform, CancellationToken token);
}