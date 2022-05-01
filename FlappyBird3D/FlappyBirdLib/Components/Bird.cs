using Godot;
using Godot.Collections;
using FlappyBirdLib.AutoLoad;

namespace FlappyBirdLib.Components
{

    public partial class Bird : RigidDynamicBody3D
    {
        private const string IDLE_ANIM = "idle";
        private const string PLAYING_ANIM = "wings_flapping";
        private const float ROTATE_SPEED = 0.03f;
        private const float INPUT_IMPULSE_FORCE = 8f;
        private const float MAX_RADIAN_LIMIT = 0.75f;
        private readonly static string[] COLLIDERS_NAMES = new string[] {
            "TileBody", "TopPipeBody", "BottomPipeBody"
        };

        private AnimationTree _animationTree;
        private AnimationNodeStateMachinePlayback _stateMachine;
        private Node3D _birdMesh;
        private Vector3 _initialPosition = Vector3.Zero;

        private AnimationNodeStateMachinePlayback StateMachine
        {
            get
            {
                if (_stateMachine == null)
                {
                    _animationTree = GetNode<AnimationTree>("AnimationTree");
                    _stateMachine = (AnimationNodeStateMachinePlayback)_animationTree.Get("parameters/playback");
                }
                return _stateMachine;
            }
        }

        public override void _Ready()
        {
            _birdMesh = GetNode<Node3D>("BirdMeshPivot");
            _initialPosition = Position;

            GameManager.Instance(this).OnPlay(Play);
            GameManager.Instance(this).OnPause(Pause);
            Pause();
        }

        public override void _Input(InputEvent e)
        {
            if (e is InputEventKey)
            {
                var keyEvent = e as InputEventKey;

                if (keyEvent.Pressed && keyEvent.Keycode == Key.Space)
                {
                    ApplyImpulse(new Vector3(0, INPUT_IMPULSE_FORCE, 0));
                }
            }
        }

        public override void _PhysicsProcess(float delta)
        {
            RotateAnimation();
            DetectCollision();
        }

        private void RotateAnimation()
        {
            if (GameManager.Instance(this).IsPaused) return;

            if (_birdMesh.Rotation.x <= MAX_RADIAN_LIMIT && _birdMesh.Rotation.x >= -MAX_RADIAN_LIMIT)
            {
                _birdMesh.RotateX(LinearVelocity.y > 0 ? ROTATE_SPEED : -ROTATE_SPEED);
            }

            else if (_birdMesh.Rotation.x > MAX_RADIAN_LIMIT)
            {
                _birdMesh.Rotation = new Vector3(MAX_RADIAN_LIMIT, 0, 0);
            }

            else if (_birdMesh.Rotation.x < -MAX_RADIAN_LIMIT)
            {
                _birdMesh.Rotation = new Vector3(-MAX_RADIAN_LIMIT, 0, 0);
            }
        }

        private void DetectCollision()
        {
            var bodies = GetCollidingBodies();

            foreach (var body in bodies)
            {
                if (body is Node)
                {
                    foreach (var colliderName in COLLIDERS_NAMES)
                    {
                        if ((body as Node).Name == colliderName)
                        {
                            Pause();
                            GameManager.Instance(this).GameOver();
                        }
                    }

                }
            }
        }

        public void Pause()
        {
            StateMachine.Travel(IDLE_ANIM);
            Freeze = true;
        }

        public void Play()
        {
            StateMachine.Travel(PLAYING_ANIM);
            Freeze = false;
        }
    }
}
