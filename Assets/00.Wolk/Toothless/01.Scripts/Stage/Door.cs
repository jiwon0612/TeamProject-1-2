using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsTarget;
    [SerializeField] private int stageNumber;
    
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & whatIsTarget) != 0)
        {
            Debug.Log(stageNumber);
            //SceneManager.LoadScene(stageNumber);
        }
    }
}
