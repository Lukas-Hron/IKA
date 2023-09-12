using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAll : MonoBehaviour
{
    public static Action Respawn;

    public void ActivateRespawn()
    {
        Respawn?.Invoke();
    }
}
