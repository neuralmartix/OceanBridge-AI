using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoGrabber2_length : MonoBehaviour
{
    infoTransfer infos;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        infos = FindObjectOfType<infoTransfer>();
    }

    // Update is called once per frame
    void Update()
    {
        target.position = new Vector3(transform.position.x, transform.position.y, 0f-infos.a);
    }
}
