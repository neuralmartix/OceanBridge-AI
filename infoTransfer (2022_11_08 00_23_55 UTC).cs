using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class infoTransfer : MonoBehaviour
{
    public InputField length;
    public InputField speed;
    public InputField wHeight;
    public InputField wLength;
    public float a;
    public float b;
    public float c;
    public float d;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        if (length != null)
            float.TryParse(length.text, out a);
        if (speed != null)
            float.TryParse(speed.text, out b);
        if (wHeight != null)
            float.TryParse(wHeight.text, out c);
        if (wLength != null)
            float.TryParse(wLength.text, out d);
    }
}
