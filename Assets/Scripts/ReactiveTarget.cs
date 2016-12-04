using UnityEngine;
using System.Collections;

public class ReactiveTarget : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReactToHit()
    {
        WanderingAI wanderingAI = GetComponent<WanderingAI>();
        if (wanderingAI != null)
        {
            wanderingAI.life = WanderingAI.state.DEAD;
        }
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        this.transform.Rotate(-75, 0, 0);
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
