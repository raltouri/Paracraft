using Godot;
using System;

public partial class World : Node3D
{
	private PauseMenu pauseMenu;
	private PackedScene materialMenuScene;
	private MaterialMenu materialMenuInstance;


	public override void _Ready()
	{
		// Instantiate and add pause menu
		var pauseMenuScene = GD.Load<PackedScene>("res://scenes/pause_menu.tscn");
		pauseMenu = (PauseMenu)pauseMenuScene.Instantiate();
		//GD.Print("pauseMenu class type: ", pauseMenu.GetType());
		//GD.Print("pauseMenu signal list: ", string.Join(", ", pauseMenu.GetSignalList()));

		AddChild(pauseMenu);

		// Connect signals
		//pauseMenu.Connect("resume_requested", new Callable(this, nameof(OnResume)));
		//pauseMenu.Connect("quit_requested", new Callable(this, nameof(OnQuit)));
		pauseMenu.Connect(nameof(PauseMenu.ResumeRequested), new Callable(this, nameof(OnResume)));
		pauseMenu.Connect(nameof(PauseMenu.QuitRequested), new Callable(this, nameof(OnQuit)));


		// Instantiate and show material menu
		materialMenuScene = GD.Load<PackedScene>("res://scenes/material_menu.tscn");
		materialMenuInstance = (MaterialMenu)materialMenuScene.Instantiate();

		AddChild(materialMenuInstance);
		materialMenuInstance.Set("visible", true);

		var player = GetNode<Player>("Player");
		if (player != null)
		{
			player.MaterialMenu = (MaterialMenu)materialMenuInstance;
		}

	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("pause"))
		{
			if (GetTree().Paused)
				OnResume();
			else
				PauseGame();
		}
	}

	private void PauseGame()
	{
		GetTree().Paused = true;
		pauseMenu.ShowMenu();
	}

	private void OnResume()
	{
		pauseMenu.HideMenu();
		GetTree().Paused = false;
	}

	private void OnQuit()
	{
		GetTree().Quit();
	}
}
