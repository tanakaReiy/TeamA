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

        public InRangeData OnEnterInteractRange(IInteractCallBackReceivable caller)
        {
            return _canInteract ? new InRangeData { _inRangeText = "“®‚©‚·" } : null;
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
