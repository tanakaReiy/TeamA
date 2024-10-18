using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
namespace UIControls
{
    public class InteractableObjectText : MonoBehaviour,IInitializable
    {
        [SerializeField] private Text _text;
        [SerializeField] private InteractDetector _detector;

        public void Initialize()
        {
          
            _detector.CurrentInteractable
                .Subscribe(data =>
                {
                    if (data == null) { gameObject.SetActive(false); }
                    else
                    {
                        gameObject.SetActive(true);
                        _text.text = data.GetInteractionMessage();
                    }
                }).AddTo(this);
        }
    }
}
