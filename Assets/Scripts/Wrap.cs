using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Wrap : MonoBehaviour
{
    private void Update()
    {
        Vector3 vViewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        Vector3 moveAdjustment = Vector3.zero; ;
        if (vViewportPosition.x < 0)
        {
            moveAdjustment.x += 1;
        }
        else if (vViewportPosition.x > 1)
        {
            moveAdjustment.x -= 1;
        }
        else if (vViewportPosition.y < 0)
        {
            moveAdjustment.y += 1;
        }
        else if ((vViewportPosition.y > 1))
        {
            moveAdjustment.y -= 1;
        }

        transform.position = Camera.main.ViewportToWorldPoint(vViewportPosition + moveAdjustment);
    }
}
