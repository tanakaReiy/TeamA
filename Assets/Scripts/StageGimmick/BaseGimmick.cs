using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGimmick : MonoBehaviour
{
    [SerializeField] private List<StageGimmickBase> _stageGimmickList = new List<StageGimmickBase>();

    // Start is called before the first frame update
    void Start()
    {
        Test();
    }

    void Test()
    {
        //flagListの数までカウントが増えるまで待つ

        //処理をあける
    }
}
