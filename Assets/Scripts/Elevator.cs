using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [Header("Refrences")]
    [SerializeField] Transform cabel;
    [SerializeField] TMP_Text currentFloorText;

    [Header("Movement")]
    [SerializeField] float speed = 3f;
    [SerializeField] float floorStopTime = 1.5f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    [Header("State")]
    public Direction direction = Direction.Idle;
    public int currentFloor = 0;
    private int targetFloor = -1;

    private List<int> requests = new List<int>();
    private FloorButton[] floorButtons;


    private void Start()
    {
        floorButtons = FindObjectsOfType<FloorButton>();
    }

    void Update()
    {
        if (!isMoving)
            return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        UpdateCurrentFloor();

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


    IEnumerator FloorStop()
    {
        direction = Direction.Idle;
        isMoving = false;

        // Reset the button light for the current floor
        foreach (FloorButton button in floorButtons)
        {
            if (button.floorIndex == currentFloor)
            {
                button.ResetButton();
            }
        }

        yield return new WaitForSeconds(floorStopTime);
        ProcessNextRequest();
    }


    public void MoveToFloor(int floorIndex)
    {
        if (requests.Contains(floorIndex)) return; // Already requested
        if (floorIndex == currentFloor && !isMoving) return; // Already at the floor and not moving

        requests.Add(floorIndex);
        SortRequests();

        if (!isMoving)
        {
            ProcessNextRequest();
        }
        else
        {
            UpdateTargetIfNeeded();
        }
    }


    void ProcessNextRequest()
    {
        if (requests.Count == 0)
        {
            direction = Direction.Idle;
            return;
        }

        targetFloor = requests[0];
        requests.RemoveAt(0);

        if (targetFloor > currentFloor) direction = Direction.Up;
        else if (targetFloor < currentFloor) direction = Direction.Down;
        else direction = Direction.Idle;

        targetPosition = new Vector3(transform.position.x, BuildingManager.Instance.floors[targetFloor].position.y, transform.position.z);

        isMoving = true;
    }


    void UpdateTargetIfNeeded()
    {
        if (requests.Count == 0) return;

        if (direction == Direction.Up)
        {
            if (requests[0] < targetFloor && requests[0] > currentFloor)
            {
                targetFloor = requests[0];
                targetPosition = new Vector3(
                    transform.position.x,
                    BuildingManager.Instance.floors[targetFloor].position.y,
                    transform.position.z
                );

                requests.RemoveAt(0);
            }
        }
        else if (direction == Direction.Down)
        {
            if (requests[0] > targetFloor && requests[0] < currentFloor)
            {
                targetFloor = requests[0];
                targetPosition = new Vector3(
                    transform.position.x,
                    BuildingManager.Instance.floors[targetFloor].position.y,
                    transform.position.z
                );

                requests.RemoveAt(0);
            }
        }
    }

    void SortRequests()
    {
        if (direction == Direction.Up)
        {
            requests.Sort();
        }
        else if (direction == Direction.Down)
        {
            requests.Sort();
            requests.Reverse();
        }
    }
}