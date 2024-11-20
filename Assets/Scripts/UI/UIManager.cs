using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// すべてのUIを管理します
/// シングルトン、DontDestoryOnLoadであり残り続けます
/// すべてのUIはUIManagerが保持する各キャンバス以下に配置されます
/// UI_Merge_Canvasを使うことで任意の位置からUIManagerに結合できます
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
