using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

//仮プレイヤーステータス
public class PlayerStatus : MonoBehaviour
{
    public enum CapureAbility
    {
        None = 0,ExampleAbility,
    }
    private void Awake()
    {

        Health = new ObservableStatus(100, 100);

        InputReader.Instance.OnSkillAsObservable()
            .Where(c => c.performed)
            .Subscribe(_ =>
            {
                switch (Ability)
                {
                    case CapureAbility.None:
                        print("スキルはありません");
                        break;
                    case CapureAbility.ExampleAbility:
                        print("ExampleSkillが設定されています");
                        break;
                }
            }).AddTo(this);
    }


    public ObservableStatus Health { get; private set; } = null;
    public CapureAbility Ability { get; set; } = CapureAbility.None;
}
