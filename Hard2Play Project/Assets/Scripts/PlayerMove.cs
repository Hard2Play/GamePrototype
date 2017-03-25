using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Player Stats")]
    public float speed = 10.0f;

    [Header("Controls Input Settings")]
    public string RightJoystickHorizontal;
    public string RightJoystickVertical;
    public string LeftJoystickHorizontal;
    public string LeftJoystickVertical;
    private UnityEngine.AI.NavMeshAgent pNavAgent;
    public GameObject playerMeshItem;
    public GameObject watchPoint;

    [Header("Camera Variable")]
    public GameObject CameraLeftRightRotationParent;
    public GameObject CameraTopDownRotationParent;
    public float cameraDownMaxRotation = 38.0f;
    public float cameraTopMaxRotation = 26.0f;
    private bool isCameraInversed = false;
    public float cameraSensitivity = 100.0f;
    // Use this for initialization
    void Start()
    {
        pNavAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        CameraControls();
        Move();
    }

    private void Move()
    {
        Vector3 move = Vector3.zero;
        Debug.Log(Input.GetAxis(LeftJoystickVertical));
        if (Input.GetAxis(LeftJoystickVertical) > 0.1f
            || Input.GetAxis(LeftJoystickVertical) < -0.1f)
        {
            move += Vector3.back * Time.deltaTime * speed * Input.GetAxis(LeftJoystickVertical);
        }
        if(Input.GetAxis(LeftJoystickHorizontal) > 0.1f
           || Input.GetAxis(LeftJoystickHorizontal) < -0.1f)
        {
            move += Vector3.right * Time.deltaTime * speed * Input.GetAxis(LeftJoystickHorizontal);
        }

        Quaternion rotation = Quaternion.Euler(CameraLeftRightRotationParent.transform.eulerAngles);
        move = rotation * move;
        pNavAgent.Move(move);
        watchPoint.transform.position = transform.position;
        watchPoint.transform.Translate(new Vector3(Input.GetAxis(LeftJoystickHorizontal), 0.0f, -Input.GetAxis(LeftJoystickVertical)), CameraLeftRightRotationParent.transform);
        playerMeshItem.transform.LookAt(watchPoint.transform);
    }

    private void CameraControls()
    {
        if (Input.GetAxis(RightJoystickHorizontal) > 0.1f)
        {
            CameraLeftRightRotationParent.transform.Rotate(((Vector3.up * Time.deltaTime) * Input.GetAxis(RightJoystickHorizontal)) * cameraSensitivity);
        }
        if (Input.GetAxis(RightJoystickHorizontal) < -0.1f)
        {
            CameraLeftRightRotationParent.transform.Rotate(((Vector3.up * Time.deltaTime) * Input.GetAxis(RightJoystickHorizontal)) * cameraSensitivity);
        }


        if (Input.GetAxis(RightJoystickVertical) > 0.1f)//camera down
        {
            if (CameraTopDownRotationParent.transform.eulerAngles.x >= (360.0f - cameraDownMaxRotation)
                || CameraTopDownRotationParent.transform.eulerAngles.x <= cameraTopMaxRotation)
                CameraTopDownRotationParent.transform.Rotate(((Vector3.right * Time.deltaTime) * Input.GetAxis(RightJoystickVertical)) * cameraSensitivity);
        }
        if (Input.GetAxis(RightJoystickVertical) < -0.1f)//camera up
        {
            if (CameraTopDownRotationParent.transform.eulerAngles.x >= (360.0f - cameraDownMaxRotation)
                || CameraTopDownRotationParent.transform.eulerAngles.x <= cameraTopMaxRotation)
                CameraTopDownRotationParent.transform.Rotate(((Vector3.right * Time.deltaTime) * Input.GetAxis(RightJoystickVertical)) * cameraSensitivity);
        }

        if (CameraTopDownRotationParent.transform.eulerAngles.x < 360.0f - cameraDownMaxRotation
            && CameraTopDownRotationParent.transform.eulerAngles.x > 360.0f - cameraDownMaxRotation - 10.0f)
            CameraTopDownRotationParent.transform.eulerAngles = new Vector3(360.0f - cameraDownMaxRotation, CameraTopDownRotationParent.transform.eulerAngles.y, 0.0f);

        if (CameraTopDownRotationParent.transform.eulerAngles.x > cameraTopMaxRotation
            && CameraTopDownRotationParent.transform.eulerAngles.x < cameraTopMaxRotation + 10.0f)
            CameraTopDownRotationParent.transform.eulerAngles = new Vector3(cameraTopMaxRotation, CameraTopDownRotationParent.transform.eulerAngles.y, 0.0f);

    }
}
