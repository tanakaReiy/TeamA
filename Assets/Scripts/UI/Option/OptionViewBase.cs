using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OptionViewBase : ViewBase
{
    public virtual async UniTask Show()
    {
        gameObject.SetActive(true);
        var cg = GetCanvasGroup();
        cg.alpha = 1.0f;
        cg.blocksRaycasts = true;
        cg.interactable = true;

    }

    public virtual async UniTask Hide()
    {
        gameObject.SetActive(false);
        var cg = GetCanvasGroup();
        cg.alpha = 0.0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }
}
