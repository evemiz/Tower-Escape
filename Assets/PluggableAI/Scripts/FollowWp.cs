using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowWp : MonoBehaviour
{
    public GameObject wpManager;
    GameObject[] wps;
    GameObject currentNode;
    NavMeshAgent agent;

    public int indToGo = 2;

    private void Start()
    {
        wps = wpManager.GetComponent<WPManager>().wayPoints;
        currentNode = wps[0];

        agent = this.GetComponent<NavMeshAgent>();

        Invoke("GoToPlace", 2f);
    }

    public void GoToPlace()
    {
        agent.SetDestination(wps[indToGo].transform.position);
    }

    private void LateUpdate()
    {
    

    }
}
