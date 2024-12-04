using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
public class MessageUIPrensenter : UIGroup
{
    [SerializeField] private Text _captureMessageText;
    [SerializeField] private Text _abilityMessageText;
    [SerializeField] private Capturaing _capturaing;
    [SerializeField] private WandManager _wandManager;
    public override void Initialize()
    {
        base.Initialize();
        _capturaing.OnCurrentTargetChanged
            .Subscribe(capturable =>
            {
                if (capturable == null)
                {
                    _captureMessageText.gameObject.SetActive(false);
                }
                else
                {
                    _captureMessageText.gameObject.SetActive(true);
                }
            }).AddTo(this);
        _wandManager.OnCurrentTargetChanged
            .Subscribe(detected =>
            {
                if (detected == null)
                {
                    _abilityMessageText.gameObject.SetActive(false);
                }
                else
                {
                    _abilityMessageText.gameObject.SetActive(true);
                }
            }).AddTo(this);
    }
}
