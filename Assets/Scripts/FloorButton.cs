using UnityEngine;
using UnityEngine.UI;

public class FloorButton : MonoBehaviour
{
    public int floorIndex = 0;

    private Image buttonLight;
    private ElevatorController elevatorController;
    private bool isActive = false;

    private void Start()
    {
        elevatorController = FindAnyObjectByType<ElevatorController>();
        buttonLight = GetComponent<Image>();
    }

    void PressButton()
    {
        if (isActive)
            return;

        isActive = true;
        buttonLight.color = Color.yellow;
    }

    public void PressUpButton()
    {
        PressButton();
        elevatorController.RequestElevator(floorIndex, Direction.Up);
    }

    public void PressDownButton()
    {
        PressButton();
        elevatorController.RequestElevator(floorIndex, Direction.Down);
    }

    public void ResetButton()
    {
        isActive = false;
        buttonLight.color = Color.white;
    }
}