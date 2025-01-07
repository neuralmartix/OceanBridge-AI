using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoGrabber : MonoBehaviour
{
    infoTransfer infos;
    public Transform target;
    void Start()
    {
        infos = FindObjectOfType<infoTransfer>();
    }

        // Update is called once per frame
    void Update()
    {
        target.rotation = Quaternion.Euler(0, 0, infos.c);
    }
}
