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
        int bestPriority = int.MaxValue;
        float bestDistance = Mathf.Infinity;


        foreach (Elevator elevator in elevators)
        {
            int priority = 3;

            if (elevator.IsGoingTowards(floorIndex, requestDirection))
            {
                priority = 1; // already moving in correct direction
            }
            else if (elevator.direction == Direction.Idle)
            {
                priority = 2; // already working but not perfect direction
            }
            else
            {
                priority = 3; // idle elevators last
            }


            float distance = Mathf.Abs(elevator.currentFloor - floorIndex);

            if (priority < bestPriority || (priority == bestPriority && distance < bestDistance))
            {
                bestPriority = priority;
                bestDistance = distance;
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
            if (elevator.currentFloor == floorIndex)
            {
                return true;
            }
        }
        return false;
    }
}