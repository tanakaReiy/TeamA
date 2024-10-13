using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Alchemy.Inspector;
public class DetectInteract : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _radius;
    [SerializeField] private GameObject _interactCallBackReceivableObject;

    [SerializeField] private bool _showRange = false;

    public IObservable<InRangeData> OnInInteractableRange => _onInRange;

    private ReactiveProperty<InRangeData> _onInRange = new (null);
    public bool CanInteract {
        get => _canInteract;
        set{
            _canInteract = value;
            if (!value)
            {
                _currentInteractable = null;            
            }
        }
    }

    private const float DETECTCALCINTERVAL = 0.1f;

    private bool _canInteract = false;
    private IInteractable _currentInteractable = null;
    private IInteractCallBackReceivable _callBackReceivable;

    private void Start()
    {
        if (!_interactCallBackReceivableObject.TryGetComponent(out _callBackReceivable))
        {
            throw new System.Exception($"{nameof(_interactCallBackReceivableObject)}ÇÕ{nameof(IInteractCallBackReceivable)}Çé¿ëïÇµÇƒÇ¢ÇÈïKóvÇ™Ç†ÇËÇ‹Ç∑");
        }
        StartCoroutine(Detect());
    }

    public void Interact()
    {
        _currentInteractable?.OnInteract(_callBackReceivable);
    }

    //àÍíËä‘äuÇ≈InteractableÇåüímÇ∑ÇÈ
    private IEnumerator Detect()
    {
        
        while (true)
        {
            if (!CanInteract) { _currentInteractable = null; goto There; }
            var hits = Physics.SphereCastAll(new Ray { origin = transform.position + _offset, direction = transform.forward },_radius,0.1f);

            //interactableÇî≠å©Ç≈Ç´ÇΩÇ©
            bool isFound = false;
            
            foreach(var hit in hits)
            {
                if(hit.collider.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    isFound = true;
                    //åªç›Ç∆ìØÇ∂InteractableÇ≈Ç†Ç¡ÇΩÇÁBreak
                    if (interactable != _currentInteractable) { break; }

                    _currentInteractable = interactable;
                    break;
                }
            }
            if (!isFound) {
                
                _currentInteractable = null;
            }

        There:
            _onInRange.Value = _currentInteractable?.OnEnterInteractRange(_callBackReceivable);
            yield return new WaitForSeconds(DETECTCALCINTERVAL);
        }
    }


    private void OnDrawGizmosSelected()
    {

        if(!_showRange) { return; }

        Gizmos.DrawSphere(transform.position + _offset, _radius);

    }

}

public class InRangeData
{
    public string _inRangeText;
}

