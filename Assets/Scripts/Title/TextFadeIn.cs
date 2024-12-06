using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;
public class TextFadeIn : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;
    [SerializeField]
    private float _fadeDuration = 3.5f;
    private void Start()
    {
        Color color = _text.color;
        color.a = 0f;
        _text.color = color;
        LMotion.Create(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), 3.5f)
            .Bind(c => _text.color = c);
    }
}
