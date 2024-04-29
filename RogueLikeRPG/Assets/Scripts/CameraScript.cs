using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject room;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out _))
        {
            Camera.main.transform.position = new Vector3(room.transform.position.x - 1, room.transform.position.y - 0.38f, -10);
        }
    }
}
