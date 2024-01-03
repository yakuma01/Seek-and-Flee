using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    public float maxSpeed = 5;
    public float maxSteering = 1;

    Vector3 velocity;
    Vector3 steering;

    public float accuracy = 2;
    public List<GameObject> waypoints;
    public GameObject parent;
    int currentWaypoint = 0;

    public GameObject target;
    
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform wp in parent.transform)
        {
            waypoints.Add(wp.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        target = waypoints[currentWaypoint];
        
        if ((Vector3.Distance(this.transform.position, target.transform.position)) < accuracy)
        {
            currentWaypoint += 1;
            if (currentWaypoint >= waypoints.Count)
            {
                currentWaypoint = 0;
            }
        }

        steering = CalculateSeek(target.transform.position);
        //steering = CalculateFlee(target.transform.position);
        //clamp steering to max value;
        steering = Vector3.ClampMagnitude(steering, maxSteering);


        velocity += steering * Time.deltaTime;
        //ensure we don't exceed the max speed
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        this.transform.position += velocity * Time.deltaTime;
    }

    public Vector3 CalculateSeek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = targetPos - this.transform.position;
        desiredVelocity = desiredVelocity.normalized;
        desiredVelocity *= maxSpeed;

        Debug.DrawRay(this.transform.position, velocity, Color.green);
        Debug.DrawRay(this.transform.position, desiredVelocity, Color.red);
        Debug.DrawRay(this.transform.position, steering, Color.blue);
        return desiredVelocity - this.velocity;
    }


    public Vector3 CalculateFlee(Vector3 targetPos)
    {
        Vector3 desiredVelocity =  this.transform.position - targetPos;
        desiredVelocity = desiredVelocity.normalized;
        desiredVelocity *= maxSpeed;

        Debug.DrawRay(this.transform.position, velocity, Color.green);
        Debug.DrawRay(this.transform.position, desiredVelocity, Color.red);
        Debug.DrawRay(this.transform.position, steering, Color.blue);
        return desiredVelocity - this.velocity;
    }
}
