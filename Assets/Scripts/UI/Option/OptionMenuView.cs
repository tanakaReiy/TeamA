using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuView : OptionViewBase
{
    [SerializeField] private Button _toIntroButton;
    [SerializeField] private Button _toSettingButton;
    [SerializeField] private Button _toExitButton;
    [SerializeField] private Button _cancelButton;

    public IObservable<Unit> OnIntroButtonPressed => _toIntroButton.onClick.AsObservable();
    public IObservable<Unit> OnSettingButtonPressed => _toSettingButton.onClick.AsObservable();
    public IObservable<Unit> OnExitButtonPressed => _toExitButton.onClick.AsObservable();
    public IObservable<Unit> OnOptionCancel => _cancelButton.onClick.AsObservable();


    public override void Entry()
    {
        throw new NotImplementedException();
    }
}
