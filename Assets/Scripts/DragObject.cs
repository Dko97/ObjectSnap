using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script as the name suggests is used for dragging a gameobject using the mouse position of the 
 * user. 
 */
public class DragObject : MonoBehaviour
{
    private Vector3 m_Offset;
    private float m_ZCoord;
    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void OnMouseDown()
    {
        m_ZCoord = mainCam.WorldToScreenPoint(transform.position).z;

        m_Offset = transform.position - GetMousePositionAsWorldPoint();
    }

    Vector3 GetMousePositionAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = m_ZCoord;
        return mainCam.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMousePositionAsWorldPoint()+m_Offset;
    }

}
