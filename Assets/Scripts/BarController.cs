using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class BarController : MonoBehaviour
{
    [SerializeField] RectTransform fillTransform;
    [SerializeField] TextMeshProUGUI numText;
    [SerializeField] bool isSpeedBar = false;
    private Vector3 originalScale;

    private float leftNum = 1f;
    public float LeftNum { get { return leftNum; } set { leftNum = value; } }
    private float rightNum = 1f;
    public float RightNum { get { return rightNum; } set { rightNum = value; } }

    private void Awake()
    {
        originalScale = fillTransform.localScale;
    }
    public void LazyUpdate()
    {
        if (isSpeedBar) rightNum = 100f;
        else
        {
            numText.text = string.Format("{0} / {1}", leftNum, rightNum);
        }
        originalScale.x = leftNum / rightNum;
        fillTransform.localScale = originalScale;
    }
}
