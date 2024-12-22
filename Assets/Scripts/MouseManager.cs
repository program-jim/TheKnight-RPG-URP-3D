using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3>{}

/// <summary>
/// Manage the input controll of PC's mouse. 
/// </summary>
public class MouseManager : MonoBehaviour
{
    public EventVector3 OnMouseClicked;
    
    private Ray ray;
    private RaycastHit hitInfo;

    private void Awake()
    {
        Debug.Log("Mouse Manager is on.");
    }

    private void Update()
    {
        SetCursorTexture();
        MouseControl();
    }

    /// <summary>
    /// A function which set the texture of PC's cursor.
    /// </summary>
    void SetCursorTexture()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            //TODO: Change cursor's texture
            
        }
    }

    /// <summary>
    /// Manage the input controll of PC's mouse.
    /// </summary>
    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
        }
    }
}
