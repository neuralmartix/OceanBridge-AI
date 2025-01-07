using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class PlayerScript : MonoBehaviour
{
    public float speed;
    public float angle;

    public Text gamePlayText;
    public Text timerText;
    public Text lastTimerText;
    public Text lastSpeedAngleText;
    public Text distanceText;

    private int gamePlay = 0;
    private float timer = 0;    
    private float lastTimer = 0;
    private float lastSpeed = 0;
    private float lastAngle = 0;

    private bool startNext = true;
    


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(angle, 0, speed));
    }

    // Update is called once per frame
    void Update()
    {
    	// float v = Input.GetAxis("Vertical");
     //    float h = Input.GetAxis("Horizontal");
     // 	GetComponent<Rigidbody>().AddForce(new Vector3(h, 0, v));
        if (startNext) {
            timer += Time.deltaTime;        
            float seconds = timer % 60;        
            timerText.text = "time: " + seconds;    
            distanceText.text = "distance: " + Math.Abs(this.transform.position.z);    
        }        
    }

    void OnCollisionEnter(Collision collision)
    {               
        if (startNext) {
            if (collision.gameObject.tag == "WinCube") {
                // Debug.Log("Collision from the player! Win!");    
                startNext = false;
                StartCoroutine(resetNewRun());
            }
            if (collision.gameObject.tag == "LoseCube") {
                // Debug.Log("Collision from the player! Lose!");    
                startNext = false;
                Debug.Log("Z Value: " + this.transform.position.z);

                timer += 1000 - Math.Abs(this.transform.position.z) * 5;
                StartCoroutine(resetNewRun());
            }
        }        
    }

    IEnumerator resetNewRun() {        
        lastTimer = timer;            
        lastSpeed = speed;
        lastAngle = angle;
        lastTimerText.text = "last time: " + lastTimer;
        lastSpeedAngleText.text = "last speed/angle: " + lastSpeed + "/" + lastAngle;

        UnityWebRequest uwr = UnityWebRequest.Get(
            "https://GA-Server.sunyu912.repl.co/write/result/" + lastSpeed + "-" + lastAngle + "/" + lastTimer);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

        while(true) {        
            UnityWebRequest uwr2 = UnityWebRequest.Get(
                "https://GA-Server.sunyu912.repl.co/fetch/wait");
            yield return uwr2.SendWebRequest();

            if (uwr2.isNetworkError)
            {
                Debug.Log("Error While Sending: " + uwr2.error);
            }
            else
            {                
                Debug.Log("Received: " + uwr2.downloadHandler.text);
                string res = uwr2.downloadHandler.text;
                if (res != "NA") {
                    var words = res.Split("-"[0]);
                    speed = float.Parse(words[0]);
                    angle = float.Parse(words[1]);                    
                    break;
                }
            }
            new WaitForSeconds(2);
        }

        this.transform.position = new Vector3(0.07f, 0.5f, 0f);
        gamePlay = gamePlay + 1;
        timer = 0;
        gamePlayText.text = "iteration: " + gamePlay;
        startNext = true;
        GetComponent<Rigidbody>().AddForce(new Vector3(angle, 0, speed));
    }
    
}
