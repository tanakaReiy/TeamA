using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy.Serialization;
using UnityEngine.UI;

[AlchemySerialize]
public partial class HelthUIVIew : MonoBehaviour
{
    [SerializeField,AlchemySerializeField] private Dictionary<WandManager.CaptureAbility, Sprite> _healthSprite;

    [SerializeField] private Image _healthImage;

    public void SetView(WandManager.CaptureAbility ability)
    {
        if(_healthSprite.TryGetValue(ability, out var sprite))
        {
            _healthImage.sprite = sprite;
            var c = _healthImage.color;

            c.a = 1;
            _healthImage.color = c;
        }
        else
        {
            _healthImage.sprite = null;
            var c = _healthImage.color;
            c.a = 0;
            _healthImage.color = c;
        }
    }
}

