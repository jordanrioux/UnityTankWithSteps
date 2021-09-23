using System;
using Tank;
using UnityEngine;

namespace Managers
{
    [Serializable]
    public class TankManager
    {
        [SerializeField] private Color playerColor;
        [SerializeField] public GameObject Instance;
        [SerializeField] public Transform SpawnPoint;

        private TankMovement _tankMovement;
        private TankShooting _tankShooting;
        private GameObject _canvasGameObject;
        
        public int PlayerNumber { get; set; }
        public string ColoredPlayerText { get; set; }
        public int Wins { get; set; }        

        public void Setup()
        {
            _tankMovement = Instance.GetComponent<TankMovement>();
            _tankShooting = Instance.GetComponent<TankShooting>();
            _canvasGameObject = Instance.GetComponentInChildren<Canvas>().gameObject;

            _tankMovement.PlayerNumber = PlayerNumber;
            _tankShooting.PlayerNumber = PlayerNumber;

            ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(playerColor) + ">PLAYER " + PlayerNumber + "</color>";

            var renderers = Instance.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material.color = playerColor;
            }
        }

        private void SetControlsState(bool enabled)
        {
            _tankMovement.enabled = enabled;
            _tankShooting.enabled = enabled;
            _canvasGameObject.SetActive(enabled);
        }

        public void DisableControls()
        {
            SetControlsState(false);
        }

        public void EnableControls()
        {
            SetControlsState(true);
        }

        public void Reset()
        {
            Instance.transform.position = SpawnPoint.position;
            Instance.transform.rotation = SpawnPoint.rotation;

            Instance.SetActive(false);
            Instance.SetActive(true);
        }
    }
}