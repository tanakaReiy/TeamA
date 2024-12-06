using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{

    [Obsolete("���̃N���X�͎g�p���Ȃ��Ȃ�܂����B�R���t���N�g���Ȃ��Ȃ�΍폜���܂�")]
    public interface IPlayerAbility 
    {

    

        bool HasAbility() { return true; }


        [Obsolete()]
        void PerformAbility();

        PlayerBaseState OnPerformAbility(GameObject performer) { return new PlayerAbilityState(performer.GetComponent<PlayerStateMachine>()); }


        
    }
    //�Ȃ�̃A�r���e�B���ݒ肳��Ă��Ȃ����Ƃ�\��Ability�ł�
    public sealed class NoneAbility : IPlayerAbility
    {
        public bool HasAbility() { return false; }

        public PlayerBaseState OnPerformAbility(GameObject performer){ return null; }

        public void PerformAbility() { }
    }

    //�e�X�g�p
    [Serializable]
    public class CandleAbility : IPlayerAbility
    {
        public PlayerBaseState OnPerformAbility(GameObject performer)
        {
            throw new NotImplementedException();
        }

        public void PerformAbility()
        {
            Debug.Log("CandleAbility���ݒ肳��Ă��܂�");
        }
    }

   




}




