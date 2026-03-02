using System;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Required] private CinemachineCamera _camera;
    [SerializeField, Required] private CinemachineOrbitalFollow _orbitalFollow;
    [SerializeField, Required] private CinemachineRotationComposer _rotationComposer;
    [SerializeField, Required] private CinemachineInputAxisController _cameraInput;
    [SerializeField, Required] private BuildingGenerator _buildingGenerator;
    [SerializeField, Required] private Transform _cameraTarget;
    
    [Header("Config")]
    [SerializeField] private float _recenterSpeed = 15f;
    [SerializeField] private float _moveTargetSpeed = 5f;
    [SerializeField] private float _zoomSpeed = 100f;
    [SerializeField] private FloatRange _zoomRange = new FloatRange(5f, 25f);
    [SerializeField, ReadOnly] private bool _isOutsideBuilding;

    private Vector3 _buildingCenter;

    private void OnEnable()
    {
        _buildingGenerator.OnBuildingGenerated += OnBuildingGenerated;
    }

    private void OnDisable()
    {
        _buildingGenerator.OnBuildingGenerated -= OnBuildingGenerated;
        
        LockCursor(false);
    }

    private void Update()
    {
        bool isHoldingRightClick = Input.GetKey(KeyCode.Mouse1);
        _cameraInput.enabled = isHoldingRightClick;
        
        _orbitalFollow.Radius -= Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime;
        _orbitalFollow.Radius = _zoomRange.Clamp(_orbitalFollow.Radius);

        _isOutsideBuilding = Physics.Raycast(_buildingCenter, -_camera.transform.forward, Vector3.Distance(_buildingCenter, _camera.transform.position));
        bool isHoldingMiddleClick = Input.GetKey(KeyCode.Mouse2);
        if (_isOutsideBuilding)
        {
            _cameraTarget.position =
                Vector3.Lerp(_cameraTarget.position, _buildingCenter, _recenterSpeed * Time.deltaTime);
        }
        else
        {
            if (!isHoldingRightClick && isHoldingMiddleClick)
            {
                float mouseX = Input.GetAxisRaw("Mouse X");
                float mouseY = Input.GetAxisRaw("Mouse Y");
                Vector3 moveDirection = _camera.transform.right * mouseX + _camera.transform.up * mouseY;
                moveDirection.Normalize();
                
                _cameraTarget.position -= moveDirection * (_moveTargetSpeed * Time.deltaTime);
            }
        }
        
        LockCursor(isHoldingRightClick || (!_isOutsideBuilding && isHoldingMiddleClick));
    }

    private void OnBuildingGenerated()
    {
        _buildingCenter = _buildingGenerator.GetBuildingCenterWorldPosition();
    }

    private static void LockCursor(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Confined : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }
}