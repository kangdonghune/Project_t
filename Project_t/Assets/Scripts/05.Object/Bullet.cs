using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    public Transform Target { get; private set; }
    public float speed = 5.0f;
    public Vector3 height;
    public GameObject Master { get; set; }

    private void Start()
    {
        height = Vector3.up * (Target.gameObject.GetComponent<Collider>().bounds.size.y / 2);
    }

    private void Update()
    {
        Vector3 targetPos = Target.position + height;
        transform.LookAt(targetPos);// Ÿ�� �������� ȸ��
        float dist = (targetPos - transform.position).magnitude;
        if (dist < 0.05f)
        {
            Collision();
            return;
        }
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;


    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    void Collision()
    {
        if(photonView.IsMine == true)
        {
            Target.GetComponent<IDamageable>().OnDamged(Master);
            Debug.Log("���� �����");
            Managers.Resource.Destroy(gameObject);
        }
    
    }
}
