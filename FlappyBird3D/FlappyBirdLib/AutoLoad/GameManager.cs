using Godot;
using System;

namespace FlappyBirdLib.AutoLoad
{
    public partial class GameManager : Node
    {
        private const float SPEED = 0.175f;
        private float _speed = 0;

        private static GameManager _gameManager;

        private event Action _onPlay;
        private event Action _onPause;

        public float Speed
        {
            get
            {
                return _speed;
            }
        }

        public bool IsPaused
        {
            get
            {
                return _speed == 0;
            }
        }

        public override void _Input(InputEvent e)
        {
            if (e is InputEventKey)
            {
                var keyEvent = e as InputEventKey;
                if (keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
                {
                    if (IsPaused)
                    {
                        Play();
                    }
                    else
                    {
                        Pause();
                    }
                }
                if (keyEvent.Pressed && keyEvent.Keycode == Key.Space)
                {
                    if (IsPaused)
                    {
                        Play();
                    }
                }
            }
        }

        public void Play()
        {
            _speed = SPEED;
            _onPlay?.Invoke();
        }

        public void Pause()
        {
            _speed = 0;
            _onPause?.Invoke();
        }

        public static GameManager Instance(Node anyNode)
        {
            if (_gameManager == null)
            {
                _gameManager = anyNode.GetTree().Root.GetNode<GameManager>("GameManager");
            }
            return _gameManager;
        }

        public void OnPlay(Action callback)
        {
            _onPlay += callback;
        }

        public void OnPause(Action callback)
        {
            _onPause += callback;
        }

        public void GameOver()
        {
            _onPlay = null;
            _onPause = null;
            GetTree().ChangeScene(Data.Resources.MAIN_SCENE);
            Pause();
        }
    }
}
