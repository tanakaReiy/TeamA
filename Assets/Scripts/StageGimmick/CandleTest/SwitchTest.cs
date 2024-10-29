using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTest : MonoBehaviour
{
    private bool hasProcessed = false;
    private float rayDistance = 1f;
    private void Update()
    {
        if (hasProcessed || !gameObject) return;

        if (IsPlayerOnSwitch())
        {
            OpenDoor();
            hasProcessed = true;
        }
    }
    private bool IsPlayerOnSwitch()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, transform.localScale.y / 2, 0), Vector3.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            return true;
        }
        return false;
    }
    private void OpenDoor()
    {
        Debug.Log("ドアオープン!");
    }
}
