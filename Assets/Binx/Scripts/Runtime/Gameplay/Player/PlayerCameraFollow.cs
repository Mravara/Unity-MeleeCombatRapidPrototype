using UnityEngine;

namespace Binx
{
    public class PlayerCameraFollow : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private float followSpeed;

        private new Transform transform;
        private Vector3 offset;

        private void Start()
        {
            transform = base.transform;
            offset = transform.position - Player.instance.Position;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        
        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, Player.instance.Position + offset, followSpeed * Time.deltaTime);
        }
    }
}
