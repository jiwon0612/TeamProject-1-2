using UnityEngine;

public class PlayerLook : MonoBehaviour, IPlayerComponent
{
    [Header("Setting")] 
    [SerializeField] private float xSeneitivity = 30f;
    [SerializeField] private float ySeneitivity = 30f;
    
    public bool IsCantLook {get; set;}
    
    private Camera _cam;
    private float xRotation = 0f;

    private Player _player;
    
    public void Initialize(Player player)
    {
        _player = player;
        _cam = Camera.main; 
    }
    
    public void SetPlayerLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;
        
        xRotation -= (mouseY * Time.deltaTime) * ySeneitivity;
        xRotation = Mathf.Clamp(xRotation, -80f,  80f);
        
        _cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSeneitivity);
    }

    private void LateUpdate()
    {
        if (IsCantLook)
            SetPlayerLook(_player.InputCompo.MouseDir);
    }
}
