using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitMotion;
using LitMotion.Extensions;
using UniRx;
using TMPro;
using System.Xml;
using Unity.Collections;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
public class TitleTest : MonoBehaviour
{
    [SerializeField]
    TMP_Text _txt;
    [SerializeField]
    [Multiline(5)]
    string[] _title;
    [SerializeField]
    Button _button;
    private int _count = 0;
    readonly CompositeMotionHandle _handle = new();
    private void Start()
    {
        _button.OnClickAsAsyncEnumerable()
            .Subscribe(_ => TextAnimation(_count))
            .AddTo(this);
        _count = Mathf.Clamp(_count, 0, _title.Length - 1);
    }
    private void TextAnimation(int count)
    {
        _handle.Complete();
        _txt.text = "";
        LMotion.String.Create512Bytes("", _title[count], 0.1f * _title.Length) //開始時文字列,終了時文字列,Animaiton速度
            .WithRichText()
            .BindToText(_txt)
            .AddTo(this);
        _count++;
    }
}
