using UnityEngine;

public class PlayerAttack : MonoBehaviour, IPlayerComponent
{
    private Animator _animator;
    
    private Player _player;
    private bool _isComboAtk;
    private Katana _katana;
    
    private readonly int _atkHash = Animator.StringToHash("IsAtk");
    
    public void Initialize(Player player)
    {
        _player = player;
        
        _animator = GetComponent<Animator>();
        _katana = _player.GetComp<Katana>();
        _player.InputCompo.OnAttackEvent += ComboAttack;
    }

    private void OnDisable()
    {
        _player.InputCompo.OnAttackEvent -= ComboAttack;
        
    }

    public void ComboAttack()
    {
        _animator.SetTrigger(_atkHash);
    }

    public void Attack()
    {
        _katana.StartAttack();
    }
}
