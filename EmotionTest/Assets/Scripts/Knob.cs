using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class Knob : MonoBehaviour {

    public Text x;
    public Text y;
    public Vector2 center;

    float rotSpeed = 20;
    float start_x;
    float start_y;
    float record_time = 0f;

    private float start_angle;
    private float current_angle;
    bool first_time = true;
    bool mousedown = false;

    private float last_record=0;
    private float value_level=0;

    private string filePath = "Recording.txt";

	private bool left_lock=false;
	private bool right_lock = false;

    private void Start()
    {
        filePath = Application.persistentDataPath + "/Recording.txt";
        System.IO.File.WriteAllText(filePath, "time,value\n");
    }



    private void OnMouseDown()
    {
        if(first_time)
        {
            first_time = false;
            InvokeRepeating("Record", 0f, 0.1f);
        }

        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        start_angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        current_angle = transform.eulerAngles.z;
        Debug.Log(start_angle);
        mousedown = true;
    }

    private void OnMouseExit()
    {
        
    }

    private void OnMouseUp()
    {
        mousedown = false;
    }

    /*
    private void OnMouseDown()
    {
        start_x = Input.mousePosition.x;
        start_y = Input.mousePosition.y;
        string xs = "X: ";
        xs += start_x.ToString();
        x.text = xs;
        string ys = "Y: ";
        ys += start_y.ToString();
        y.text = ys;

        float angel = 360f * Mathf.Atan((start_x - touch_x) / (start_y - touch_y)) / Mathf.PI;
        if (float.IsNaN(angel))
            angel = 180;
        //Debug.Log(start_x);
        //Debug.Log(start_y);
    }
  

    void OnMouseDrag()
    {
        float touch_x = Input.mousePosition.x;
        float touch_y = Input.mousePosition.y;
        Vector2 touch = new Vector2(touch_x,touch_y);
        string xs = "X: ";
        xs += touch_x.ToString();
        x.text = xs;
        string ys = "Y: ";
        ys += touch_y.ToString();
        y.text = ys;
        float angel = 360f * Mathf.Atan((start_x - touch_x) / (start_y - touch_y)) / Mathf.PI;
        if (float.IsNaN(angel))
            angel = 180;
        Debug.Log(angel);
        //Debug.Log(Mathf.Atan((start_x - touch_x) / (start_y - touch_y)));
        //Debug.Log(rotX);
        //Debug.Log(rotY);
        //Debug.Log(transform.eulerAngles.z);
        transform.eulerAngles = new Vector3(0, 0, -angel + 180);
    }
    */
    void Record()
    {
		Debug.Log (transform.eulerAngles.z);
        if (last_record > 280 && transform.eulerAngles.z < 80)
            value_level--;
        if (last_record < 80 && transform.eulerAngles.z > 280)
            value_level++;
        float new_value = value_level * 360f - transform.eulerAngles.z;



        record_time += 0.1f;
        string newtext = record_time.ToString() + "," + new_value.ToString() + "\n";
        File.AppendAllText(filePath, newtext);
        last_record = transform.eulerAngles.z;
       //Debug.Log(value_level);
    }
    void Update()
    {
        if (mousedown)
        {
            Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            difference.Normalize();
            float rotation_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
          
            rotation_z -= start_angle;
           
            transform.rotation = Quaternion.Euler(0f, 0f, rotation_z+ current_angle);

			if (left_lock) {
				if (transform.eulerAngles.z < 120f) {
					left_lock = false;
				} else {
					
					transform.rotation = Quaternion.Euler(0f, 0f, 120f);
					return;
				}
			}

			if (right_lock) {
				if (transform.eulerAngles.z>240f) {
					right_lock = false;
				} else {
					
					transform.rotation = Quaternion.Euler(0f, 0f, 240f);
					return;
					
				}
			}

			if (transform.eulerAngles.z<180f&&transform.eulerAngles.z>120f) {
				transform.rotation = Quaternion.Euler(0f, 0f, 120f);
				left_lock = true;
			}

			if (transform.eulerAngles.z<240f&&transform.eulerAngles.z>180f) {
				transform.rotation = Quaternion.Euler(0f, 0f, 240f);
				right_lock = true;
			}
            
        }

        
    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            transform.eulerAngles += new Vector3(0, 0, 16);
            Debug.Log(transform.eulerAngles.z);
        }
    }
    */

}
