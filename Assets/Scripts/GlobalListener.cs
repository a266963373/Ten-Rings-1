using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlobalListener : MonoBehaviour
{
    public AudioClip defaultClickSound;
    public AudioSource audioSource;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 鼠标左键或触屏点击
        {
            if (EventSystem.current.IsPointerOverGameObject()) // 判断是否点到 UI
            {
                GameObject clicked = GetClickedUI();

                if (clicked != null && (clicked.GetComponent<Button>() != null 
                    || clicked.GetComponentInParent<Button>() != null))
                {
                    audioSource.PlayOneShot(defaultClickSound);
                }
            }
        }
    }

    GameObject GetClickedUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        if (raycastResults.Count > 0)
            return raycastResults[0].gameObject;

        return null;
    }
}
