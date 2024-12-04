using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class OptionExitView : OptionViewBase
{
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    public IObservable<Unit> OnExitConfirmed => _confirmButton.onClick.AsObservable();
    public IObservable<Unit> OnExitCancel => _cancelButton.onClick.AsObservable();

    public override void Entry()
    {
        
    }
}
