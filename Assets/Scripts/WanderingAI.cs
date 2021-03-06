﻿using UnityEngine;
using System.Collections;

public class WanderingAI : MonoBehaviour
{
    public float speed = 3.0f;
    public float obstacleRange = 5.0f;

    public enum state { ALIVE, DEAD }

    public state life = state.ALIVE;

    [SerializeField]
    private GameObject fireballPrefab;

    private GameObject _fireball;

    // Update is called once per frame
    void Update()
    {
        switch (life)
        {
            case state.ALIVE:
                transform.Translate(0, 0, speed * Time.deltaTime);

                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;

                if (Physics.SphereCast(ray, .75f, out hit))
                {
                    GameObject hitObject = hit.transform.gameObject;

                    if (hitObject.GetComponent<PlayerCharacter>())
                    {
                        Debug.Log("found player");
                        if (_fireball == null)
                        {
                            _fireball = Instantiate(fireballPrefab) as GameObject;
                            _fireball.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                            _fireball.transform.rotation = transform.rotation;


                        }
                    }
                    else if (hit.distance < obstacleRange)
                    {
                        float angle = Random.Range(-110, 110);
                        transform.Rotate(0, angle, 0);

                    }
                }
                break;

            default:
                Destroy(this);
                break;
        }

    }
}
