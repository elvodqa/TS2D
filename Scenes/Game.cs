using Godot;
using System;
using TS2D.Util;

public class Game : Node2D
{
	private ImageTexture _tile_1 = new ImageTexture();
	private ImageTexture _tile_2 = new ImageTexture();
	private ImageTexture _tile_3 = new ImageTexture();
	private ImageTexture _tile_4 = new ImageTexture();
	private ImageTexture _tile_5 = new ImageTexture();
	private ImageTexture _tile_6 = new ImageTexture();
	private ImageTexture _tile_7 = new ImageTexture();
	private ImageTexture _tile_8 = new ImageTexture();
	private ImageTexture _tile_9 = new ImageTexture();
	
	private Map _map;
	
	public Game(Map map)
	{
		_map = map;
	}
	
	public override void _Ready()
	{
		_tile_1.Load("./Tile/tile1.png");
		_tile_2.Load("./Tile/tile2.png");
		_tile_3.Load("./Tile/tile3.png");
		_tile_4.Load("./Tile/tile4.png");
		_tile_5.Load("./Tile/tile5.png");
		_tile_6.Load("./Tile/tile6.png");
		_tile_7.Load("./Tile/tile7.png");
		_tile_8.Load("./Tile/tile8.png");
		_tile_9.Load("./Tile/tile9.png");
	}
	
	public override void _Process(float delta)
	{
		Update();
	}
	
	public override void _Draw()
	{
		for (int x = 0; x <= 29; x++)
		{
			for (int y = 0; y <= 29; y++)
			{
				var tile = _map.TileMap[x, y];
				switch (tile)
				{
					case 0:
						break;
					case 1:
						DrawTextureRect(_tile_1, new Rect2(x * 48, y * 48, 48, 48), false, null, false, null);
						break;
					case 2:
						DrawTextureRect(_tile_2, new Rect2(x * 48, y * 48, 48, 48), false, null, false, null);
						break;
					case 3:
						DrawTextureRect(_tile_3, new Rect2(x * 48, y * 48, 48, 48), false, null, false, null);
						break;
					case 4:
						DrawTextureRect(_tile_4, new Rect2(x * 48, y * 48, 48, 48), false, null, false, null);
						break;
					case 5:
						DrawTextureRect(_tile_5, new Rect2(x * 48, y * 48, 48, 48), false, null, false, null);
						break;
					case 6:
						DrawTextureRect(_tile_6, new Rect2(x * 48, y * 48, 48, 48), false, null, false, null);
						break;
					case 7:
						DrawTextureRect(_tile_7, new Rect2(x * 48, y * 48, 48, 48), false, null, false, null);
						break;
					case 8:
						DrawTextureRect(_tile_8, new Rect2(x * 48, y * 48, 48, 48), false, null, false, null);
						break;
					case 9:
						DrawTextureRect(_tile_9, new Rect2(x * 48, y * 48, 48, 48), false, null, false, null);
						break;
				}
			}
		}
		
	}

}
