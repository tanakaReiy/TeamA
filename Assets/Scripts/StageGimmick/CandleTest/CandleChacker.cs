using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CandleChacker : StageGimmickBase
{
    [LabelText("ギミックのろうそく")]
    [SerializeField] private List<CandleAppearanceChanger> candleAppearanceChangers;

    private Dictionary<CandleAppearanceChanger, bool> candleDictionary = new Dictionary<CandleAppearanceChanger, bool>();

    private void Start()
    {
        if (candleAppearanceChangers != null)
        {
            foreach(var candle in candleAppearanceChangers)
            {
                candleDictionary.Add(candle, candle.IsFiredCorrect);
                candle.OnStateChanged += CheckIfClear;
            }
        }
        else
        {
            Debug.Log("ロウソクが登録されていません");
        }
    }

    //クリア判定を行う
    private void CheckIfClear()
    {
        foreach( var candle in candleDictionary.Keys)
        {
            if(candle.IsFiredCorrect != candleDictionary[candle])
            {
                return;
            }
        }

        ClearActive(true);
    }

    private void OnDestroy()
    {
        // イベントからハンドラを解除してメモリリークを防止
        foreach (CandleAppearanceChanger candleAppearance in candleAppearanceChangers)
        {
            if (candleAppearance != null)
            {
                candleAppearance.OnStateChanged -= CheckIfClear;
            }
        }
    }
}
