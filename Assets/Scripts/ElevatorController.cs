using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public Elevator[] elevators;

    public void RequestElevator(int floorIndex, Direction requestDirection)
    {
        Elevator bestElevator = null;
        int bestPriority = int.MaxValue;
        float bestDistance = Mathf.Infinity;

        foreach (Elevator elevator in elevators)
        {
            int priority = 3;

            if (elevator.direction == requestDirection)
            {
                if (requestDirection == Direction.Up && elevator.currentFloor <= floorIndex)
                {
                    priority = 1;
                }
                else if (requestDirection == Direction.Down && elevator.currentFloor >= floorIndex)
                {
                    priority = 1;
                }
            }
            else if (elevator.direction == Direction.Idle)
            {
                priority = 2;
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
            bestElevator.MoveToFloor(floorIndex);
        }
    }

    public void RequestElevatorUp(int floorIndex)
    {
        RequestElevator(floorIndex, Direction.Up);
    }
    public void RequestElevatorDown(int floorIndex)
    {
        RequestElevator(floorIndex, Direction.Down);
    }
}