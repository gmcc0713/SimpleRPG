using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour
{

    private void Update()
    {
        //ī�޶�� �÷��̾� ������ �Ÿ�
        float Distance = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        //ī�޶� �÷��̾ ���� ������
        Vector3 Direction = (PlayerController.Instance.transform.position - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Direction, out hit, Distance))
        {
            
        }
    }

}
