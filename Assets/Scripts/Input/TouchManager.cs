using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerMoveHandler
{
    private Camera _camera;
    private bool _pressed;
    private void Awake()
    {
        _camera = Camera.main;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        _pressed = true;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_pressed)
        {
            var wf = Screen.width / (float)Screen.height * _camera.orthographicSize;
            _camera.transform.position -= (Vector3)((Vector2)_camera.ScreenToViewportPoint(eventData.delta) * new Vector2(wf, _camera.orthographicSize) * 2);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressed = false;
    }
}
