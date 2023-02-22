using HomaGames.Internal.BaseTemplate;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngameUI : MonoBehaviour
{
    //public void OnPointerDown(BaseEventData eventData)
    //{
    //    PointerEventData pointerData = eventData as PointerEventData;
    //    Vector2 normalizedPosition = pointerData.position;
    //    normalizedPosition.Scale(new Vector3(1 / Screen.width, 1 / Screen.height));
    //    GameManagerBase.Instance.Context.SetValue("X_Axis", normalizedPosition.x);
    //}

    //public void OnPointerDrag(BaseEventData eventData)
    //{
    //    PointerEventData pointerData = eventData as PointerEventData;
    //    float normalizedX = pointerData.position.x;
    //    normalizedX *= (1.0f / Screen.width);
    //    normalizedX -= 0.5f;
    //    normalizedX *= 2.0f;
    //    GameManagerBase.Instance.Context.SetValue("X_Axis", normalizedX);
    //}
}
