using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SumoRole {SUMODUDE, SECONDARY};

public class SumoAssociations {

	//what role is this player?
	public SumoRole role;

	//the player ID of this association
	public int playerID;

	//Which of the two sumodudes am I associated with?
	//if role is sumodude, then this is the sumodude they are controlling
	//else this is the sumodude the secondary is currently on
	public int sumoDudeNumber;

	public SumoAssociations(SumoRole role, int playerID, int sumoDudeNumber) {
		this.role = role;
		this.playerID = playerID;
		this.sumoDudeNumber = sumoDudeNumber;
	}

	public SumoAssociations() {
		this.role = SumoRole.SUMODUDE;
		this.playerID = 1;
		this.sumoDudeNumber = 1;
	}
}
