using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    public Transform[] floors;

    void Awake()
    {
        Instance = this;
    }
}