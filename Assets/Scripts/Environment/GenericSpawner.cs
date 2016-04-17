using UnityEngine;
using System.Collections;

public class GenericSpawner : MonoBehaviour
{
	public GameObject prefab;
	public uint amount;
	public Vector3 offset;

	void Start()
	{
		for (int i = 0; i < amount; i++)
		{
			GameObject go = (GameObject)Instantiate(prefab, transform.position + offset, transform.rotation);

			go.transform.parent = transform.parent;
		}
	}
}
