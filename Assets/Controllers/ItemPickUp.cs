using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
    private Canvas canvas;
    private Camera uiCamera;

    private RectTransform rectParent;
    private RectTransform nameTag;

    public Vector3 offset = new Vector3(0, -2.0f, 0f);
    public Transform targetTr;

    Text text;
    // Start is called before the first frame update
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;

        rectParent = canvas.GetComponent<RectTransform>();
        nameTag = this.gameObject.GetComponent<RectTransform>();

        text = nameTag.GetComponentInChildren<Text>();
    }

    private void LateUpdate()
    {
        text.text = targetTr.GetComponent<Souls>().name;
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);
        var localPos = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectParent, screenPos, uiCamera, out localPos);

        nameTag.localPosition = localPos;
    }

}
