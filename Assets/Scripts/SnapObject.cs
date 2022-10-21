using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is the main script which brings in the functionality on attaching it to any gameobject with a Collider and a 
 * custom DragObject script (which drags respective gameobject according to user mouse inputs). I have given a
 * boolean for CornerSnap because there are some minor improvments needed for it. The CornerSnap bool can be 
 * turned off for the base functionality and can be turned on for the bonus corner snap. 
 */

/* Pre-requisite : This gameobject should have a different layer other than the default layer. I have created a 
 * new layer called player and assigned the layer to this gameobject. It is to avoid confusions while using 
 * Physics.OverlapSphere as it detects this gameobjects collider as well. I have also added a ground layer
 * and have marked the floor plane as this layer to prevent interference with the functionality. The 
 * floor plane is not a necessity and is only added for the look and feel. 
 */

[RequireComponent (typeof(Collider),typeof(DragObject))]
public class SnapObject : MonoBehaviour
{
    Collider targetCollider;
    Collider targetCollider2;
    Collider myCollider;
    Vector3 oldPos;

    public float SnapDistance = 1;
    public bool CornerSnap;
    

    private void Start()
    {
        myCollider = transform.GetComponent<Collider>();
        oldPos = transform.position;
    }
    void Update()
    {
        if (CornerSnap)
        {

            FindTwoNearestColliders();

            if (Input.GetKey(KeyCode.LeftShift) && targetCollider != null && targetCollider2 != null && targetCollider != targetCollider2)
            {
                SnapToCorner();
            }

        }
        else
        {
            FindNearestCollider();
            if (Input.GetKey(KeyCode.LeftShift) && targetCollider != null)
            {
                Snap();
            }
        }
    }


    #region FindingCollider
    void FindNearestCollider()
    {
        // Running only when there's been a change in position
        if (transform.position != oldPos)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 10, 1 << 0);
            float nearest = Mathf.Infinity;


            for (int i = 0; i < nearbyColliders.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, nearbyColliders[i].transform.position);
                if (distance < nearest)
                {
                    nearest = distance;
                    targetCollider = nearbyColliders[i];
                }
            }
            oldPos = transform.position;
        }
    }

    void FindTwoNearestColliders()
    {
        // Running only when there's been a change in position
        if (transform.position != oldPos)
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, 10, 1 << 0);
            float nearest = Mathf.Infinity;
            float secondNearest = Mathf.Infinity + 1;

            for (int i = 0; i < nearbyColliders.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, nearbyColliders[i].transform.position);
                if (distance < nearest)
                {
                    secondNearest = nearest;
                    nearest = distance;

                    targetCollider = nearbyColliders[i];
                    targetCollider2 = targetCollider;

                }
                else if (distance < secondNearest && distance != nearest)
                {
                    secondNearest = distance;
                    targetCollider2 = nearbyColliders[i];
                }

            }
            oldPos = transform.position;
        }
    }

    #endregion

    #region Snap functions
    void Snap()
    {
            Vector3 myClosestPoint = myCollider.ClosestPoint(targetCollider.transform.position);
            Vector3 targetClosestPoint = targetCollider.ClosestPoint(myClosestPoint);
            Vector3 offset = targetClosestPoint - myClosestPoint;
            if (offset.magnitude < SnapDistance)
            {
                transform.position += offset;
            }
    }

    void SnapToCorner()
    {
        /* I have made an assumption here because there could be a case where 2 walls are parallel and might satisfy this 
         * condition. The assumption is that the corner wall's colliders will have atleast some amount of 
         * intersection between their colliders. I've checked the condition based on the assumption. 
         */

        if (targetCollider.bounds.Intersects(targetCollider2.bounds) && targetCollider != targetCollider2)
        {
            Vector3 myClosestPoint1 = myCollider.ClosestPoint(targetCollider.transform.position);
            Vector3 myClosestPoint2 = myCollider.ClosestPoint(targetCollider2.transform.position);

            Vector3 targetClosestPoint1 = targetCollider.ClosestPoint(myClosestPoint1);
            Vector3 targetClosestPoint2 = targetCollider2.ClosestPoint(myClosestPoint2);

            Vector3 offset1 = targetClosestPoint1 - myClosestPoint1;
            Vector3 offset2 = targetClosestPoint2 - myClosestPoint2;

            if (offset1.magnitude < SnapDistance)
            {
                transform.position += offset1;
            }

            if (offset2.magnitude < SnapDistance)
            {
                transform.position += offset2;
            }
        }

    }
    #endregion
}
