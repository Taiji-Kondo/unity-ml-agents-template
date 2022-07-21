using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class RollerAgent : Agent
{
    private Rigidbody _rigidbody;

    public Transform target;
    public TargetManager targetManager;
    public float forceMultiplier = 10f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    public override void OnEpisodeBegin()
    {
        // If the Agent fell, reset its moment
        if (this.transform.localPosition.y < 0)
        {
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        // Move the Target to a new spot and move automatically
        target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
        targetManager.GotoNextPoint();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);

        // Agent velocity
        sensor.AddObservation(_rigidbody.velocity.x);
        sensor.AddObservation(_rigidbody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        _rigidbody.AddForce(controlSignal * forceMultiplier);

        // Rewards
        float distanceToTarget = Vector3.Distance(transform.localPosition, target.localPosition);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            EndEpisode();
        }
        else if (transform.localPosition.y < 0)
        {
            // Fell off platform
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionOut = actionsOut.ContinuousActions;
        continuousActionOut[0] = Input.GetAxis("Horizontal");
        continuousActionOut[1] = Input.GetAxis("Vertical");
    }
}
