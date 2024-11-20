using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class UI_Parts_Base : MonoBehaviour
{
    [LabelText("��������UI�̗D��x"),Tooltip("��SortOrder�̃L�����o�X�ł͑傫���قǏ�ɗ��܂�")]
    [SerializeField] private int _uiMergePriority;

    private CanvasGroup _canvasGroup = null;

    public CanvasGroup GetCanvasGroup()
    {
        if (_canvasGroup == null) { _canvasGroup = GetComponent<CanvasGroup>(); }
        return _canvasGroup; 
    }
}
