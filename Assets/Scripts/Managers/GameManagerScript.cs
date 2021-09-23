using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Camera;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class GameManagerScript : MonoBehaviour
    {
        [SerializeField] private int numberOfRoundsToWin = 5;
        [SerializeField] private float startDelay = 3f;
        [SerializeField] private float endDelay = 3f;
        [SerializeField] private CameraControl cameraControl;
        [SerializeField] private Text messageText;
        [SerializeField] private GameObject tankPrefab;
        [SerializeField] private TankManager[] tanks;

        private int _roundNumber;
        private WaitForSeconds startWait;
        private WaitForSeconds endWait;
        private TankManager roundWinner;
        private TankManager gameWinner;

        private void Start()
        {
            startWait = new WaitForSeconds(startDelay);
            endWait = new WaitForSeconds(endDelay);

            SpawnAllTanks();
            SetCameraTargets();

            StartCoroutine(GameLoop());
        }

        private void SpawnAllTanks()
        {
            for (int i = 0; i < tanks.Length; i++)
            {
                tanks[i].Instance = Instantiate(tankPrefab, tanks[i].SpawnPoint.position, tanks[i].SpawnPoint.rotation);
                tanks[i].PlayerNumber = i + 1;
                tanks[i].Setup();
            }
        }

        private void SetCameraTargets()
        {
            var targets = new Transform[tanks.Length];
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i] = tanks[i].Instance.transform;
            }
            cameraControl.Targets = targets;
        }

        private IEnumerator GameLoop()
        {
            yield return StartCoroutine(RoundStarting());
            yield return StartCoroutine(RoundPlaying());
            yield return StartCoroutine(RoundEnding());

            if (gameWinner != null)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                StartCoroutine(GameLoop());
            }
        }


        private IEnumerator RoundStarting()
        {
            ResetAllTanks();
            DisableTankControls();
            cameraControl.SetStartPositionAndSize();

            _roundNumber++;
            messageText.text = $"ROUND {_roundNumber}";

            yield return startWait;
        }


        private IEnumerator RoundPlaying()
        {
            EnableTankControls();
            messageText.text = string.Empty;

            while (!IsOneTankLeft())
            { 
                yield return null;
            }
        }


        private IEnumerator RoundEnding()
        {
            DisableTankControls();
            roundWinner = GetRoundWinner();
            if (roundWinner != null)
            {
                roundWinner.Wins++;
            }

            gameWinner = GetGameWinner();
            string message = GetEndMessage();
            messageText.text = message;

            yield return endWait;
        }


        private bool IsOneTankLeft()
        {
            return (tanks.Where(tank => tank.Instance.activeSelf).Take(2).Count() == 1);            
        }

        private TankManager GetRoundWinner()
        {
            return tanks.FirstOrDefault(tank => tank.Instance.activeSelf);
        }


        private TankManager GetGameWinner()
        {
            return tanks.FirstOrDefault(tank => tank.Wins == numberOfRoundsToWin);
        }


        private string GetEndMessage()
        {
            string message = "DRAW!";
            if (roundWinner != null)
            {
                message = roundWinner.ColoredPlayerText + " WINS THE ROUND!";
            }
            message += "\n\n\n\n";

            for (int i = 0; i < tanks.Length; i++)
            {
                message += tanks[i].ColoredPlayerText + ": " + tanks[i].Wins + " WINS\n";
            }

            if (gameWinner != null)
            {
                message = gameWinner.ColoredPlayerText + " WINS THE GAME!";
            }
            return message;
        }

        private void ResetAllTanks()
        {
            tanks.ToList().ForEach(tank => tank.Reset());
        }

        private void EnableTankControls()
        {
            tanks.ToList().ForEach(tank => tank.EnableControls());
        }

        private void DisableTankControls()
        {
            tanks.ToList().ForEach(tank => tank.DisableControls());
        }
    }
}