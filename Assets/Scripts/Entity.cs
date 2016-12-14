using UnityEngine;

// todo: Walk (anim)
// todo: Fall (anim)
// todo: Stoppers
// todo: builder
// todo: parachute
// todo: Climb
// todo: exploder
// todo: drown (anim)
// todo: splash (anim)
// todo: escape (anim)
// todo: dig down
// todo: dig sideways
// todo: dig diagonal

public class Entity : MonoBehaviour
{
    float fallingTime;
    const float fallTolerence = 1.5f;

    CharacterController character;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update ()
	{
        if(character.isGrounded)
        {
            UpdateNotFalling();
            UpdateWalk();
        }
        else
        {
            UpdateFalling();
            UpdateStationary();
        }
    }

    public bool IsGrounded() { return character.isGrounded; }

    void UpdateFalling()
    {
        fallingTime += Time.deltaTime;
    }

    void UpdateNotFalling()
    {
        if (fallingTime > fallTolerence)
        {
            Die();
        }
        fallingTime = 0;
    }

    void UpdateWalk()
    {
        character.SimpleMove(transform.forward);
    }

    void UpdateStationary()
    {
        character.SimpleMove(Vector3.zero);
    }

    void OnTriggerEnter(Collider other)
	{
		Director director = other.GetComponent<Director>();
		if (director)
		{
			transform.Rotate(Vector3.up, director.Turn);
		}
	}

    public void Die()
    {
        Destroy(gameObject);
    }

    public bool MarkForEscape()
    {
        Destroy(gameObject);
        return true;
    }
}
