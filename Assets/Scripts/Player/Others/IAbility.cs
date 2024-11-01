using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ability
{
    //プレイヤーのアビリティ(スキル)を抽象的に扱うためのinterfaceです
    public interface IPlayerAbility 
    {
        /// <summary>
        /// 現在のAbilityがNoneAbility(アビリティが設定されていない状態)でないときtrueを返しmasu
        /// </summary>
        /// <returns></returns>
        bool HasAbility() { return true; }

        /// <summary>
        /// 設定されたアビリティを実行します
        /// </summary>
        [Obsolete()]
        void PerformAbility();

        /// <summary>
        /// アビリティ使用時、この関数が呼ばれた後にPlayerのステートが戻り値に切り替わります
        /// </summary>
        /// <param name="performer"></param>
        /// <returns></returns>
        PlayerBaseState OnPerformAbility(GameObject performer) { return null; }
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


