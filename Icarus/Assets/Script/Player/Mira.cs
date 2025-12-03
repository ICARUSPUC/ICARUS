using UnityEngine;

public class Mira : MonoBehaviour
{
    public Transform PlayerTransform;
    [SerializeField]
    private float velocity = 5.0f;

 
    private Player playerComponent;
    private Ray ray;
    private RaycastHit hit;

    void Awake()
    {
     
        if (PlayerTransform != null)
        {
            playerComponent = PlayerTransform.GetComponent<Player>();
        }
        
    }

    private void Update()
    {
        
        if (playerComponent == null || PlayerTransform == null)
            return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

      
        if (!Physics.Raycast(ray, out hit))
            return;

        Vector3 pos = hit.point - PlayerTransform.position;

    
        Quaternion rotacaoinvertida = Quaternion.LookRotation(pos);
        Quaternion rotacaofinal = rotacaoinvertida * Quaternion.Euler(0, -90, 0);


        Quaternion smoothedRotation = Quaternion.Lerp(PlayerTransform.rotation, rotacaofinal, velocity * Time.deltaTime);

   
        Vector3 finalAngles = smoothedRotation.eulerAngles;
        float miraY = finalAngles.y;
        float miraZ = finalAngles.z;

       
        float currentInput = Player.CurrentHorizontalInput;

        
        float tiltX = playerComponent.ApplyTilt(currentInput);

        PlayerTransform.rotation = Quaternion.Euler(tiltX, miraY, miraZ);
    }
}