using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UIGroup : MonoBehaviour
{
    [LabelText("結合時のUIの優先度"), Tooltip("同SortOrderのキャンバスでは大きいほど上に来ます")]
    [field: SerializeField] public int MergePriority { get; private set; }

    private CanvasGroup _canvasGroup = null;

    public CanvasGroup GetCanvasGroup()
    {
        if (_canvasGroup == null) { _canvasGroup = GetComponent<CanvasGroup>(); }
        return _canvasGroup; 
    }

    /// <summary>
    /// UIManagerに統合された際に実行されます
    /// </summary>
    public virtual void Initialize() { }
}
