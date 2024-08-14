using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private BedManager manager;
    [SerializeField] private NetWorkManager netManager;
    [SerializeField] private SleeperManager sleeperManager;
    private void Awake()
    {
        player = new Player();
        player.Enable();
        player.Play.Touch1.performed += Touch_performed;
        player.Play.Touch2.performed += Touch_performed;
    }
    private void OnDestroy()
    {
        player.Disable();
        player.Dispose();
    }
    private void Start()
    {
        if (maxOrthographicsize <= 3)
        {
            if (Camera.main.aspect > 1)
            {
                var h = manager.MapRect.height * .5f;
                var w = manager.MapRect.height * .5f;
                maxOrthographicsize = (h < w ? h : w) / Camera.main.aspect;
            }
            else
            {
                var h = manager.MapRect.height * .5f;
                var w = manager.MapRect.height * .5f;
                maxOrthographicsize = h < w ? h : w;
            }
        }
    }
    private void moveScreen(Vector2 move)
    {
        var cameraTransform = Camera.main.transform;
        Vector2 scale = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize) * 2;
        var target = Vector3.MoveTowards(cameraTransform.position, cameraTransform.position - (Vector3)(move*scale/new Vector2(Screen.width,Screen.height)), Vector2.Distance(Vector2.zero,move));
        target = DetechRect(target);
        cameraTransform.position = target;
    }
    public Vector3 DetechRect(Vector3 target)
    {
        Vector2 scale = new Vector2(Camera.main.aspect * Camera.main.orthographicSize, Camera.main.orthographicSize) * 2;
        var rect = new Rect(0, 0, scale.x, scale.y);
        rect.center = target;
        var mrect = manager.MapRect;
        if (rect.xMin < mrect.xMin)
        {
            target.x += mrect.xMin - rect.xMin;
        }
        else if (rect.xMax > mrect.xMax)
        {
            target.x -= rect.xMax - mrect.xMax;
        }
        if (rect.yMin < mrect.yMin)
        {
            target.y += mrect.yMin - rect.yMin;
        }
        else if (rect.yMax > mrect.yMax)
        {
            target.y -= rect.yMax - mrect.yMax;
        }
        target.z = Camera.main.transform.position.z;
        return target;
    }
    bool doubleFinger = false;
    TouchState oneTouch = new TouchState();
    float maxOrthographicsize = 3;
    private void Touch_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        var TouchState = obj.ReadValue<TouchState>();
        if(obj.control.path.Contains("touch1"))
        {
            doubleFinger = true;
        }
        switch (TouchState.phase)
        {
            case UnityEngine.InputSystem.TouchPhase.Moved:
                sleeperManager.AllwaysLookMeOFF();
                if (!doubleFinger)
                {
                    if (!UITouch(TouchState.position) && !UITouch(TouchState.startPosition))
                        moveScreen(TouchState.delta);
                }
                else
                {
                    if (obj.control.path.Contains("touch0"))
                    {
                        oneTouch = TouchState;
                    }
                    else
                    {
                        var startDistance = Vector2.Distance(oneTouch.position - oneTouch.delta, TouchState.position - TouchState.delta);
                        var endDistance = Vector2.Distance(oneTouch.position, TouchState.position);
                        var distance = Vector2.Distance(oneTouch.delta, TouchState.delta) * .01f;
                        var dirmove = TouchState.delta + oneTouch.delta;
                        var targetDistance = Camera.main.orthographicSize;
                        if (startDistance > endDistance)
                        {
                            targetDistance += distance;
                        }
                        else
                        {
                            targetDistance -= distance;
                        }
                        if (targetDistance < 3)
                            targetDistance = 3;
                        else if (targetDistance > maxOrthographicsize)
                        {
                            targetDistance = maxOrthographicsize;
                        }
                        Camera.main.orthographicSize = targetDistance;
                        moveScreen(dirmove * .5f);
                    }
                }
                break;
            case UnityEngine.InputSystem.TouchPhase.Ended:
                if (TouchState.isTap && TouchState.tapCount > 0)
                {
                    var isUITOUCH = UITouch(TouchState.position);
                    if (!isUITOUCH)
                    {
                        Vector2 pos = Camera.main.ScreenToWorldPoint(TouchState.position);
                        sleeperManager.MoveTo(pos);
                    }
                }
                if(obj.control.path.Contains("touch1"))
                {
                    doubleFinger = false;
                }
                break;
        }
    }
    private PointerEventData pointerData;
    private bool UITouch(Vector2 pos)
    {
        var casters = FindObjectsOfType<GraphicRaycaster>();
        foreach(var caster in casters)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            if (pointerData == null)
                pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = pos;
            caster.Raycast(pointerData, results);
            if (results.Count > 0)
                return true;
        }
        return false;
    }

}
