using Godot;
using System;
using System.Collections.Generic;
using FlappyBirdLib.AutoLoad;

namespace FlappyBirdLib.Components
{
    public partial class PipesManager : Node3D
    {
        private const float CREATE_POSITION = -15f;
        private const float DESTRUCTION_POSITION = 15f;
        private const double WAIT_TIME = 1f;

        private List<Node3D> _pipes = new List<Node3D>();
        private Timer _timer;

        public override void _Ready()
        {
            _timer = new Timer();
            _timer.WaitTime = WAIT_TIME;
            _timer.Autostart = true;
            _timer.OneShot = false;
            _timer.Paused = true;

            AddChild(_timer);

            InstantiatePipe();

            GameManager.Instance(this).OnPlay(() => _timer.Paused = false);
            GameManager.Instance(this).OnPause(() => _timer.Paused = true);
        }

        private async void InstantiatePipe()
        {
            var pipe = Data.Resources.PIPES_SCENE.Instantiate<Node3D>();
            AddChild(pipe);
            var random = new Random();
            float y = (float)random.Next(4, 9);
            pipe.Position = new Vector3(0, y, CREATE_POSITION);
            _pipes.Add(pipe);
            await ToSignal(_timer, "timeout");
            InstantiatePipe();
        }

        public override void _Process(float delta)
        {
            foreach (var pipe in _pipes)
            {
                if (pipe == null) continue;
                var z = pipe.Position.z + GameManager.Instance(this).Speed;
                pipe.Position = new Vector3(0, pipe.Position.y, z);
            
                if (pipe.Position.z >= DESTRUCTION_POSITION) {
                    pipe.QueueFree();
                }
            }
        }
    }
}
