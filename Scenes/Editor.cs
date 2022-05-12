using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Godot.Collections;
using TS2D.Util;
using Array = Godot.Collections.Array;
using File = System.IO.File;

public class Editor : Node2D
{
	private Control _control;
	private CanvasLayer _canvasLayer;
	private TabContainer _createMenu;
	private GridContainer _tilesContainer;
	private GridContainer _entitiesContainer;
	private Camera2D _camera;
	private float _cameraSpeed = 200;
	private float _cameraZoom = 1;
	private List<TileTextureButton> _tiles = new List<TileTextureButton>();
	private List<TileTextureButton> _entities = new List<TileTextureButton>();
	private Panel _bottomToolbox;
	private Button _zoomInButton;
	private Button _zoomOutButton;
	private Button _saveButton;
	private Button _loadButton;
	private Button _newButton;
	private Button _testButton;
	private Button _exportButton;
	private Panel _loadPanel;
	private LineEdit _loadPanelLineEdit;
	private Button _loadPanelButton;
	private ItemList _loadPanelList;
	private FileDialog _loadDialog;
	private Panel _saveNewPanel;
	private LineEdit _saveNewName;
	private LineEdit _saveNewAuthor;
	private LineEdit _saveNewVersion;
	private LineEdit _saveNewDescription;
	private Button _saveNewButton;
	private int _currentTile = 1;
	private int _currentEntity = 0;
	private bool _testing = false;

	private Map _currentMap;
	private int[,] _tileMap { get; set; } = new int[30, 30];
	private int[,] _entityMap { get ;set;} = new int[30, 30];
	
	private ImageTexture _tile_1 = new ImageTexture();
	private ImageTexture _tile_2 = new ImageTexture();
	private ImageTexture _tile_3 = new ImageTexture();
	private ImageTexture _tile_4 = new ImageTexture();
	private ImageTexture _tile_5 = new ImageTexture();
	private ImageTexture _tile_6 = new ImageTexture();
	private ImageTexture _tile_7 = new ImageTexture();
	private ImageTexture _tile_8 = new ImageTexture();
	private ImageTexture _tile_9 = new ImageTexture();

	private bool hoveringUI;

	public Editor(Map map = null)
	{
		_currentMap = map;
	}

	public override void _Ready()
	{
		_canvasLayer = GetNode<CanvasLayer>("CanvasLayer");
		_control = GetNode<Control>("CanvasLayer/Control");
		_createMenu = GetNode<TabContainer>("CanvasLayer/Control/CreateMenu");
		_tilesContainer = GetNode<GridContainer>("CanvasLayer/Control/CreateMenu/Tiles");
		_entitiesContainer = GetNode<GridContainer>("CanvasLayer/Control/CreateMenu/Entities");
		_bottomToolbox = GetNode<Panel>("CanvasLayer/Control/BottomToolbox");
		_zoomInButton = GetNode<Button>("CanvasLayer/Control/BottomToolbox/ZoomIn");
		_zoomOutButton = GetNode<Button>("CanvasLayer/Control/BottomToolbox/ZoomOut");
		_saveButton = GetNode<Button>("CanvasLayer/Control/BottomToolbox/SaveButton");
		_loadButton = GetNode<Button>("CanvasLayer/Control/BottomToolbox/LoadButton");
		_newButton = GetNode<Button>("CanvasLayer/Control/BottomToolbox/NewButton");
		_testButton = GetNode<Button>("CanvasLayer/Control/BottomToolbox/TestButton");
		_exportButton = GetNode<Button>("CanvasLayer/Control/BottomToolbox/ExportButton");
		_loadPanel = GetNode<Panel>("CanvasLayer/Control/LoadPanel");
		_loadPanelLineEdit = GetNode<LineEdit>("CanvasLayer/Control/LoadPanel/Input");
		_loadPanelButton = GetNode<Button>("CanvasLayer/Control/LoadPanel/Load");
		_loadPanelList = GetNode<ItemList>("CanvasLayer/Control/LoadPanel/List");
		_loadDialog = GetNode<FileDialog>("CanvasLayer/Control/LoadFileDialog");
		_saveNewPanel = GetNode<Panel>("CanvasLayer/Control/SaveNewMap");
		_saveNewName = GetNode<LineEdit>("CanvasLayer/Control/SaveNewMap/Container/Name");
		_saveNewAuthor = GetNode<LineEdit>("CanvasLayer/Control/SaveNewMap/Container/Author");
		_saveNewVersion = GetNode<LineEdit>("CanvasLayer/Control/SaveNewMap/Container/Version");
		_saveNewDescription = GetNode<LineEdit>("CanvasLayer/Control/SaveNewMap/Container/Description");
		_saveNewButton = GetNode<Button>("CanvasLayer/Control/SaveNewMap/Container/Save");
		
		_camera = GetNode<Camera2D>("Camera2D");
		
		_zoomInButton.Connect("pressed", this, nameof(OnZoomIn));
		_zoomInButton.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
		_zoomInButton.Connect("mouse_exited", this, nameof(OnUIMouseExit));
		
		_zoomOutButton.Connect("pressed", this, nameof(OnZoomOut));
		_zoomOutButton.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
		_zoomOutButton.Connect("mouse_exited", this, nameof(OnUIMouseExit));
		
		_saveButton.Connect("pressed", this, nameof(OnSave));
		_saveButton.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
		_saveButton.Connect("mouse_exited", this, nameof(OnUIMouseExit));
		
		_loadButton.Connect("pressed", this, nameof(OnLoad));
		_loadButton.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
		_loadButton.Connect("mouse_exited", this, nameof(OnUIMouseExit));
		
		_newButton.Connect("pressed", this, nameof(OnNew));
		_newButton.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
		_newButton.Connect("mouse_exited", this, nameof(OnUIMouseExit));
		
		_testButton.Connect("pressed", this, nameof(OnTest));
		_testButton.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
		_testButton.Connect("mouse_exited", this, nameof(OnUIMouseExit));
		
		_exportButton.Connect("pressed", this, nameof(OnExport));
		_exportButton.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
		_exportButton.Connect("mouse_exited", this, nameof(OnUIMouseExit));
		
		_loadDialog.Connect("file_selected", this, nameof(OnFileSelected));
		_loadDialog.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
		_loadDialog.Connect("mouse_exited", this, nameof(OnUIMouseExit));
		
		_createMenu.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
		_createMenu.Connect("mouse_exited", this, nameof(OnUIMouseExit));
		
		_saveNewButton.Connect("pressed", this, nameof(OnSaveNewMap));
		//_bottomToolbox.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
		//_bottomToolbox.Connect("mouse_exited", this, nameof(OnUIMouseExit));
		
		_camera.Zoom = new Vector2(_cameraZoom, _cameraZoom);
		_camera.Position = new Vector2(280, 270);
		
		_tile_1.Load("./Tile/tile1.png");
		_tile_2.Load("./Tile/tile2.png");
		_tile_3.Load("./Tile/tile3.png");
		_tile_4.Load("./Tile/tile4.png");
		_tile_5.Load("./Tile/tile5.png");
		_tile_6.Load("./Tile/tile6.png");
		_tile_7.Load("./Tile/tile7.png");
		_tile_8.Load("./Tile/tile8.png");
		_tile_9.Load("./Tile/tile9.png");
		
		_loadPanel.Visible = false;
		_saveNewPanel.Visible = false;
		
		foreach (string tile 
				 in System.IO.Directory.GetFiles("./Tile")
					 .AsEnumerable().Where(s => ".png".Equals(s.Substring(s.Length - 4))))
		{
			Console.WriteLine(tile);
			var button = new TileTextureButton();
			var image =  new ImageTexture();
			image.Load(tile);
			
			
			button.Expand = true;
			button.StretchMode = TextureButton.StretchModeEnum.Scale;
			button.TextureNormal = image;
			button.RectMinSize = new Vector2(48, 48);
			// get the number of the tile after tile
			var resultString = Regex.Match(tile, @"\d+").Value;
			button.TileIndex = Int32.Parse(resultString);
			_tiles.Add(button);
		}
		
		for (int i = 0; i < 30; i++)
		{
			var button = new TileTextureButton();
			button.RectMinSize = new Vector2(48, 48);
			_entities.Add(button);
		}
		
		foreach (var button in _tiles)
		{
			_tilesContainer.AddChild(button);
		}
		
		foreach (var button in _entities)
		{
			_entitiesContainer.AddChild(button);
		}

		
		foreach (var button in _tiles)
		{
			button.Connect("mouse_entered", this, nameof(OnUIMouseEnter));
			button.Connect("mouse_exited", this, nameof(OnUIMouseExit));
			button.Connect("pressed", this, nameof(OnTileButtonPressed), new Array(button));
		}
		
		OS.SetWindowTitle("TS2D - " + "New*");
	}
	
	void OnTileButtonPressed(TileTextureButton button)
	{
		Console.WriteLine(button.TileIndex);
		_currentTile = button.TileIndex;
	}
	

	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("arrow_up"))
		{
			_camera.Position += new Vector2(0, -_cameraSpeed * delta);
		}
		if (Input.IsActionPressed("arrow_down"))
		{
			_camera.Position += new Vector2(0, _cameraSpeed * delta);
		}
		if (Input.IsActionPressed("arrow_left"))
		{
			_camera.Position += new Vector2(-_cameraSpeed * delta, 0);
		}
		if (Input.IsActionPressed("arrow_right"))
		{
			_camera.Position += new Vector2(_cameraSpeed * delta, 0);
		}
		
		if (Input.IsActionJustPressed("editor_save")) OnSave();
		
		if (Input.IsActionPressed("shift")) _cameraSpeed = 325; else _cameraSpeed = 200;

		if (Input.IsActionJustPressed("ui_cancel"))
		{
			_loadPanel.Visible = false;
			_saveNewPanel.Visible = false;
		}
		
		if (Input.IsActionJustPressed("mouse_left_down") && !hoveringUI)
		{
			var mousePos = GetGlobalMousePosition();
			Console.WriteLine(GetLocalMousePosition());
			if (mousePos.x > 0 && mousePos.x < 1440 && mousePos.y > 0 && mousePos.y < 1440)
			{
				var asd = MathHelper.snap(mousePos, 48);
				Console.WriteLine("Rounded:	" + asd);
				var jsonLoc = new Vector2(asd.x / 48, asd.y / 48);
				Console.WriteLine("JSON: " + jsonLoc);
				_tileMap[(int) jsonLoc.x, (int) jsonLoc.y] = _currentTile;
				Update();
			}
		}
		
		if (Input.IsActionJustPressed("mouse_right_down") && !hoveringUI)
		{
			var mousePos = GetGlobalMousePosition();
			Console.WriteLine(GetLocalMousePosition());
			if (mousePos.x > 0 && mousePos.x < 1440 && mousePos.y > 0 && mousePos.y < 1440)
			{
				var asd = MathHelper.snap(mousePos, 48);
				Console.WriteLine("Rounded:	" + asd);
				var jsonLoc = new Vector2(asd.x / 48, asd.y / 48);
				Console.WriteLine("JSON: " + jsonLoc);
				_tileMap[(int) jsonLoc.x, (int) jsonLoc.y] = 0;
				Update();
			}
		}

		if (File.Exists($"./Maps/{_saveNewName.Text}.json"))
		{
			_saveNewButton.Disabled = true;
		}
		else
		{
			_saveNewButton.Disabled = false;
		}
		
		
		
		base._Process(delta);
	}

	public override void _Draw()
	{

		if (!_testing)
		{
			for (int x = 0; x <= 29; x++)
			{
				for (int y = 0; y <= 29; y++)
				{
					var tile = _tileMap[x, y];
					switch (tile)
					{
						case 0:
							/*
							DrawTextureRect(_tile_2, new Rect2(x * 48, y * 48, 48, 48), 
								false,
								null,
								false,
								null
							);  */
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

			for (int i = 0; i <= 1440; i = i + 48)
			{
				DrawLine(
					new Vector2(0, i),
					new Vector2(1440, i),
					new Color(255, 255, 255)
				);
				DrawLine(
					new Vector2(i, 0),
					new Vector2(i, 1440),
					new Color(255, 255, 255)
				);
			}

		}

		base._Draw();
	}
	
	private void OnZoomIn()
	{
		Console.WriteLine("Zooming in");
		var cameraZoom = _camera.Zoom;
		cameraZoom.x -= 0.1f;
		cameraZoom.y -= 0.1f;
		_camera.Zoom = cameraZoom;
		Console.WriteLine($"Zoom: {_camera.Zoom}");
	}
	
	private void OnZoomOut()
	{
		Console.WriteLine("Zooming out");
		var cameraZoom = _camera.Zoom;
		cameraZoom.x += 0.1f;
		cameraZoom.y += 0.1f;
		_camera.Zoom = cameraZoom;
		Console.WriteLine($"Zoom: {_camera.Zoom}");
	}
	
	private void OnSave()
	{
		Console.WriteLine("Saving");
		if (_currentMap == null)
		{
			_saveNewPanel.Visible = true;
		}
		else
		{
			_currentMap.Save(_tileMap, _entityMap);
		}
	}
	
	private void OnLoad()
	{
		Console.WriteLine("Loading");
		
		//_loadPanel.Show();
		_loadDialog.PopupCentered();
	}

	private void OnFileSelected(string file)
	{
		Console.WriteLine("File selected: " + file);
		Console.WriteLine("Also: " + file.Split("/").Last());
		Console.WriteLine("Also: " + file.Split("/").Last().Remove(file.Split("/").Last().Length - 4));
		_currentMap = new Map(file.Split("/").Last().Remove(file.Split("/").Last().Length - 5));
		_tileMap = _currentMap.TileMap;
		_entityMap = _currentMap.EntityMap;
		OS.SetWindowTitle("TS2D - " + _currentMap.Name);
		Update();
	}
	
	private void OnNew()
	{
		Console.WriteLine("Creating new map");
		_currentMap = null;
		_tileMap = new int[30, 30];
		_entityMap = new int[30, 30];
		Update();
		OS.SetWindowTitle("TS2D - " + "New*");
	}
	
	private void OnTest()
	{
		Console.WriteLine("Testing");
		Game game = new Game(_currentMap);
		RemoveChild(_camera);
		RemoveChild(_canvasLayer);
		_camera.CallDeferred("free");
		_canvasLayer.CallDeferred("free");
		AddChild(game);
		_testing = true;
		Update();
	}

	private void OnExport()
	{
		Console.WriteLine("Exporting");
	}

	private void OnUIMouseEnter()
	{
		Console.WriteLine("Mouse entered UI");
		hoveringUI = true;
	}

	private void OnUIMouseExit()
	{
		Console.WriteLine("Mouse exited UI");
		hoveringUI = false;
	}

	private void OnSaveNewMap()
	{
		_currentMap = new Map(_saveNewName.Text);
		_currentMap.Author = _saveNewAuthor.Text;
		_currentMap.Description = _saveNewDescription.Text;
		_currentMap.Version = _saveNewVersion.Text;
		_currentMap.Save(_tileMap, _entityMap);
		_saveNewPanel.Visible = false;
		OS.SetWindowTitle("TS2D - " + _currentMap.Name);
	}
	
}



