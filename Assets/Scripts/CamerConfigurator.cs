using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CamerConfigurator : MonoBehaviour
{
    // define variables
    private Camera mainCamera;          
    
    public GameObject cameraFrustum;
    public GameObject perspectiveSettings;
    public GameObject orthographicSettings;

    public Toggle projectionToggle;
    
    public Slider fovSlider;
    public Slider sizeSlider;
    public Slider nearClipSlider;
    public Slider farClipSlider;

    private bool isOrthographic = false;

    // Start is called before the first frame update
    void Start()
    {
        // instancia la cámara a mainCamara
        mainCamera = GetComponent <Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("No se encontró ningún componente de cámara en GameObject con el script CameraConfigurator.");
            enabled = false; // Desactiva el script para evitar futuros errores.
            return;
        }
        // método para actualizar la configuración de la cámara
        UpdateUIFromCameraSettings();

    }

    // Update is called once per frame
    void Update()
    {
        /*
         si el GameObject cameraFrustum no es nulo =>
            escala el objeto cameraFrustum con un Vector3:
            en x = 
                    si la cámara está en modo ortográfico (orthographic) => el tamaño ortográfico *2 y el aspect ratio de la cámara.
                    si la cámara está en modo de proyección perspectiva => calcula el ancho basado en el campo de visión y la distancia al plano de recorte cercano.
            en y =
                    si la cámara está en modo ortográfico => el tamaño ortográfico *2 (la altura es igual al tamaño).
                    si la cámara está en modo de proyección perspectiva => la altura basada en el campo de visión y la distancia al plano de recorte cercano.
            en z =
                    calcula restando el plano de recorte lejano del plano de recorte cercano de la cámara.
         */
        if (cameraFrustum != null)
        {
            cameraFrustum.transform.localScale = new Vector3(
                mainCamera.orthographic ? mainCamera.orthographicSize * 2 * mainCamera.aspect : 2 * Mathf.Tan(mainCamera.fieldOfView * 0.5f) * mainCamera.nearClipPlane,
                mainCamera.orthographic ? mainCamera.orthographicSize * 2 : 2 * Mathf.Tan(mainCamera.fieldOfView * 0.5f) * mainCamera.nearClipPlane,
                mainCamera.farClipPlane - mainCamera.nearClipPlane
            );
        }
    }

    // actualiza la configuración de la cámara
    private void UpdateUIFromCameraSettings()
    {
        // asigna al booleano isOrthographic ewl valor de la proyección de la cámara com oortográfica => 
        isOrthographic = mainCamera.orthographic;

        // activa el projectionToggle según el valor de isOrthographic
        projectionToggle.isOn = isOrthographic;

        /* 
        si la cámara está en modo ortográfico =>
            el GameObject perspectiveSettings se detactiva
            el GameObject orthographicSettings se activa
            el tamaño del sizeSlider se ajusta al valor que tiene la cámara de orthographicSize
        sino =>
            el GameObject perspectiveSettings se activa
            el GameObject orthographicSettings se desactiva
            el tamaño del fovSlider se ajusta al valor que tiene la cámara de fieldOfView
        */
        if (isOrthographic)
        {
            perspectiveSettings.SetActive(false);
            orthographicSettings.SetActive(true);
            sizeSlider.value = mainCamera.orthographicSize;
        }
        else
        {
            perspectiveSettings.SetActive(true);
            orthographicSettings.SetActive(false);
            fovSlider.value = mainCamera.fieldOfView;
        }

        // el Slider nearClipSlider = al valor nearClipPlane y el Slider farClipSlider = al valor farClipPlane
        nearClipSlider.value = mainCamera.nearClipPlane;
        farClipSlider.value = mainCamera.farClipPlane;
    }

    // ajusta el componente Projection de la mámara
    public void ToggleProjection()
    {
        // niega el valor actual de isOrthographic
        // componente orthographic de la cámara = isOrthographic
        // llama a UpdateUIFromCameraSettings() para actualizar la configuración de la cámara
        isOrthographic = !isOrthographic;
        mainCamera.orthographic = isOrthographic;
        UpdateUIFromCameraSettings();
    }

    // modifica el componente fieldOfView de la cámara
    public void SetFOV(float value)
    {
        mainCamera.fieldOfView = value;
    }

    // modifica el componente orthographicSize de la cámara
    public void SetSize(float value)
    {
        mainCamera.orthographicSize = value;
    }

    // modifica el componente nearClipPlane de la cámara
    public void SetNearClipPlane(float value)
    {
        mainCamera.nearClipPlane = value;
    }

    // modifica el componente farClipPlane de la cámara
    public void SetFarClipPlane(float value)
    {
        mainCamera.farClipPlane = value;
    }

}
