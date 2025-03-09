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
public class MouseManager : SingletonMono<MouseManager>
{
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;

    [Header("Cursor Textures")]
    public Texture2D point;
    public Texture2D doorway;
    public Texture2D attack;
    public Texture2D target;
    public Texture2D arrow;

    [Header("Cursor Properties")]
    public Vector2 cursorOffset = new Vector2(16f, 16f);
    public CursorMode cursorMode = CursorMode.Auto;
    public CursorLockMode cursorLockMode = CursorLockMode.None;
    
    private Ray ray;
    private RaycastHit hitInfo;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
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
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, cursorOffset, cursorMode);
                    break;

                case "Enemy":
                    Cursor.SetCursor(attack, cursorOffset, cursorMode);
                    break;
            }
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

            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }

            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
        }
    }
}
