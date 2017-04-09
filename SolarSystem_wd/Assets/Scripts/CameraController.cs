using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController instance;

    public float MoveSpeed;
    public float RotateSpeed;
    public float ScrollWheelSpeed;

    private Vector3 newPos;
    Quaternion rotation;

    float MoveX;
    float MoveZ;

    float MoveScroll;

    float RotateX;
    float RotateY;


    void Start()
    {
        instance = this;
        newPos = transform.position;
    }

    void Update () 
    {
        //wasd
         MoveX = Input.GetAxis("Horizontal") * MoveSpeed* Time.deltaTime;
         MoveZ = Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;

         if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
         {
             newPos += MoveX * transform.right;
             newPos += MoveZ * transform.forward;
         }

        //control the mouse
         if (Input.GetAxis("Mouse ScrollWheel") != 0)
         {
             MoveScroll = Input.GetAxis("Mouse ScrollWheel") * ScrollWheelSpeed * Time.deltaTime;
             newPos += MoveScroll * transform.forward;
         }

         transform.position = newPos;
      
        //rotate
        if (Input.GetMouseButton(0))
        {
            RotateY += Input.GetAxis("Mouse X") * RotateSpeed * Time.deltaTime;
            RotateX -= Input.GetAxis("Mouse Y") * RotateSpeed * Time.deltaTime;
            rotation = Quaternion.Euler(RotateX, RotateY, transform.rotation.z);
        }
        transform.rotation = rotation;
        OnSelectPlanet();
	}


    public delegate void OnClickPlanet(string name);
    public event OnClickPlanet ClickPlanetEvent;

    void OnSelectPlanet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask mask = 1 << LayerMask.NameToLayer("Planets");

            if (Physics.Raycast(ray, out hit, 1000, mask.value))
            {

                Debug.Log(hit.collider.name);
                if (ClickPlanetEvent != null)
                {
                    ClickPlanetEvent(hit.collider.name);

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (UIManager.ui_instance.isOpenUIFlag)
            {
                UIManager.ui_instance.CloseUI();
                UIManager.ui_instance.isOpenUIFlag = false;
            }
            else
            {
                UIManager.ui_instance.OpenUI();
                UIManager.ui_instance.isOpenUIFlag = true;
            }
        }        
    }
}
