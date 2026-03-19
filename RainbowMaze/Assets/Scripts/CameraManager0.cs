using System.Collections;
using UnityEngine;

public class CameraManager0 : MonoBehaviour
{
    [Header("Camera Settings")]
    private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private float leftLimit, rightLimit, upLimit, bottomLimit;

    [Header("Shake Settings")]
    [SerializeField] private float shakeIntensity;
    
    public bool cameraShake;

    private float x;
    private float y;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        if (target == null)
        {
            Debug.Log("Erro. Player não esta atribuido a câmera.");            
        }
    }

    void Update()
    {
        if (target == null) return;

        CameraFollow();

        if (cameraShake == true)
        {
            StartCoroutine(Shake());
        }
    }

    void CameraFollow()
    {
        // LIMITA O VALOR DE X Y
        x = Mathf.Clamp(target.position.x, leftLimit, rightLimit);
        y = Mathf.Clamp(target.position.y, bottomLimit, upLimit);

        // APLICA O VALOR E MOVIMENTO DA CAMERA
        Vector3 cameraPosition = new Vector3(x, y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, cameraPosition, speed * Time.deltaTime);
    }
    
    IEnumerator Shake()
    {
        cameraShake = false;

        for (int i = 0; i < 3; i++)
        {
            transform.position = new Vector3(x + Random.Range(-shakeIntensity, shakeIntensity), 
                                             y + Random.Range(-shakeIntensity, shakeIntensity), 
                                             transform.position.z);            

            yield return new WaitForSeconds(0.06f);
        }
    }

}
