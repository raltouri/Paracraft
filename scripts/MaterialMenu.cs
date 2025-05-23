using Godot;
using System;
using System.Collections.Generic;

public partial class MaterialMenu : CanvasLayer
{
	[Export]
	public Godot.Collections.Array<string> TileNames = new();

	private int selectedIndex = 0;
	private List<TextureButton> buttons = new();

	private HBoxContainer hbox;
	private Label label;
	private Control _control;

	public override void _Ready()
	{
		hbox = GetNode<HBoxContainer>("Control/VBoxContainer/HBoxContainer");
		label = GetNode<Label>("Control/VBoxContainer/Label");

		buttons.Clear();

		foreach (var child in hbox.GetChildren())
		{
			if (child is TextureButton btn)
			{
				buttons.Add(btn);
			}
		}

		for (int i = 0; i < buttons.Count; i++)
		{
			int index = i; // capture index for closure
			buttons[i].Pressed += () => OnTileSelected(index);
		}

		UpdateSelectionVisual();
		
	}
	

	public int GetSelectedTileId()
	{
		return selectedIndex;
	}

	private void OnTileSelected(int index)
	{
		selectedIndex = index;
		UpdateSelectionVisual();
		label.Text = TileNames.Count > index ? TileNames[index] : "";
	}

	public void CycleSelectionForward()
	{
		if (buttons.Count == 0)
		{
			GD.Print("⚠ No buttons to cycle through.");
			return;
		}

		selectedIndex = (selectedIndex + 1) % buttons.Count;
		UpdateSelectionVisual();
		label.Text = TileNames.Count > selectedIndex ? TileNames[selectedIndex] : "";
	}

	private void UpdateSelectionVisual()
	{
		for (int i = 0; i < buttons.Count; i++)
		{
			buttons[i].Modulate = (i == selectedIndex)
				? new Color(1, 1, 1)
				: new Color(0.5f, 0.5f, 0.5f);
		}
	}
}
