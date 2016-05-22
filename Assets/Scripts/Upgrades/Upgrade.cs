using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Upgrade : MonoBehaviour {
    protected Player _ScenePlayer = null;
    protected UpgradeStruct _UpgradeInfo;
	// Update is called once per frame
	public abstract void UpgradeUpdate ();
}
