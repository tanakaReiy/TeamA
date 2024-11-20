using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���ׂĂ�UI���Ǘ����܂�
/// �V���O���g���ADontDestoryOnLoad�ł���c�葱���܂�
/// ���ׂĂ�UI��UIManager���ێ�����e�L�����o�X�ȉ��ɔz�u����܂�
/// UI_Merge_Canvas���g�����ƂŔC�ӂ̈ʒu����UIManager�Ɍ����ł��܂�
/// 
/// </summary>
public class UIManager : SingletonMonoBehavior<UIManager>
{

    private List<CanvasData> _regsteredCanvas;

    public void RegisterCanvas(Canvas canvas,bool isSerachEnable = true)
    {
        _regsteredCanvas.Add(new CanvasData(canvas,isSerachEnable));
    }



    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private class CanvasData
    {
        public CanvasData(Canvas canvas, bool isSerachEnable)
        {
            Canvas = canvas;
            IsSerachEnable = isSerachEnable;
        }

        public Canvas Canvas { get;set; }
        public bool IsSerachEnable { get; set; }
    }


    public enum UiMergeType
    {
        Normal,Independent
    }
}
