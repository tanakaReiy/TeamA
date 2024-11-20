using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UI_Parts_Base : MonoBehaviour
{
    [LabelText("結合時のUIの優先度"),Tooltip("同SortOrderのキャンバスでは大きいほど上に来ます")]
    [SerializeField] private int _uiMergePriority;

    private CanvasGroup _canvasGroup = null;

    public CanvasGroup GetCanvasGroup()
    {
        if (_canvasGroup == null) { _canvasGroup = GetComponent<CanvasGroup>(); }
        return _canvasGroup; 
    }
}
