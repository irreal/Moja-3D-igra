using UnityEngine;
using System.Collections;

public class NewBehaviourScript1 : MonoBehaviour {

	public GameObject follow;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		offset = transform.position - follow.GetComponent<Transform> ().position;
	}
	
	// Update is called once per frame
	void Update () {
		var pos = follow.GetComponent<Transform> ().position;
		transform.position = new Vector3 (pos.x + offset.x, transform.position.y, pos.z + offset.z);
	}
}
