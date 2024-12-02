using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
/// <summary>
/// ���ׂĂ�UI���Ǘ����܂�
/// �V���O���g���ADontDestoryOnLoad�ł���c�葱���܂�
/// ���ׂĂ�UI��UIManager���ێ�����e�L�����o�X�ȉ��ɔz�u����܂�
/// UI_Merge_Canvas���g�����ƂŔC�ӂ̈ʒu����UIManager�Ɍ����ł��܂�
/// 
/// </summary>
public class UIManager : SingletonMonoBehavior<UIManager>
{
    
    private List<CanvasData> _canvases = new();

    private readonly string CanvasPrefabAddressableAddress = "CanvasPrefab";
    private GameObject _canvasPrefab;

    public bool IsReady { get; private set; } = false;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        UniTask.Create(async () =>
        {
            _canvasPrefab = await Addressables.LoadAssetAsync<GameObject>(CanvasPrefabAddressableAddress);
        }).ContinueWith(() =>
        {

            if (_canvasPrefab == null)
            {
                Debug.LogError("CanvasPrefab�����ݒ�");
                return;
            }
            if (!_canvasPrefab.TryGetComponent(out CanvasData canvasData))
            {
                Debug.LogError("CanvasPrefab��Canvas���܂݂܂���"); return;
            }
            IsReady = true;

        }).Forget();
        
 
    }
    
    public void RegisterGroup(int sortOrder, IEnumerable<UIGroup> groups)
    {
        var canvasData = GetCanvasData(sortOrder);
        foreach(var g in groups)
        {
            canvasData.AddGroup(g);
        }
    }

    public CanvasData GetCanvasData(int sortOrder)
    {
        int index = _canvases.FindIndex(c => c.Canvas.sortingOrder == sortOrder);
        if (index != -1)
        {
            return _canvases[index];
        }
        else
        {
            return CreateNewCanvas(sortOrder);
        }
    }

    private CanvasData CreateNewCanvas(int sortOrder)
    {
        GameObject canvasObject = Instantiate(_canvasPrefab);
        canvasObject.name = $"Canvas_{sortOrder}";
        CanvasData canvasData = canvasObject.GetComponent<CanvasData>();
        int index = _canvases.FindIndex(c => c.Canvas.sortingOrder > sortOrder);
        if(index != -1)
        {
            _canvases.Insert(index, canvasData);
            canvasObject.transform.SetParent(transform);
            canvasObject.transform.SetSiblingIndex(index);
        }
        else
        {
            _canvases.Add(canvasData);
            canvasObject.transform.SetParent(transform);
            canvasObject.transform.SetAsLastSibling();
        }
        return canvasData;
    }



}
