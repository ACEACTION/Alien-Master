using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [SerializeField] Transform walkPoint1;
    [SerializeField] Transform walkPoint2;
    [SerializeField] Vector3 walkPointPos;
    [SerializeField] float moveSpeed;
    [SerializeField] bool isPlayer;
    [SerializeField] Vector3 resetPos;
    [SerializeField] float standingTime;

    void Start()
    {
        walkPointPos = walkPoint1.position;
    }

    
    void Update()
    {
        SetWalkPoint();
        Move();
    }
    void Move()
    {
        transform.position =  Vector3.MoveTowards(transform.position, walkPointPos, moveSpeed * Time.deltaTime);
    }

    void SetWalkPoint()
    {
        if (ArriveToPoint(walkPoint1.position))
        {
            walkPointPos = walkPoint2.position;
        }

        if (ArriveToPoint(walkPoint2.position))
        {
            walkPointPos = walkPoint1.position;
        }
        
    }


    bool ArriveToPoint(Vector3 p)
    {
        return Vector3.Distance(transform.position, p) < 2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //isPlayer = true;
            resetPos = walkPointPos;
            walkPointPos = transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ResetWalkPoint());
        }
    }


    IEnumerator ResetWalkPoint()
    {
        yield return new WaitForSeconds(standingTime);
        walkPointPos = resetPos;
    }

}
