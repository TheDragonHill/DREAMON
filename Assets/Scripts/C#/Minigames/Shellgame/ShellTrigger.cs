using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellTrigger : Interactable
{
	ShellgameManager shellgameManager;

    protected override void InitValues()
    {
        base.InitValues();
		shellgameManager = GetComponentInParent<ShellgameManager>();

	}

    public override void Interact()
	{
		SelectShell();
	}

	public void SelectShell()
	{
		shellgameManager.RevealShell(this);
	}
}
