using Godot;

namespace FlappyBirdLib.Data {
    public static class Resources {
        public const string MAIN_SCENE = "res://Scenes/Main.tscn";
        public readonly static PackedScene PIPES_SCENE = ResourceLoader.Load<PackedScene>("res://Scenes/Components/Pipes.tscn");
    }
}