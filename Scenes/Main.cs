using Godot;
using System;

public class Main : Node2D
{
	private Control _mainControl;
	private Button _playButton;
	private Button _quitButton;
	private Button _optionsButton;
	private Button _editButton;
	
	public override void _Ready()
	{
		_mainControl = GetNode<Control>("MainUI");
		_playButton = GetNode<Button>("MainUI/Container/Play");
		_quitButton = GetNode<Button>("MainUI/Container/Quit");
		_optionsButton = GetNode<Button>("MainUI/Container/Options");
		_editButton = GetNode<Button>("MainUI/Container/Edit");
		
		_playButton.Connect("pressed", this, nameof(_on_Play_pressed));
		_quitButton.Connect("pressed", this, nameof(_on_Quit_pressed));
		_optionsButton.Connect("pressed", this, nameof(_on_Options_pressed));
		_editButton.Connect("pressed", this, nameof(_on_Edit_pressed));
	}

	public void _on_Play_pressed()
	{
		Console.WriteLine("Play pressed");
		_mainControl.Hide();
	}
	
	public void _on_Edit_pressed()
	{
		_mainControl.Hide();
		var scene = GD.Load<PackedScene>("res://Scenes/Editor.tscn");
		var editor = scene.Instance<Editor>();
		AddChild(editor);
		Update();	
	}
	public void _on_Options_pressed()
	{
		_mainControl.Hide();
	}
	
	
	public void _on_Quit_pressed()
	{
		GetTree().Quit();
	}

}
