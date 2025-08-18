using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public PlayerManager firstObj;
    Quaternion quat = Quaternion.identity;
    bool is_initial = false;
    public bool is_run = true;
    public void SetPos(Vector3 _pos)
    {
        transform.position = _pos;
    }
    public void SetQuat(Quaternion _quat)
    {
        if (!is_run) return;

        //Debug.Log("id-" + id + "_rotation-" + _quat);
        if (!is_initial)
        {
            quat = _quat;
            is_initial = true;
        }
        else
        {
            Vector3 deltaQuat = _quat.eulerAngles - quat.eulerAngles;
            transform.eulerAngles += deltaQuat;
            quat = _quat;
            Debug.Log("id-" + id + "eulerAngles-" + deltaQuat);
        }
    }
}
