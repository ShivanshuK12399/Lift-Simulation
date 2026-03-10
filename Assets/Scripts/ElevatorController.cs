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
            float distance = Mathf.Abs(elevator.currentFloor - floorIndex); // dis btw current floor and requested floor
            int load = elevator.GetPendingRequestCount(); // how many pending requests the elevator has

            float directionPenalty = 0;

            if (!elevator.IsGoingTowards(floorIndex, requestDirection)) // not going towards the requested floor
                directionPenalty = 5f; // discourage wrong direction elevators

            // If elevator already has UP or DOWN request for this floor, skip it - this is just a safety fallback
            if (elevator.HasRequest(floorIndex, Direction.Up) || elevator.HasRequest(floorIndex, Direction.Down))
                continue;

            // Combine distance, load, and direction penalty into a single score
            // lower the score the better the elevator is for this request
            float score = distance + load * 3 + directionPenalty;

            if (bestDistance > score)
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
            // if elevator is already at the requested floor and has no pending requests, consider it available
            if (elevator.currentFloor == floorIndex && !elevator.HasPendingRequests()) 
            {
                return true;
            }
        }
        return false;
    }
}