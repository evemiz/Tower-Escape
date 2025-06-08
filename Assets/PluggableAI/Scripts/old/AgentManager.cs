using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentManager : MonoBehaviour
{

    // List<NavMeshAgent> agents = new List<NavMeshAgent>();

    // private void Start()
    // {
    //     GameObject[] a = GameObject.FindGameObjectsWithTag("AI");
    //     foreach (GameObject go in a)
    //     {
    //         agents.Add(go.GetComponent<NavMeshAgent>());
    //     }
    // }
    
    // private void Update() {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         RaycastHit hit;
    //         if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
    //         {
    //             foreach (NavMeshAgent a in agents)
    //             {
    //                 a.SetDestination(hit.point);
    //             }
    //         }
    //     }
    // }
}
