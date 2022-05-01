using Godot;
using System.Collections.Generic;
using FlappyBirdLib.AutoLoad;

namespace FlappyBirdLib.Components
{
    public partial class TilesManager : Node
    {
        private const float Z_POSITION_LIMIT = 80;
        private const float TILE_WIDTH = 37.524f; // 38.524f;

        private List<Node3D> _tiles = new List<Node3D>();

        public override void _Ready()
        {
            _tiles.Add(GetNode<Node3D>("Tiles1"));
            _tiles.Add(GetNode<Node3D>("Tiles2"));
            _tiles.Add(GetNode<Node3D>("Tiles3"));
        }

        public override void _Process(float delta)
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                var tile = _tiles[i];
                var lastTile = i == 0 ? _tiles[2] : i == 1 ? _tiles[0] : _tiles[1];

                var z = tile.Position.z >= Z_POSITION_LIMIT ?
                lastTile.Position.z - TILE_WIDTH :
                tile.Position.z + GameManager.Instance(this).Speed;
                tile.Position = new Vector3(tile.Position.x, tile.Position.y, z);
            }
        }
    }
}
