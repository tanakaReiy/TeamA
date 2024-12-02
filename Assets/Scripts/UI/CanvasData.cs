using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasData : MonoBehaviour
{
    [field:SerializeField]public Canvas Canvas { get; private set; }
    [field: SerializeField] public CanvasScaler CanvasScaler { get; private set; }
    [field: SerializeField] public GraphicRaycaster GraphicRaycaster { get; private set; }

    public IReadOnlyList<UIGroup> Groups => _groups;

    private List<UIGroup> _groups = new();

    public void AddGroup(UIGroup group)
    {
        int index;
        for (index = 0; index < _groups.Count; index++)
        {
            if(_groups[index].MergePriority > group.MergePriority)
            {
                break;
            }
        }
        _groups.Insert(index, group);
        group.transform.SetParent(transform);
        group.transform.SetSiblingIndex(index);
        group.Initialize();
    }

    

}
