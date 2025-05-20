using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	[Signal]
	public delegate void ResumeRequestedEventHandler();

	[Signal]
	public delegate void QuitRequestedEventHandler();

	private Button resumeButton;
	private Button quitButton;

	public override void _Ready()
	{
		resumeButton = GetNode<Button>("Container/VBoxContainer/Resume");
		quitButton = GetNode<Button>("Container/VBoxContainer/Quit");

		resumeButton.Pressed += EmitResume;
		quitButton.Pressed += EmitQuit;

		Visible = false;
		// Input.PassThrough = true; // Uncomment if needed
	}

	public void ShowMenu()
	{
		Visible = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
		resumeButton.GrabFocus();
	}

	public void HideMenu()
	{
		Visible = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	private void EmitResume()
	{
		EmitSignal(SignalName.ResumeRequested);
	}

	private void EmitQuit()
	{
		EmitSignal(SignalName.QuitRequested);
	}
}
