using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimer : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private TextMeshProUGUI climberTimer;
    [SerializeField] private TextMeshProUGUI dashTimer;
    [SerializeField] private TextMeshProUGUI doubleJumpTimer;


    private void Update()
    {
        climberTimer.text = "Climber Timer:" + playerController.GetClimbTimer();

        dashTimer.text = "Dasher Timer:" + playerController.GetDashTimer();

        doubleJumpTimer.text = "Double Jump Timer:" + playerController.GetDoubleJumpTimer();
    }
}
