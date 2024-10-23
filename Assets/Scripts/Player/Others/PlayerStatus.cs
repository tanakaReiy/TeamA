using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Ability;
//仮プレイヤーステータス
public class PlayerStatus : MonoBehaviour
{

    private void Awake()
    {
        Ability = new NoneAbility();

        Health = new ObservableStatus(100, 100);

        InputReader.Instance.OnSkillAsObservable()
            .Where(c => c.performed)
            .Subscribe(_ =>
            {
                Ability.PerformAbility();
            }).AddTo(this);
    }


    public ObservableStatus Health { get; private set; } = null;
    public IPlayerAbility Ability { get; set; }
}
