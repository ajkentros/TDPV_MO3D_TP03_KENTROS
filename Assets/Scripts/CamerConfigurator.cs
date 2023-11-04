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
        // instancia la c�mara a mainCamara
        mainCamera = GetComponent <Camera>();
        if (mainCamera == null)
        {
            Debug.LogError("No se encontr� ning�n componente de c�mara en GameObject con el script CameraConfigurator.");
            enabled = false; // Desactiva el script para evitar futuros errores.
            return;
        }
        // m�todo para actualizar la configuraci�n de la c�mara
        UpdateUIFromCameraSettings();

    }

    // Update is called once per frame
    void Update()
    {
        /*
         si el GameObject cameraFrustum no es nulo =>
            escala el objeto cameraFrustum con un Vector3:
            en x = 
                    si la c�mara est� en modo ortogr�fico (orthographic) => el tama�o ortogr�fico *2 y el aspect ratio de la c�mara.
                    si la c�mara est� en modo de proyecci�n perspectiva => calcula el ancho basado en el campo de visi�n y la distancia al plano de recorte cercano.
            en y =
                    si la c�mara est� en modo ortogr�fico => el tama�o ortogr�fico *2 (la altura es igual al tama�o).
                    si la c�mara est� en modo de proyecci�n perspectiva => la altura basada en el campo de visi�n y la distancia al plano de recorte cercano.
            en z =
                    calcula restando el plano de recorte lejano del plano de recorte cercano de la c�mara.
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

    // actualiza la configuraci�n de la c�mara
    private void UpdateUIFromCameraSettings()
    {
        // asigna al booleano isOrthographic ewl valor de la proyecci�n de la c�mara com oortogr�fica => 
        isOrthographic = mainCamera.orthographic;

        // activa el projectionToggle seg�n el valor de isOrthographic
        projectionToggle.isOn = isOrthographic;

        /* 
        si la c�mara est� en modo ortogr�fico =>
            el GameObject perspectiveSettings se detactiva
            el GameObject orthographicSettings se activa
            el tama�o del sizeSlider se ajusta al valor que tiene la c�mara de orthographicSize
        sino =>
            el GameObject perspectiveSettings se activa
            el GameObject orthographicSettings se desactiva
            el tama�o del fovSlider se ajusta al valor que tiene la c�mara de fieldOfView
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

    // ajusta el componente Projection de la m�mara
    public void ToggleProjection()
    {
        // niega el valor actual de isOrthographic
        // componente orthographic de la c�mara = isOrthographic
        // llama a UpdateUIFromCameraSettings() para actualizar la configuraci�n de la c�mara
        isOrthographic = !isOrthographic;
        mainCamera.orthographic = isOrthographic;
        UpdateUIFromCameraSettings();
    }

    // modifica el componente fieldOfView de la c�mara
    public void SetFOV(float value)
    {
        mainCamera.fieldOfView = value;
    }

    // modifica el componente orthographicSize de la c�mara
    public void SetSize(float value)
    {
        mainCamera.orthographicSize = value;
    }

    // modifica el componente nearClipPlane de la c�mara
    public void SetNearClipPlane(float value)
    {
        mainCamera.nearClipPlane = value;
    }

    // modifica el componente farClipPlane de la c�mara
    public void SetFarClipPlane(float value)
    {
        mainCamera.farClipPlane = value;
    }

}
