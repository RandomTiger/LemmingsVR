using UnityEngine;

public class Entrance : MonoBehaviour
{
	public GameObject toSpawn;

	public float frequency = 1.0f;
	public float quantity = 50;

	private float timer;

	void Start()
	{
		timer = frequency;
	}

	void Update()
	{
		if (quantity > 0)
		{
			timer -= Time.deltaTime;
			if (timer < 0)
			{
				timer = frequency;
				quantity--;
				Instantiate(toSpawn, transform.position, transform.rotation);
			}
		}
	}

}
