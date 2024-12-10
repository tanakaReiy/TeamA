using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
/// <summary>�ꂩ���������</summary>
public sealed class GuardEnemy : IMovePatternEnemy
{
    [SerializeField] private Vector3 _guardPosition;
    [SerializeField] private float _guardFrontAngle;

    (Vector3 position, float direction)[] IMovePatternEnemy.GetAllTargets()
    {
        return new (Vector3 position, float direction)[] { (_guardPosition , _guardFrontAngle)};
    }
    public (Vector3 position, float direction) GetNextTarget()
    {
        return (_guardPosition, _guardFrontAngle);
    }
    public void Dispose() { }

    public async UniTask NextTargetActionAsync(Quaternion targetRotation, Transform transform, CancellationToken token)
    {
        try
        {
            //�h�q�̌����ɍ��킹�Ĉړ�����
            await LMotion.Create(transform.rotation, targetRotation, 1).WithEase(Ease.InOutCubic).BindToLocalRotation(transform).ToUniTask(token);
            //�ŏ��ɏ����X��
            await LMotion.Create(transform.rotation, targetRotation * Quaternion.AngleAxis(-30, Vector3.up), 1.5f).WithEase(Ease.InOutCubic).BindToLocalRotation(transform).ToUniTask(token);
            //���[�v�ō��E�ɉ�]���Čx��������
            await LMotion.Create(targetRotation * Quaternion.AngleAxis(-30, Vector3.up), targetRotation * Quaternion.AngleAxis(60, Vector3.up), 3f).WithEase(Ease.InOutCubic).WithLoops(-1, loopType: LoopType.Yoyo).BindToLocalRotation(transform).ToUniTask(token);
        }
        catch
        {
            Debug.Log("Cancel NextTargetAction");
        }
    }

    /*
#if UNITY_EDITOR
/// <summary>
/// 
/// �����̉��z�N���X�ȊO�ŌĂяo���Ȃ�����
/// </summary>
[Button]
public void GenerateGuradPosition() 
{

}
#endif
*/
}
