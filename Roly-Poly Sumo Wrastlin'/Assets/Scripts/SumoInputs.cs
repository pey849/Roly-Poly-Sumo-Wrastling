using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoInputs {

    private float horizontalMovement = 0;
    private float verticalMovement = 0;
    private bool swappedPrimary = false;

	public SumoInputs() {
		
	}

    public void setHorizontalMovemt(float hMovement)
    {
        horizontalMovement = hMovement;
    }

    public void setVerticalMovemt(float vMovement)
    {
        verticalMovement = vMovement;
    }

	public void setMovemt(float hMovement, float vMovement)
	{
		verticalMovement = vMovement;
		horizontalMovement = hMovement;
	}

	public void setSwappedProperty(bool swapped)
    {
        swappedPrimary = swapped;
    }

	public float getHorizontalMovemt()
	{
		return horizontalMovement;
	}

	public float getVerticalMovemt()
	{
		return verticalMovement;
	}

	public bool GetSwappedPrimary() {
		return this.swappedPrimary;
	}

}
