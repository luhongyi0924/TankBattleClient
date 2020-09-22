using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //距离矢量
    public Vector3 distance = new Vector3(0, 8, -18);
    //public Vector3 distance = new Vector3(0, 15, -22);//x代表角度
    //相机
    //public Camera camera;
    //偏移值
    public Vector3 offset = new Vector3(0, 5f, 0);
    //相机移动速度
    public float speed = 18f;
    //炮塔
    public GameObject turretObj;
    //炮管
    public GameObject gunObj;
    //使否重置位置
    private bool isReset;
    ////最大和最小距离
    //public float minDistanceZ = -35f;
    //public float maxDistanceZ = -10f;
    ////距离变化速度
    //public float zoomSpeed = 2f;

    void Start()
    {
        ////默认为主相机
        //camera = Camera.main;
        //炮塔
        turretObj = GameObject.FindGameObjectWithTag("Turret");
        //炮管
        gunObj = GameObject.FindGameObjectWithTag("Gun");
        //初次重置相机的位置和视角
        FollowTurret();
        ////相机初始位置
        //Vector3 pos = transform.position;
        //Vector3 forward = transform.forward;
        //Vector3 initPos = pos - 30 * forward + Vector3.up * 10;
        //camera.transform.position = initPos;
    }

    ////调整距离
    //void Zoom()
    //{
    //    float axis = Input.GetAxis("Mouse ScrollWheel");
    //    distance.z += axis * zoomSpeed;
    //    distance.z = Mathf.Clamp(distance.z, minDistanceZ, maxDistanceZ);
    //}

    void Update()
    {
        Debug.Log( ETCInput.GetAxis("FreeLookHorizontal") !=0);
        if (ETCInput.GetButton("ResetButton"))
        {
            isReset = true;
        }

        if (ETCInput.GetAxis("FreeLookHorizontal") != 0 || ETCInput.GetAxis("FreeLookVertical") != 0)
        {
            isReset = false;
        }

        if (isReset)
        {
            //炮管位置
            Vector3 turretPos = turretObj.transform.position;
            //相机目标位置
            Vector3 targetPos = turretPos + transform.forward * distance.z - offset;
            targetPos.y += distance.y;
            //相机位置
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetPos, Time.deltaTime * speed);
            //对准炮管
            Camera.main.transform.LookAt(turretPos);
        }
        else if (!isReset)
        {
            FollowTurret();
        }
    }

    ////调整角度
    //void Rotate()
    //{
    //    if (!Input.GetMouseButton(1))
    //    {//右键
    //        return;
    //    }
    //    float axis = Input.GetAxis("Mouse X");
    //    distance.x += 2 * axis;
    //    distance.x = Mathf.Clamp(distance.x, -20, 20);
    //}

    //所有组件update之后发生
    //void LateUpdate()
    //{
    //    //坦克位置
    //    Vector3 pos = transform.position;
    //    //坦克方向
    //    Vector3 forward = transform.forward;
    //    Vector3 rigiht = transform.right;
    //    //相机目标位置
    //    Vector3 targetPos = pos;
    //    targetPos = pos + forward * distance.z + rigiht * distance.x;
    //    targetPos.y += distance.y;
    //    //相机位置
    //    Vector3 cameraPos = camera.transform.position;
    //    cameraPos = Vector3.MoveTowards(cameraPos, targetPos, Time.deltaTime * speed);
    //    camera.transform.position = cameraPos;
    //    //对准坦克
    //    Camera.main.transform.LookAt(pos + offset);
    //    //调整距离
    //    Zoom();
    //    //调整角度
    //    Rotate();
    //}
    private void FollowTurret()
    {
        //炮管位置
        Vector3 turretPos = turretObj.transform.position;
        //炮管方向
        Vector3 turretfor = -turretObj.transform.up;
        //相机目标位置
        Vector3 targetPos = turretPos + turretfor * distance.z - offset;
        targetPos.y += distance.y;
        //相机位置
        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetPos, Time.deltaTime * speed);
        //对准炮管
        Camera.main.transform.LookAt(turretPos);
    }
}