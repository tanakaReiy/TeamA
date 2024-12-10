using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
public class PlayerHealthPresenter : UIGroup
{
    [SerializeField] private List<HelthUIVIew> _helthUIVIews = new();
    
    //[SerializeField]PlayerStatus status;
    //Debug—p
    //–{—ˆ‚ÍPlayerStatus‚ÌHealth‚É‚àSubscribe‚·‚é


    public override void Initialize()
    {
        _helthUIVIews.ForEach(view =>
        {
            view.SetView(WandManager.CaptureAbility.None);
        });

        base.Initialize();
        PlayerEventHelper.OnCaptureAbility
            .Subscribe((ability) =>
            {
                _helthUIVIews.ForEach(view =>
                {
                    view.SetView(ability);
                });
            }).AddTo(this);
    }

}
