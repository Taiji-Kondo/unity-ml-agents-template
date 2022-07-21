using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class TargetManager : MonoBehaviour
{
    private NavMeshAgent _agent;
    // private bool _isMoving;

    public Transform floor;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.autoBraking = false;

        GotoNextPoint();
    }

    private void Update()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }

    public void GotoNextPoint()
    {
        // _isMoving = true;
        _agent.isStopped = false;

        var floorLocalScale = floor.localScale * 10;
        float posX = Random.Range(-1 * floorLocalScale.x, floorLocalScale.x);
        float posZ = Random.Range(-1 * floorLocalScale.z, floorLocalScale.z);

        Vector3 pos = floor.position;
        pos.x += posX;
        pos.z += posZ;

        _agent.destination = pos;
    }
}
