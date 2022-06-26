using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGridInfo : EachGridInfo
{
    [SerializeField] private bool firstPlayer = true;
    private bool won = false;
    protected override void ChangeColor(Transform obj)
    {
       if(obj.CompareTag("Player1") && !firstPlayer)
        {
            base.ChangeColor(obj);
        }
        if (obj.CompareTag("Player2") && firstPlayer)
        {
            base.ChangeColor(obj);
        }
    }
    public override bool IsMovable()
    {
        won = true;
        return base.IsMovable();
    }
   
    public override bool FinalGrid()
    {
        return won;
    }
}
