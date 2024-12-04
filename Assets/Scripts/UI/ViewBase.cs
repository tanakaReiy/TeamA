using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class ViewBase : MonoBehaviour
{
    private CanvasGroup _canvasGroup = null;

    public CanvasGroup GetCanvasGroup()
    {
        if (_canvasGroup == null) { _canvasGroup = GetComponent<CanvasGroup>(); }
        return _canvasGroup;
    }

    public abstract void Entry();
}
