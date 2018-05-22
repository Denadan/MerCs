using UnityEngine;


namespace Mercs.Tactical
{

    public class CameraMove : MonoBehaviour
    {
        [SerializeField]
        private float CameraSpeed = 10f;


        Camera m_camera;

        // Use this for initialization
        private void Start()
        {
            m_camera = GetComponent<Camera>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                m_camera.orthographicSize *= 0.92f;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                m_camera.orthographicSize *= 1.1f;

            var speed = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f) * CameraSpeed * Time.deltaTime;
            m_camera.transform.position += speed;
        }
    }
}