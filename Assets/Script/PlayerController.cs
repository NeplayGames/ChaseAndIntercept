using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action<Transform> OnClick;
    float time = 0.5f;
    float tempTime = 0f;
    void Start()
    {
        TransformPointVector2 = new Vector3(transform.position.x, transform.position.z);
    }

    // Update is called once per frame
   public void OnClickByUser()
    {
        if (OnClick != null)
        {
            OnClick(this.transform);
        }
    }
    private Vector3 TransformPoint,currentPoint;
    public Vector2 TransformPointVector2;
    bool move = false;

    /// <summary>
    /// WHen the move is favarable this is called to transfer to a given position.
    /// </summary>
    /// <param name="pos"></param>
    public void Move(Transform pos)
    {
        move = true;
        TransformPoint = new Vector3(pos.position.x, transform.position.y, pos.position.z);
        TransformPointVector2 = new Vector3(pos.position.x, pos.position.z);
        currentPoint = transform.position;
    }

    private void Update()
    {
        if (move)
        {
            TransferPlayer();
        }
    }
    /// <summary>
    /// Transfer the player to the selected Grid position.
    /// </summary>
    private void TransferPlayer()
    {
        if (tempTime < time)
        {
            transform.position = Vector3.Lerp(currentPoint, TransformPoint, tempTime / time);
            tempTime += Time.deltaTime;
        }
        else
        {
            move = false;
            tempTime = 0;
            transform.position = TransformPoint;
        }
    }
}
