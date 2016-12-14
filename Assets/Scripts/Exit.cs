using UnityEngine;

public class Exit : MonoBehaviour
{
    int escapeCount;

    void OnTriggerStay(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity && entity.IsGrounded())
        {
            if(entity.MarkForEscape())
            {
                escapeCount++;
            }
        }
    }
}
