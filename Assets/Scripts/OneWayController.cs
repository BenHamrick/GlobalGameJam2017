using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Collider2D))]

public class OneWayController : MonoBehaviour
{

    public String m_OneWayTag = "OneWayPlatform"; //Tag to use for one way platforms

    List<Collider2D> m_ActorColliders; //List of colliders on this character

    void Awake()
    {
        //Cache refrences to this character's colliders in a list
        m_ActorColliders = new List<Collider2D>();

        Collider2D[] colliders = GetComponents<Collider2D>();

        for (int i = 0; i < colliders.Length; i++) {

            //Exclude the one way trigger from the collider list
            if (!colliders[i].isTrigger)
                m_ActorColliders.Add(colliders[i]);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //When a platform with the one way tag enters the trigger
        //ignore collisions between it and this character
        if (other.tag == m_OneWayTag) {

            for (int i = 0; i < m_ActorColliders.Count; i++) {

                Physics2D.IgnoreCollision(other, m_ActorColliders[i], true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //When the platform exits the trigger re-enable collisions
        if (other.tag == m_OneWayTag) {

            for (int i = 0; i < m_ActorColliders.Count; i++) {

                Physics2D.IgnoreCollision(other, m_ActorColliders[i], false);
            }
        }
    }

    public void Ignorecollision(Collider2D other)
    {
        if (other.tag == m_OneWayTag) {

            for (int i = 0; i < m_ActorColliders.Count; i++) {

                Physics2D.IgnoreCollision(other, m_ActorColliders[i], true);
            }
        }
    }

    public void UnIgnorecollision(Collider2D other)
    {
        if (other.tag == m_OneWayTag) {

            for (int i = 0; i < m_ActorColliders.Count; i++) {

                Physics2D.IgnoreCollision(other, m_ActorColliders[i], false);
            }
        }
    }
}
