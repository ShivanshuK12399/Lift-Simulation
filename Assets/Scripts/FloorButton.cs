using UnityEngine;
using UnityEngine.UI;

public class FloorButton : MonoBehaviour
{
    public int floorIndex = 0;
    public Direction buttonDirection;

    private Image buttonLight;
    private ElevatorController elevatorController;
    private bool isActive = false;


    private void Start()
    {
        elevatorController = FindAnyObjectByType<ElevatorController>();
        buttonLight = GetComponent<Image>();
    }

    public void PressButton()
    {
        if (isActive || elevatorController.IsElevatorAlreadyPresent(floorIndex))
            return;

        isActive = true;
        buttonLight.color = Color.yellow;

        elevatorController.RequestElevator(floorIndex, buttonDirection);
    }

    public void ResetButton()
    {
        isActive = false;
        buttonLight.color = Color.white;
    }
}