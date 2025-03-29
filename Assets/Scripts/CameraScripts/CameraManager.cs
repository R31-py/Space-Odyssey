using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

    [Header("Controls for lerping the Y Damping during player jump/fall")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;
    private Coroutine _lerpYPanCoroutine;
    private float _defaultYPanAmount;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (_allVirtualCameras == null || _allVirtualCameras.Length == 0)
        {
            Debug.LogError("No virtual cameras assigned in CameraManager!");
            return;
        }
        SetCurrentActiveCamera();
    }

    private void SetCurrentActiveCamera()
    {
        foreach (var cam in _allVirtualCameras)
        {
            if (cam.enabled)
            {
                _currentCamera = cam;
                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                _defaultYPanAmount = _framingTransposer.m_YDamping;
                return;
            }
        }

        Debug.LogError("No active Cinemachine camera found!");
    }

    public void SwitchToCamera(CinemachineVirtualCamera newCamera)
    {
        if (newCamera == null)
        {
            Debug.LogError("SwitchToCamera() received a null camera!");
            return;
        }

        foreach (var cam in _allVirtualCameras)
        {
            cam.Priority = 0;
        }
        newCamera.Priority = 10;
        _currentCamera = newCamera;
        _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        if (_currentCamera == null || _framingTransposer == null)
        {
            Debug.LogError("Cannot adjust Y damping: No active camera or framing transposer!");
            return;
        }

        if (_lerpYPanCoroutine != null)
            StopCoroutine(_lerpYPanCoroutine);

        _lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        IsLerpingYDamping = true;
        float startValue = _framingTransposer.m_YDamping;
        float targetValue = isPlayerFalling ? _fallPanAmount : _defaultYPanAmount;
        float elapsedTime = 0f;

        while (elapsedTime < _fallYPanTime)
        {
            _framingTransposer.m_YDamping = Mathf.Lerp(startValue, targetValue, elapsedTime / _fallYPanTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _framingTransposer.m_YDamping = targetValue;
        IsLerpingYDamping = false;
    }
}
