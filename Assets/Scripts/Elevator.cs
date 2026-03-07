using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Elevator : MonoBehaviour
{

    [Header("Refrences")]
    [SerializeField] Transform cabel;
    [SerializeField] TMP_Text currentFloorText;
    private ElevatorController elevatorController;


    [Header("State")]
    public Direction direction = Direction.Idle;
    public int currentFloor = 0;
    private int targetFloor = -1;


    private List<int> upRequests = new List<int>(); // Contains the list of requested floors in the upward direction
    private List<int> downRequests = new List<int>(); // Contains the list of requested floors in the downward direction
    private FloorButton[] floorButtons;

    private Vector3 targetPosition;
    private bool isMoving = false;



    private void Start()
    {
        floorButtons = FindObjectsByType<FloorButton>(FindObjectsSortMode.None);
        elevatorController = FindAnyObjectByType<ElevatorController>();
    }

    void Update()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, elevatorController.speed * Time.deltaTime);
        UpdateCurrentFloor();  // Update the current floor based on the elevator's position

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            StartCoroutine(FloorStop());
        }
    }


    void UpdateCurrentFloor()
    {
        float minDistance = float.MaxValue;
        int closestFloor = currentFloor;

        for (int i = 0; i < BuildingManager.Instance.floors.Length; i++)
        {
            float dist = Mathf.Abs(transform.position.y - BuildingManager.Instance.floors[i].position.y);

            if (dist < minDistance)
            {
                minDistance = dist;
                closestFloor = i;
            }
        }

        if (closestFloor != currentFloor)
        {
            currentFloor = closestFloor;
            currentFloorText.text = currentFloor.ToString();
        }
    }


    public void MoveToFloor(int floorIndex, Direction requestDirection)
    {
        // Gives the Up/Down requestList of the elevator
        List<int> requests = GetRequestList(requestDirection);

        if (requests.Contains(floorIndex)) return; // Already requested
        if (floorIndex == currentFloor && !isMoving) return; // Already at the floor and not moving

        requests.Add(floorIndex);
        SortRequests(requestDirection);

        // Print all request values on one line
        string directionStr = requestDirection == Direction.Up ? "Up" : "Down";
        print($"{gameObject.name} going {directionStr}: {string.Join("->", requests)}");

        if (direction == Direction.Idle)
        {
            if (floorIndex > currentFloor) direction = Direction.Up;
            else if (floorIndex < currentFloor) direction = Direction.Down;
        }

        ProcessNextRequest();
    }


    void ProcessNextRequest()
    {
        List<int> requests = null;

        if (direction == Direction.Up)
        {
            if (upRequests.Count > 0)
                requests = upRequests;
            else if (downRequests.Count > 0)
            {
                direction = Direction.Down;
                requests = downRequests;
            }
        }
        else if (direction == Direction.Down)
        {
            if (downRequests.Count > 0)
                requests = downRequests;
            else if (upRequests.Count > 0)
            {
                direction = Direction.Up;
                requests = upRequests;
            }
        }
        else // Idle
        {
            if (upRequests.Count > 0)
            {
                direction = Direction.Up;
                requests = upRequests;
            }
            else if (downRequests.Count > 0)
            {
                direction = Direction.Down;
                requests = downRequests;
            }
        }

        if (requests == null || requests.Count == 0)
        {
            direction = Direction.Idle;
            return;
        }

        targetFloor = requests[0];

        if (targetFloor > currentFloor) direction = Direction.Up;
        else if (targetFloor < currentFloor) direction = Direction.Down;

        targetPosition = new Vector3(
            transform.position.x,
            BuildingManager.Instance.floors[targetFloor].position.y,
            transform.position.z
        );

        isMoving = true;
    }

    public bool IsGoingTowards(int floorIndex, Direction requestDirection)
    {
        if (direction == Direction.Up && floorIndex >= currentFloor)
            return true;

        if (direction == Direction.Down && floorIndex <= currentFloor)
            return true;

        return false;
    }

    public bool HasPendingRequests()
    {
        return upRequests.Count > 0 || downRequests.Count > 0;
    }

    public int GetPendingRequestCount()
    {
        return upRequests.Count + downRequests.Count;
    }

    public bool HasRequest(int floor, Direction dir)
    {
        if (dir == Direction.Up)
            return upRequests.Contains(floor);

        if (dir == Direction.Down)
            return downRequests.Contains(floor);

        return false;
    }

    IEnumerator FloorStop()
    {
        if (direction==Direction.Up) upRequests.Remove(targetFloor);
        if (direction == Direction.Down) downRequests.Remove(targetFloor);

        // Reset the button light for the current floor
        foreach (FloorButton button in floorButtons)
        {
            if (button.floorIndex == currentFloor && button.buttonDirection == direction)
            {
                button.ResetButton();
            }
        }

        direction = Direction.Idle;
        isMoving = false;

        yield return new WaitForSeconds(elevatorController.floorStopTime);
        ProcessNextRequest();
    }


    void SortRequests(Direction requestDirection)
    {
        List<int> requests = GetRequestList(requestDirection);

        if (requestDirection == Direction.Up)
        {
            requests.Sort();
        }
        else if (requestDirection == Direction.Down)
        {
            requests.Sort();
            requests.Reverse();
        }
    }

    List<int> GetRequestList(Direction requestDirection)
    {
        switch (requestDirection)
        {
            case Direction.Up:
                return upRequests;

            case Direction.Down:
                return downRequests;
        }

        // Return null list if list if idle;
        return new List<int>();
    }
}