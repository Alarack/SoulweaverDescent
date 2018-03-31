using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollDragger : MonoBehaviour, IDragHandler, IBeginDragHandler {

    public Camera uiCam;
    private ScrollRect scrollRect;

    private Vector2 mousePos;
    private Vector2 offset;

    void Start () {
        scrollRect = GetComponent<ScrollRect>();
	}

	void Update () {
		
	}

    public void OnDrag(PointerEventData eventData) {
        //scrollRect.verticalNormalizedPosition -= eventData.delta.y / Screen.height;
        //scrollRect.horizontalNormalizedPosition -= eventData.delta.x / Screen.width;
        //Debug.Log("Dragging");
        //scrollRect.OnDrag(eventData);



        scrollRect.content.position = (Vector2)uiCam.ScreenToWorldPoint(Input.mousePosition) + offset;

    }

    public void OnBeginDrag(PointerEventData eventData) {
        mousePos = uiCam.ScreenToWorldPoint(Input.mousePosition);
        offset = (Vector2)scrollRect.content.position - mousePos;
    }
}
