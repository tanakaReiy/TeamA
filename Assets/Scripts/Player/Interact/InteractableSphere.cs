using Cysharp.Threading.Tasks;
using LitMotion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractableObject
{

    public class InteractableSphere : MonoBehaviour, IInteractable
    {
        private bool _canInteract = true;

        public bool CanInteract()
        {
            return _canInteract;
        }

        public string GetInteractionMessage()
        {
            return "“®‚©‚·";
        }

        public void OnInteract(IInteractCallBackReceivable caller)
        {
            if (_canInteract)
            {
                UniTask.Create(async () =>
                {
                    await LMotion.Create(0, 0, 1)
                     .Bind(_ => transform.Translate(0, 10 * Time.deltaTime, 0))
                     .ToUniTask();

                    await LMotion.Create(0, 0, 1)
                     .Bind(_ => transform.Translate(0, -10 * Time.deltaTime, 0))
                     .ToUniTask();

                    _canInteract = true;

                }).Forget();
                
            }

        }

       
    }
}
