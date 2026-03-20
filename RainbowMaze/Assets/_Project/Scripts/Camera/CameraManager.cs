using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float shakeIntensity;
    [SerializeField] private float shakeTime;
    private bool isShaking = false;

    [SerializeField] private Material screenMaterial;

    //Efeito de choque elétrico, que faz a câmera tremer e a tela piscar em cores invertidas
    public IEnumerator ElectricSchockEffect()
    {
        if (!isShaking)
        {
            Vector3 startPosition = transform.position;
            isShaking = true;

            for (int i = 0; i < 3; i++)
            {
                transform.position = new Vector3(startPosition.x + Random.Range(-shakeIntensity, shakeIntensity),
                                                 transform.position.y,
                                                 transform.position.z);
                screenMaterial.SetInt("_Active", screenMaterial.GetInt("_Active") == 0 ? 1 : 0);

                yield return new WaitForSeconds(shakeTime);
            }

            screenMaterial.SetInt("_Active", 0);
            transform.position = startPosition;
            isShaking = false;
        }
    }
}
