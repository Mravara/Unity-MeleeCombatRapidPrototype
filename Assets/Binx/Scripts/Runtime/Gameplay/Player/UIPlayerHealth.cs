using System;
using UnityEngine;
using UnityEngine.UI;

namespace Binx.UI
{
    public class UIPlayerHealth : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private RectTransform healthLayout;
        [SerializeField] private Image healthImage;
        [SerializeField] private Vector3 healthOffset = new Vector3(0f, 0.1f, -0.5f);
        [SerializeField] private float referenceResolutionHeight = 1080;
        private new Camera camera;

        private Vector3 ScaledHealthOffset => healthOffset * (Screen.height / referenceResolutionHeight);

        private void Awake()
        {
            transform.SetParent(null);
            camera = Camera.main;
        }

        public void LateUpdate()
        {
            UpdateFillAmount();
            UpdatePosition();
        }

        public void SetCamera(Camera camera)
        {
            this.camera = camera;
        }

        private void UpdateFillAmount()
        {
            Player player = Player.instance;
            
            float amount = (float)player.CurrentHealth / player.MaxHealth;
            
            if (Mathf.Abs(healthImage.fillAmount - amount) > 0.01f)
                healthImage.fillAmount = amount;
        }

        private void UpdatePosition()
        {
            if (camera)
                healthLayout.position = camera.WorldToScreenPoint(target.position) + ScaledHealthOffset;
        }
    }
}