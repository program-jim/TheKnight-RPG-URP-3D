using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//-------
// Old way of Event to translate Vector3.
//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3>{}
// -------

/// <summary>
/// Manage the input controll of PC's mouse. 
/// </summary>
public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance;
    public event Action<Vector3> OnMouseClicked;
    public Texture2D point, doorway, attack, target, arrow;
    
    private Ray ray;
    private RaycastHit hitInfo;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
        if (Camera.main != null)
        {
            //The Camera.main is not null.
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

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
