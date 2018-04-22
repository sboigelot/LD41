using UnityEngine;

namespace PyralisStudio.TheCorp.Engine.Camera
{
    public class SimpleRtsCam : MonoBehaviour
    {
        public float ScrollSpeed = 15;

        public float ScrollEdge = 0.1f;

        public float PanSpeed = 10;

        public Vector2 zoomRange = new Vector2(-10, 100);

        public float CurrentZoom = 0;

        public float ZoomZpeed = 1;

        public float ZoomRotation = 1;

        public Vector2 zoomAngleRange = new Vector2(20, 70);

        public float rotateSpeed = 10;

        private Vector3 initialPosition;

        private Vector3 initialRotation;

        public Bounds boundingBox = new Bounds(new Vector3(0f, 0f, 0f), new Vector3(500f, 500f, 500f));

        public string horizontalAxis = "Horizontal";
        public string verticalAxis = "Vertical";

        public void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.eulerAngles;
        }

        public void Update()
        {
            // panning     
            if (Input.GetMouseButton(1))
            {
                transform.Translate(
                    Vector3.right * Time.deltaTime * PanSpeed * (Input.mousePosition.x - Screen.width * 0.5f) /
                    (Screen.width * 0.5f),
                    Space.World);
                transform.Translate(
                    Vector3.forward * Time.deltaTime * PanSpeed * (Input.mousePosition.y - Screen.height * 0.5f) /
                    (Screen.height * 0.5f),
                    Space.World);
            }

            var scrollEdge = ScrollEdge;
#if UNITY_EDITOR
            scrollEdge = 0;
#endif

            var haxisValue = Input.GetAxis(horizontalAxis);
            if (haxisValue > 0 || scrollEdge != 0 && Input.mousePosition.x >= Screen.width * (1 - scrollEdge))
            {
                transform.Translate(Vector3.right * Time.deltaTime * PanSpeed, Space.Self);
            }
            else if (haxisValue < 0 || scrollEdge != 0 && Input.mousePosition.x <= Screen.width * scrollEdge)
            {
                transform.Translate(Vector3.right * Time.deltaTime * -PanSpeed, Space.Self);
            }

            var vaxisValue = Input.GetAxis(verticalAxis);
            if (vaxisValue > 0 || scrollEdge != 0 && Input.mousePosition.y >= Screen.height * (1 - scrollEdge))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * PanSpeed, Space.Self);
            }
            else if (vaxisValue < 0 || scrollEdge != 0 && Input.mousePosition.y <= Screen.height * scrollEdge)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * -PanSpeed, Space.Self);
            }

            if (Input.GetMouseButton(2))
            {
                var left = Input.mousePosition.x < Screen.width / 2;
                transform.Rotate(Vector3.up * Time.deltaTime * (left ? -rotateSpeed : rotateSpeed), Space.World);
            }
            if (Input.GetKey("q"))
            {
                transform.Rotate(Vector3.up * Time.deltaTime * -rotateSpeed, Space.World);
            }
            else if (Input.GetKey("e"))
            {
                transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed, Space.World);
            }

            // zoom in/out
            CurrentZoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 1000 * ZoomZpeed;

            CurrentZoom = Mathf.Clamp(CurrentZoom, zoomRange.x, zoomRange.y);

            var newPos = new Vector3(
                transform.position.x,
                transform.position.y - (transform.position.y - (initialPosition.y + CurrentZoom)) * 0.1f,
                transform.position.z);

            transform.position = newPos;

            float x = transform.eulerAngles.x -
                      (transform.eulerAngles.x - (initialRotation.x + CurrentZoom * ZoomRotation)) * 0.1f;
            x = Mathf.Clamp(x, zoomAngleRange.x, zoomAngleRange.y);

            transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);

            transform.position = boundingBox.ClosestPoint(transform.position);
        }
    }
}