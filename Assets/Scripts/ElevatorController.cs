using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 100f;
    public float floorStopTime = 1.5f;

    public Elevator[] elevators;
    

    public void RequestElevator(int floorIndex, Direction requestDirection)
    {
        Elevator bestElevator = null;
        float bestDistance = Mathf.Infinity;


        foreach (Elevator elevator in elevators)
        {
            float distance = Mathf.Abs(elevator.currentFloor - floorIndex);
            int load = elevator.GetPendingRequestCount();

            float directionPenalty = 0;

            if (!elevator.IsGoingTowards(floorIndex, requestDirection))
                directionPenalty = 5f; // discourage wrong direction elevators

            if (elevator.HasRequest(floorIndex, Direction.Up) || elevator.HasRequest(floorIndex, Direction.Down))
                continue;

            float score = distance + load * 3 + directionPenalty;

            if (score < bestDistance)
            {
                bestDistance = score;
                bestElevator = elevator;
            }
        }

        if (bestElevator != null)
        {
            bestElevator.MoveToFloor(floorIndex, requestDirection);
        }
    }

    public bool IsElevatorAlreadyPresent(int floorIndex)
    {
        foreach (Elevator elevator in elevators)
        {
            if (elevator.currentFloor == floorIndex && !elevator.HasPendingRequests())
            {
                return true;
            }
        }
        return false;
    }
}