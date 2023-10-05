using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class IndicatorsManager : UIElement
{
	public static IndicatorsManager Instance;

	public ArrowTarget _markerPrefabs; 

	public List<EnemyZone> _targets = new List<EnemyZone>();
	public List<ArrowTarget> _markers = new List<ArrowTarget>();

	public Sprite arrowSprite; // ������ ����� ���� �� ��������� ������

	private Camera _camera;
	private Vector3 newPos;
	private float upDown;

	void Awake()
	{
		Instance = this;
	}

    private void Start()
    {
        _camera = Camera.main;
    }

    bool Behind(Vector3 point) // ��������� �� ��������� ����� ������ ���
	{
		bool result = false;
		Vector3 forward = _camera.transform.TransformDirection(Vector3.forward);
		Vector3 toOther = point - _camera.transform.position;
		if (Vector3.Dot(forward, toOther) < 0) result = true;
		return result;
	}

	void LateUpdate()
	{
        if(_camera == null)
        {
            return;
        }
		Rect rect = new Rect(0, 0, Screen.width, Screen.height);

		for (int i = 0; i < _targets.Count; i++)
        {
			if(_targets[i] && _markers[i] && _markers[i].IsActive)
            {
                _markers[i].Image.sprite = arrowSprite;
                Vector3 position = _camera.WorldToScreenPoint(_targets[i].transform.position);
                newPos = position;
                upDown = 1;

                if (!Behind(_targets[i].transform.position))
                {
                    if (!rect.Contains(position)) // ���� ���� � ���� ������
                    {
                        if (!_markers[i].IsHasPlayer)
                        {
                            _markers[i].gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        _markers[i].gameObject.SetActive(false);
                    }
                }
                else // ���� ���� ������
                {
                    position = -position;

                    if (_camera.transform.position.y > _targets[i].transform.position.y)
                    {
                        newPos = new Vector3(position.x, 0, 0); // ���� ���� ���� ������, ���������� ������ �����
                    }
                    else
                    {
                        // ���� ���� ���� ������, ���������� ������ ������
                        // � ����������� ���� ��������
                        upDown = -1;
                        newPos = new Vector3(position.x, Screen.height, 0);
                    }
                }

                // ���������� ������ � �������� ������ � ������� �������� �� �������
                float size = _markers[i].Marker.sizeDelta.x / 2;
                newPos.x = Mathf.Clamp(newPos.x, size, Screen.width - size);
                newPos.y = Mathf.Clamp(newPos.y, size, Screen.height - size);

                // ������� ���� �������� � ����
                Vector3 pos = position - newPos;
                float angle = Mathf.Atan2(pos.x, pos.y) * Mathf.Rad2Deg;
                _markers[i].Marker.rotation = Quaternion.AngleAxis(angle * upDown, Vector3.back);

                _markers[i].Marker.anchoredPosition = newPos;

            }
			
		}
		
	}

	public void AddTarget(EnemyZone zone)
    {
		_targets.Add(zone);
		ArrowTarget marker = Instantiate(_markerPrefabs, transform);
		_markers.Add(marker);
        zone.SetMarker(marker);
        marker.gameObject.SetActive(false);
        if (zone.IsInAttack)
        {
            marker.IsActive = true;
        }
	}

	public void DestroyMarkers()
    {
		_markers.Clear();
		_targets.Clear();
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}
}