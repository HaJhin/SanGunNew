using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // 따라갈 대상 (보통 플레이어)
    public Vector3 offset;         // 카메라와 대상 사이의 거리

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x + offset.x, offset.y, offset.z);
        }
    }
}

