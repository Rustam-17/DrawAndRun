using UnityEngine;
using UnityEngine.EventSystems;

public class LineDrawer : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private RectTransform _drawingArea;
    [SerializeField] private float _pointsStep;

    private Camera _camera;
    private Vector2 _currentLocalPoint;
    private Vector2 _lastLocalPoint;
    private Vector3 _worldPoint;

    private void Start()
    {
        _camera = Camera.main;

        ResetLine();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ResetLine();
        AddPoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        AddPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetLine();
    }

    private void ResetLine()
    {
        _line.positionCount = 0;
    }

    private void AddPoint(Vector2 point)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(_drawingArea, point, _camera))
        {
            if ((Vector2.Distance(point, _lastLocalPoint) > _pointsStep))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_drawingArea, point, _camera, out _currentLocalPoint);

                _worldPoint = _drawingArea.transform.TransformPoint(_currentLocalPoint);
                _worldPoint.z--;

                _line.positionCount++;
                _line.SetPosition(_line.positionCount - 1, _worldPoint);

                _lastLocalPoint = point;
            }            
        }
    }
}
