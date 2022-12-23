using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            //GameManager.Instance.LevelWin();
            GameManager.Instance.player.followCamera.LevelEndCamera();
        }
    }
}
