using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Alchemy.Inspector;
using Ability;
public class Capturable : MonoBehaviour,IDetectable
{
    [field: SerializeField, SerializeReference]
    public IPlayerAbility CapturableAbility { get; private set; }

    [field: SerializeField]
    public GameObject StaffHeadPrefab { get; private set; }

    public bool IsEnableDetect => true;



    private void Awake()
    {
        if(!TryGetComponent<Collider>(out var c))
        {
            Debug.LogError("Colider‚ª‘¶İ‚µ‚È‚¢‚½‚ß‚±‚ÌCapturable‚ÍŒŸo‚³‚ê‚Ü‚¹‚ñ");
        }
    }


}


