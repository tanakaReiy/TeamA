using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̃C���^�[�t�F�C�X���p������MonoBehavior�N���X�̓A�r���e�B�̉e�����󂯂���悤�ɂȂ�܂�
/// �e�����N������Ability�̎�ނ�OnAbilityDetect�̈����Ŕ��f���Ă�������
/// ���o�̂��߂ɃR���C�_�[�͕K�v�ł�
/// </summary>
public interface IAbilityDetectable : IDetectable
{
    void OnAbilityDetect(WandManager.CaptureAbility ability);
}
