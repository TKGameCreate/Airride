using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 cameraPosition = new Vector3(0, 2, -3);

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position - cameraPosition;
    }
}
