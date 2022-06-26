using System;
using System.Collections.Generic;
using UnityEngine;

public class EachGridInfo : MonoBehaviour
{
    [HideInInspector]
    public bool Movable = false;
    [HideInInspector]
    public MeshRenderer render;
    void Start()
    {
        PlayerController.OnClick += PlayerController_OnClick;
        render = GetComponent<MeshRenderer>();
    }

    private void OnDestroy()
    {
        PlayerController.OnClick -= PlayerController_OnClick;
    }





    /// <summary>
    /// It is called when the player click events happens.
    /// </summary>
    /// <param name="obj"></param>
    private void PlayerController_OnClick(Transform obj)
    {
        if (!CheckOccupiedCondition()) return;
        Vector2 playerPos = new Vector2(obj.position.x, obj.position.z);
        Vector2 thisPos = new Vector2(transform.position.x, transform.position.z);
        float requiredDistance = GameController.instance.requiredDistance;
        if (Vector2.Distance(playerPos, thisPos) > requiredDistance * 2) return;
        Vector2 oppositePlayer = GameController.instance.oppPlayer;
        if ((playerPos.x == thisPos.x || playerPos.y == thisPos.y))
        {
            if (CheckBlockAge(obj.position)) return;
            if (Vector2.Distance(playerPos, thisPos) < requiredDistance ) { 
                    ChangeColor(obj);
            }
            else if(Vector2.Distance(playerPos, thisPos)> Vector2.Distance(oppositePlayer, playerPos))
            {
                if (Vector2.Distance(thisPos, oppositePlayer) < requiredDistance)
                ChangeColor(obj);
            }
        }
    }


    private bool CheckOccupiedCondition()
    {
        return  !(GameController.instance.oppPlayer == new Vector2(transform.position.x, transform.position.z));        
    }

    private bool CheckBlockAge(Vector3 playerPos)
    {
        List<Transform> block = GameController.instance.actualHolder;
        Vector3 thisPos = transform.position;
        foreach(Transform pos in block)
        {
            Vector3 blockPos = pos.position;
            Vector3 blockRot = pos.eulerAngles;
            float minx,miny,maxx,maxy;

            if (thisPos.x < playerPos.x)
            {
                minx = thisPos.x;
                maxx = playerPos.x;
            }
            else
            {
                maxx = thisPos.x;
                minx = playerPos.x;
            }
            if (thisPos.z < playerPos.z)
            {
                miny = thisPos.z;
                maxy= playerPos.z;
            }
            else
            {
                maxy = thisPos.z;
                miny = playerPos.z;
            }
            bool OnXCondition = (minx < blockPos.x && blockPos.x < maxx && blockRot.y == 90);
            bool OnZCondition = (miny < blockPos.z && blockPos.z < maxy && blockRot.y == 180);
            if ((OnXCondition||OnZCondition ) && Vector2.Distance(new Vector2( blockPos.x,blockPos.z), new Vector2(playerPos.x, playerPos.z)) <1.1f)
                return true;
        }
        return false;
    }

    protected virtual void ChangeColor(Transform obj)
    {
        GameController.instance.ChangedMeshrenderer(this);
        Movable = true;
    }
    /// <summary>
    /// Determine if the object is movabel.
    /// </summary>
    /// <returns></returns>
    public virtual bool IsMovable()
    {
        return Movable;
    }
    /// <summary>
    /// Determine if the object has reach final position
    /// </summary>
    /// <returns></returns>
    public virtual bool FinalGrid()
    {
        return false;
    }
}
