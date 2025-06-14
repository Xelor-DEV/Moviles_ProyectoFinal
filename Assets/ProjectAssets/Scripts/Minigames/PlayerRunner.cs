using UnityEngine;

public class PlayerRunner : MonoBehaviour
{
       [Header("Configuraci�n")]
 public float jumpForce = 8f;
    public bool isGrounded = true;

    [Header("Referencias")]
    [SerializeField] private ExternalRunner gameManager; 
    private Rigidbody _comprigidbody;

    private Vector3 tama�oNormal = new Vector3(1, 1, 1);
    private Vector3 tama�oTransformado = new Vector3(1, 0.6828f, 1);
    private bool shrinkPressed = false;
    void Awake()
    {
        _comprigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
      
            if (isGrounded)
            {
                if (shrinkPressed && transform.localScale != tama�oTransformado)
                {
                    float deltaY = (tama�oNormal.y - tama�oTransformado.y) * 0.5f;
                    transform.localScale = tama�oTransformado;
                    transform.position -= new Vector3(0, deltaY, 0);
                }
                else if (!shrinkPressed && transform.localScale != tama�oNormal)
                {
                    float deltaY = (tama�oNormal.y - tama�oTransformado.y) * 0.5f;
                    transform.localScale = tama�oNormal;
                    transform.position += new Vector3(0, deltaY, 0);
                }
            }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            _comprigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    public void SetShrink(bool shrink)
    {
        shrinkPressed = shrink;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == ("Ground"))
        {
            isGrounded=true;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle") && gameManager != null)
        {
            gameManager.GameOver(); 
        }
    }
}
