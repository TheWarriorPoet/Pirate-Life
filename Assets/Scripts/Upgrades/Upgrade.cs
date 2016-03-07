using UnityEngine;
using System.Collections;

public abstract class Upgrade : MonoBehaviour {
    public bool UpgradeActive = false;
    protected Player _ScenePlayer = null;

	// Update is called once per frame
	public abstract void UpgradeUpdate ();
}
