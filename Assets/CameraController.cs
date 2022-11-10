using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed;
    [SerializeField] private float rotateFollowSpeed;
    [SerializeField] private Vector3 rotOffset;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        transform.position = target.position + target.TransformDirection(offset);//Vector3.Lerp(transform.position, target.position + target.TransformDirection(offset), Time.deltaTime * followSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation * Quaternion.Euler(rotOffset), Time.deltaTime * rotateFollowSpeed);
    }


}
