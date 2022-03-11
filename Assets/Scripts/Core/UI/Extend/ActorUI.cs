using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorUI : MonoBehaviour
{
    public Transform ModelTransform;

    private bool isRotate;
    private Vector3 startPoint;
    private Vector3 startAngel;
    private float rotateScale = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isRotate)
        {
            isRotate = true;
            startPoint = Input.mousePosition;
            startAngel = ModelTransform.eulerAngles;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isRotate = false;
        }

        if (isRotate)
        {
            var currentPoint = Input.mousePosition;
            var x = startPoint.x - currentPoint.x;

            ModelTransform.eulerAngles = startAngel + new Vector3(0, x * rotateScale, 0);
        }
    }
}
