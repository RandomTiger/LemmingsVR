using UnityEngine;

public class Entity : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		GetComponent<CharacterController>().SimpleMove(transform.forward);
	}

	void OnTriggerEnter(Collider other)
	{
		Director director = other.GetComponent<Director>();
		if (director)
		{
			transform.Rotate(Vector3.up, director.Turn);
		}
	}
}
