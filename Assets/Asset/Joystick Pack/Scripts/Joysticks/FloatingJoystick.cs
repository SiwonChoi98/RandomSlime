using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(true);
        background.gameObject.transform.localPosition = initPos.gameObject.transform.localPosition; ///조이스틱 위치 초기화
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(true);
        background.gameObject.transform.localPosition = initPos.gameObject.transform.localPosition; ///조이스틱 위치 초기화
        
        base.OnPointerUp(eventData);
    }
}