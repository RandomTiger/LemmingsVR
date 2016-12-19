using UnityEngine;
using PicaVoxel;

// todo: Setup paths with voxels

// todo: easy
// todo: Stoppers
// todo: exploder (anim)

// 2ndry
// todo: builder
// todo: escape (anim)
// todo: drown (anim)
// todo: splash (anim)
// todo: Climb
// todo: dig down
// todo: dig sideways
// todo: dig diagonal

public class Entity : MonoBehaviour
{
    [System.Flags]
    public enum Behaviour
    {
		Default		= 0,
		Stop		= 1 << 1,
		Build		= 1 << 2,
		Parachute	= 1 << 3,
		Climb		= 1 << 4,
		Explode		= 1 << 5,
		DigDown		= 1 << 6,
		DigSideways = 1 << 7,
		DigDiagonal = 1 << 8,
	}

    float fallingTime;
    const float fallTolerence = 1.5f;

    CharacterController character;
    [EnumFlag]
	public Behaviour behaviour = Behaviour.Default;

    float parachuteTime = 6;
    float currentParachuteTime;
	float parachuteMassModifier = 0.3f;
	float originalMass;

    float explodeTimer = 5;

    public enum Animations
    {
        Walking,
        Falling,
        Parachute,
        Stopper
    }

    public GameObject[] animations;

    void Start()
    {
        character = GetComponent<CharacterController>();
        currentParachuteTime = parachuteTime;
    }

    void Update ()
	{
        if(CheckBehaviour(Behaviour.Explode))
        {
            explodeTimer -= Time.deltaTime;
            if(explodeTimer < 0)
            {
                Explode();
                return;
            }
        }

        if (character.isGrounded)
		{
            if (fallingTime > fallTolerence)
			{
				Die();
			}
			else if (fallingTime > 0)
			{
				ProcessJustBackOnGround();
			}

			if (CheckBehaviour(Behaviour.Stop))
			{
				UpdateStationary();
			}
			else
			{
				UpdateWalk();
			}
		}
		else // we are falling
		{
            float gravity = 9.8f;

            if (CheckBehaviour(Behaviour.Parachute) && currentParachuteTime > 0)
			{
                SetActiveAnim(Animations.Parachute);

                currentParachuteTime -= Time.deltaTime;
                character.Move(Vector3.down * gravity * parachuteMassModifier * Time.deltaTime);
            }
            else
			{
                SetActiveAnim(Animations.Falling);

                fallingTime += Time.deltaTime;
                character.SimpleMove(Vector3.down * gravity * Time.deltaTime);

            }
		}
	}

    void SetActiveAnim(Animations anim)
    {
        for (int i = 0; i < animations.Length; i++)
        {
            if(animations[i] == null)
            {
                continue;
            }

            if(i == (int) anim)
            {
                if (animations[i].activeSelf == false)
                {
                    animations[i].SetActive(true);
                }
            }
            else
            {
                if(animations[i].activeSelf)
                {
                    animations[i].SetActive(false);
                }
            }
        }
    }

    void ProcessJustBackOnGround()
	{
		fallingTime = 0;
		currentParachuteTime = parachuteTime;
	}

    public bool IsGrounded() { return character.isGrounded; }

	bool CheckBehaviour(Behaviour check)
	{
		return (behaviour & check) == check;
	}

	bool AddBehaviour(Behaviour add)
	{
		if (CheckBehaviour(add))
		{
			// already set
			return false;
		}

		switch(add)
		{
			case Behaviour.Parachute:
				currentParachuteTime = parachuteTime;
				break;
		}

		behaviour |= add;
		return true;
	}

	void RemoveBehaviour(Behaviour remove)
	{
		Debug.Assert(CheckBehaviour(remove), "Behaviour must be there to be removed");
		behaviour &= ~remove;
	}

    void UpdateWalk()
    {
        SetActiveAnim(Animations.Walking);
        character.SimpleMove(transform.forward);
    }

    void UpdateStationary()
    {
        SetActiveAnim(Animations.Stopper);
        character.SimpleMove(Vector3.zero);
    }

    Transform FindTop(Transform node)
    {
        while(node.parent != null)
        {
            node = node.parent;
        }

        return node;
    }

    void OnTriggerEnter(Collider other)
	{
        bool isSelf = FindTop(transform) == FindTop(other.transform);
        if (isSelf)
        {
            return;
        }

        if (other.transform.tag == "Stopper")
        {
            transform.Rotate(Vector3.up, 180);
        }
    }

    public void Explode()
    {
        GetComponent<Exploder>().Explode();
        Die();
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
