using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{

    [Obsolete("このクラスは使用しなくなりました。コンフリクトしなくなれば削除します")]
    public interface IPlayerAbility 
    {

    

        bool HasAbility() { return true; }


        [Obsolete()]
        void PerformAbility();

        PlayerBaseState OnPerformAbility(GameObject performer) { return new PlayerAbilityState(performer.GetComponent<PlayerStateMachine>()); }


        
    }
    //なんのアビリティも設定されていないことを表すAbilityです
    public sealed class NoneAbility : IPlayerAbility
    {
        public bool HasAbility() { return false; }

        public PlayerBaseState OnPerformAbility(GameObject performer){ return null; }

        public void PerformAbility() { }
    }

    //テスト用
    [Serializable]
    public class CandleAbility : IPlayerAbility
    {
        public PlayerBaseState OnPerformAbility(GameObject performer)
        {
            throw new NotImplementedException();
        }

        public void PerformAbility()
        {
            Debug.Log("CandleAbilityが設定されています");
        }
    }

   




}




