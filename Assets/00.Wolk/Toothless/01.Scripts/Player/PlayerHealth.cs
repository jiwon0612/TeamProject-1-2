using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    public int CurretnHealth { get; private set; }
    public bool IsCanHit { get; private set; }

    public UnityEvent<float> OnHitEvent;
    public UnityEvent OnDeathEvent;
    

    private void Awake()
    {
        CurretnHealth = maxHealth;
        IsCanHit = true;
    }

    public bool TakeDamage(int damage)
    {
        if (IsCanHit == false) return false;
        
        CurretnHealth -= damage;
        OnHitEvent.Invoke((float)CurretnHealth / maxHealth);
        
        if (CurretnHealth <= 0)
            OnDeathEvent.Invoke();

        return true;
    }

    public void Death()
    {
        IsCanHit = false;
        GameManager.Instance.SetScene(SceneManager.GetActiveScene());
        OnDeathEvent.Invoke();
    }
}
