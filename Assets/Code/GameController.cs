using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    public class GameController : MonoBehaviour
    {
        public MazeGenerator Maze;
        public GameObject PlayerPrefab;
        public GameObject ScaryManPrefab;

		public bool MazeControlledSpawn = true;
		public int SpawnDistFromCenter;
		public int AmountOfScaryMen;
		public Vector3 PlayerSpawnLocation = new Vector3(110, 1, 110);
        public Vector3[] ScaryManSpawnLocation;

        public bool SpawnScaryMenRandomly;
        public int RandomScaryMenAmount;
        private int SpawnedMen = 0;
        public float SpawnTime;
        private float DT = 0.0f;
        private bool GameOn = false;
        private List<GameObject> RandomMen; 

		public MenuCameraBehaviour MenuCamera;
        public PlayerCameraBehaviour PlayCamera;

        public Canvas PlayCanvas;

        public Canvas MasterMenuCanvas;
        private Button _masterMenuPlayButton;
        private Button _masterMenuRebuildMazeButton;
        private Button _masterMenuExitButton;

        public Canvas InGameMenuCanvas;
        private Button _inGameMenuResumeButton;
        private Button _inGameMenuGoToMasterMenuButton;

        public Canvas GameOverCanvas;
        private Text _gameOverDetailsText;
        private Button _gameOverOkayButton;

        private PlayerBehaviour _player;
        private GameObject[] _scaryMen;

        private GameState _currentState;

        public void Awake()
        {
            RandomMen = new List<GameObject>();

            Screen.lockCursor = false;
            Screen.showCursor = true;

            _currentState = GameState.Play;

            _masterMenuPlayButton = MasterMenuCanvas.transform.FindChild("play_button").GetComponent<Button>();
            _masterMenuRebuildMazeButton = MasterMenuCanvas.transform.FindChild("rebuild_maze_button").GetComponent<Button>();
            _masterMenuExitButton = MasterMenuCanvas.transform.FindChild("exit_button").GetComponent<Button>();

            _inGameMenuGoToMasterMenuButton = InGameMenuCanvas.transform.FindChild("go_to_master_menu_button").GetComponent<Button>();
            _inGameMenuResumeButton = InGameMenuCanvas.transform.FindChild("resume_button").GetComponent<Button>();

            _gameOverDetailsText = GameOverCanvas.transform.FindChild("game_over_details_text").GetComponent<Text>();
            _gameOverOkayButton = GameOverCanvas.transform.FindChild("game_over_okay_button").GetComponent<Button>();

            _masterMenuPlayButton.onClick.AddListener(OnPlayButtonClicked);
            _masterMenuRebuildMazeButton.onClick.AddListener(OnRebuildMazeButtonClicked);
            _masterMenuExitButton.onClick.AddListener(OnExitButtonClicked);

            _inGameMenuGoToMasterMenuButton.onClick.AddListener(OnGoToMasterMenuButtonClicked);
            _inGameMenuResumeButton.onClick.AddListener(OnPlayButtonClicked);

            _gameOverOkayButton.onClick.AddListener(OnGoToMasterMenuButtonClicked);

            PlayCanvas.enabled = false;
            InGameMenuCanvas.enabled = false;
            MasterMenuCanvas.enabled = true;
            GameOverCanvas.enabled = false;

			Maze.Build();

			if (MazeControlledSpawn)
			{
				PlayerSpawnLocation = Maze.GetOpenLocationNearCenterAndRemove (SpawnDistFromCenter) + new Vector3(0,1,0);

				ScaryManSpawnLocation = new Vector3[AmountOfScaryMen];

				for(var i = 0; i < AmountOfScaryMen; i++)
					ScaryManSpawnLocation[i] = Maze.GetOpenLocationNearCenterAndRemove (SpawnDistFromCenter) + new Vector3(0,1,0);
			}
			else
			{
				AmountOfScaryMen = ScaryManSpawnLocation.Length;
			}

			_scaryMen = new GameObject[AmountOfScaryMen];
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(_currentState == GameState.Play)
                    OnInGameMenuButtonClicked();
                else if (_currentState == GameState.InGameMenu)
                    OnPlayButtonClicked();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Screen.lockCursor = !Screen.lockCursor;
                Screen.showCursor = !Screen.showCursor;
            }

            if (SpawnScaryMenRandomly && GameOn)
            {
                DT += Time.deltaTime;
                if (DT > SpawnTime)
                {
                    DT = 0.0f;
                    if (RandomScaryMenAmount > SpawnedMen)
                        RandomMen.Add(Instantiate(ScaryManPrefab, Maze.GetRandomOpenPosition(), Quaternion.identity) as GameObject);
                    else
                        SpawnScaryMenRandomly = false;

                    SpawnedMen ++;
                }
            }
            if (_player != null)
            {
                if (_player.IsDead)
                    OnGameOver(false);
                if (_player.IsFree)
                    OnGameOver(true);
            }
        }

        public void OnGameOver(bool didWin)
        {
            Screen.lockCursor = false;
            Screen.showCursor = true;

            PlayCanvas.enabled = false;
            MasterMenuCanvas.enabled = false;
            InGameMenuCanvas.enabled = false;
            GameOverCanvas.enabled = true;

            MenuCamera.gameObject.SetActive(true);
            PlayCamera.gameObject.SetActive(false);

            if(_player != null)
                Destroy(_player.gameObject);

            for (var i = 0; i < AmountOfScaryMen; i++)
                Destroy(_scaryMen[i].gameObject);

            _gameOverDetailsText.text = didWin ? "YOU HAVE ESCAPED THE MAZE" : "SCARY MAN HAS CAUGHT YOU";
            _gameOverOkayButton.GetComponentInChildren<Text>().text = didWin ? "HOORAY!" : "OKAY :(";
        }

        private void OnInGameMenuButtonClicked()
        {
            Screen.lockCursor = false;
            Screen.showCursor = true;

            PlayCanvas.enabled = false;
            MasterMenuCanvas.enabled = false;
            InGameMenuCanvas.enabled = true;
            GameOverCanvas.enabled = false;

            MenuCamera.gameObject.SetActive(false);
            PlayCamera.gameObject.SetActive(true);

            _currentState = GameState.InGameMenu;
        }

        private void OnGoToMasterMenuButtonClicked()
        {
            Screen.lockCursor = false;
            Screen.showCursor = true;

            PlayCanvas.enabled = false;
            MasterMenuCanvas.enabled = true;
            InGameMenuCanvas.enabled = false;
            GameOverCanvas.enabled = false;

            MenuCamera.gameObject.SetActive(true);
            PlayCamera.gameObject.SetActive(false);

            if(_player != null)
            Destroy(_player.gameObject);

			for(var i = 0; i < AmountOfScaryMen; i++)
				Destroy(_scaryMen[i].gameObject);

            for (int i = 0; i < RandomMen.Count; i++)
                Destroy(RandomMen[i]);

            _currentState = GameState.MasterMenu;
        }

        private void OnPlayButtonClicked()
        {
            Screen.lockCursor = true;
            Screen.showCursor = false;

            PlayCanvas.enabled = true;
            MasterMenuCanvas.enabled = false;
            InGameMenuCanvas.enabled = false;
            GameOverCanvas.enabled = false;

            if (_player == null && _scaryMen[0] == null)
            {
                var playerObject = Instantiate(PlayerPrefab, PlayerSpawnLocation, Quaternion.identity) as GameObject;
                _player = playerObject.GetComponent<PlayerBehaviour>();

				for(int i = 0; i < AmountOfScaryMen; i++)
					_scaryMen[i] = Instantiate(ScaryManPrefab, ScaryManSpawnLocation[i], Quaternion.identity) as GameObject;
            }

            MenuCamera.gameObject.SetActive(false);
            PlayCamera.gameObject.SetActive(true);
            PlayCamera.TargetPlayer = _player;
            _player.MainCamera = PlayCamera.camera;

            _currentState = GameState.Play;

            GameOn = true;
            SpawnedMen = 0;
        }

        private void OnRebuildMazeButtonClicked()
        {
	        Maze.TearDown();
			Maze.Build();
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }

    enum GameState { Play, InGameMenu, MasterMenu }
}
