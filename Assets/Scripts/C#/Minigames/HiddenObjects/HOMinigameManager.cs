using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HOMinigameManager : MinigameManager
{
    public bool isEnd = false;

    bool go = true;

    private void Update()
    {
        if (isEnd)
        {
            HOStopPlayer();
        }
    }

    public override void StartNewMinigame()
    {
        Cursor.visible = false;
        cameraController.MoveToOffset(player.transform);
        cameraController.StartResetCameraToPlayer();
        player.motor.ResumeAgent();

        SetMinigameActive();
    }

    public override void EndMinigame()
    {
        isEnd = true;

        player.motor.ResumeAgent();
        player.motor.FollowTarget(GetComponent<Belphe>());
    }

    public void HOStopPlayer()
    {
        Belphe belphe = this.GetComponent<Belphe>();

        if (go && Vector3.Distance(this.transform.position, player.transform.position) <= belphe.radius)
        {
            player.motor.StopAgent();

            go = false;
        }
    }
}
