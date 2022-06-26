using System;
using System.Collections.Generic;
using UnityEngine;

public class DefenceController : MonoBehaviour
{
    private  Transform holders;   
    private Material defaultMaterial;
    private bool clicked;
    private Vector3 initialPos;
    private Quaternion rotation;
    public Vector3 worldPosition;
    private bool rotationDone = false;
    new Vector3 localScal;
    private void Start()
    {
        initialPos = transform.position;
        rotation = transform.rotation;
        localScal = transform.localScale;
    }
    private void Update()
    {
        if(clicked)
        { 
            if( Input.GetMouseButton(0))           
                TransferToPoint();
            
            if (Input.GetMouseButtonDown(1))          
                RotateOnRightClick();
            
            if (Input.GetMouseButtonUp(0))
            {
                clicked = false;
                List<Transform> holdersInfo = GameController.instance.holders;


                if (holders!=null && !holdersInfo.Contains(holders) )         
                    SnapToPoint();            
                else
                {
                    transform.position = initialPos;
                    transform.rotation = rotation;
                    rotationDone = false;
                }
            }           
        }
    }

   
    

    private void CanBeSnapped()
    {
        RaycastHit[] hit = Physics.RaycastAll(holders.position + (rotationDone ? new Vector3(2f, 2f, 0) : new Vector3(0, 2f, 2f)), -transform.up);
        GetCollidingPlaceHolderInfo(hit);
        RaycastHit[] hit2 = Physics.RaycastAll(holders.position + (rotationDone ? new Vector3(-2f, 2f, 0) : new Vector3(0, 2f, -2f)), -transform.up);
        GetCollidingPlaceHolderInfo(hit2);
        RaycastHit[] hit3 = Physics.RaycastAll(holders.position + new Vector3(0, 2f, 0), -transform.up);
        GetIntersectingPlaceHolderInfo(hit3);
    }

    private void GetIntersectingPlaceHolderInfo(RaycastHit[] hit3)
    {
        if (hit3.Length > 0)
        {
            for (int i = 0; i < hit3.Length; i++)
            {
                Transform hit = hit3[i].transform;
                if (hit.CompareTag("PositionHolder"))
                {
                    if (Vector3.Distance(hit.position, transform.position) < 1.6f)
                        GameController.instance.holders.Add(hit.transform);
                }
            }
        }
    }

    private void GetCollidingPlaceHolderInfo(RaycastHit[] hit2)
    {
        if (hit2.Length > 0)
        {
            for (int i = 0; i < hit2.Length; i++)
            {
               Transform hit = hit2[i].transform;
                if (hit.CompareTag("PositionHolder") && hit.rotation == transform.rotation)
                {
                    if (Vector3.Distance(hit.position, transform.position) < 1.6f)
                        GameController.instance.holders.Add(hit.transform);
                }
            }
        }
    }

    /// <summary>
    /// Rotate the object on right click.
    /// </summary>
    private void RotateOnRightClick()
    {
        transform.rotation = Quaternion.Euler(new Vector2(0, transform.eulerAngles.y == 180 ? 90 : 180));
        if (holders != null)
        {
            holders.GetComponent<MeshRenderer>().material = defaultMaterial;
            holders.localScale = localScal;
        }
           
        holders = null;
        rotationDone = !rotationDone;
    }
    /// <summary>
    /// Get the mouse Position infomation.
    /// The mouse position is converted to world cordinates.
    /// Then the object is transfer to the point.
    /// </summary>
    private void TransferToPoint()
    {
        Plane plane = new Plane(Vector3.up, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out float distance))
            worldPosition = ray.GetPoint(distance);
        transform.position = new Vector3(worldPosition.x, transform.position.y, worldPosition.z);
    }
  
    private void SnapToPoint()
    {
        transform.position = holders.position + Vector3.up/2;
        transform.rotation = holders.rotation;
        holders.GetComponent<MeshRenderer>().material = defaultMaterial;
        holders.localScale = localScal;
        GameController.instance.holders.Add(holders);
        GameController.instance.actualHolder.Add(holders);
        GameController.instance.ChangeTurn();
        CanBeSnapped();
    }
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (snap)
    //    {
    //        if (collision.collider.CompareTag("PositionHolder"))
    //        {
    //            if (collision.transform != this.transform)
    //            {
    //                GameController.instance.holders.Add(collision.transform);
    //                add++;
    //                if (add == 2)
    //                    snap = false;
    //            }
    //        }
    //    }
    //}
    private void OnCollisionStay(Collision collision)
    {
        if (clicked)
        {
            if (!collision.collider.CompareTag("PositionHolder")) return;
            if (collision.transform.rotation != transform.rotation) return;
            CheckForClosenessAndAvailability(collision);
        }
    }
    /// <summary>
    /// Identify whether the collide object is near enough to be hilighted.
    /// </summary>
    /// <param name="collision"></param>
    private void CheckForClosenessAndAvailability(Collision collision)
    {
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 colliderPos = new Vector2(collision.transform.position.x, collision.transform.position.z);
        if (Vector2.Distance(playerPos, colliderPos) < GameController.instance.requiredDistanceToHolder)
        {
            GetInfoOnHolder(collision);
        }
    }

    private void GetInfoOnHolder(Collision collision)
    {
        if (holders != null)
            holders.GetComponent<MeshRenderer>().material = defaultMaterial;
        holders = collision.transform;
        MeshRenderer ren = holders.GetComponent<MeshRenderer>();
        defaultMaterial = ren.material;
        ren.material = GameController.instance.changeMaterialDefense;
        holders.localScale =new Vector3( localScal.x * 1.5f,localScal.y,localScal.z * 1.5f);
    }

    private void OnCollisionExit(Collision collision)
    {   
        if(holders  == collision.transform)
        {
            collision.transform.GetComponent<MeshRenderer>().material = defaultMaterial;
            holders = null;
        }
    }

    public void OnClickByUser()
    {
        clicked = true; 
    }
   

  
}

