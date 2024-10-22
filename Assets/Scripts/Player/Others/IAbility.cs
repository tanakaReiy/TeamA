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
        void PerformAbility();
    }
    //なんのアビリティも設定されていないことを表すAbilityです
    public sealed class NoneAbility : IPlayerAbility
    {
        public bool HasAbility() { return false; }
        public void PerformAbility() { }
    }

    //テスト用
    public class CandleAbility : IPlayerAbility
    {
        public void PerformAbility()
        {
            Debug.Log("CandleAbilityが設定されています");
        }
    }
}


