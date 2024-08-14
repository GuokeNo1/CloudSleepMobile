using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class phoneButton : Selectable, IPointerClickHandler
{
    [SerializeField] private Phone Phone;
    [SerializeField] private int ui;
    public void OnPointerClick(PointerEventData eventData)
    {
        Phone.gameObject.transform.position = transform.position;
        Phone.gameObject.SetActive(true);
        Phone.open(ui);
    }
}
