﻿using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Shellblade.Graphics
{
	public class Window
	{
		private readonly RenderWindow _window;

		public List<Drawable> Drawables { get; }

		public Dictionary<uint, Action>         JoystickEvents { get; }
		public Dictionary<Keyboard.Key, Action> KeyboardEvents { get; }

		public Window(uint width, uint height, string title)
		{
			Drawables      = new List<Drawable>();
			JoystickEvents = new Dictionary<uint, Action>();
			KeyboardEvents = new Dictionary<Keyboard.Key, Action>();

			const uint windowScale = 4;

			_window = new RenderWindow(new VideoMode(width * windowScale, height * windowScale), title, Styles.Close | Styles.Titlebar);
			_window.SetFramerateLimit(60);

			_window.Closed                += (sender, args) => _window.Close();
			_window.JoystickButtonPressed += (sender, args) =>
			{
				if (JoystickEvents.ContainsKey(args.Button)) JoystickEvents[args.Button]();
			};
			_window.KeyPressed += (sender, args) =>
			{
				if (KeyboardEvents.ContainsKey(args.Code)) KeyboardEvents[args.Code]();
			};

			var view = new View(_window.GetView());
			view.Zoom(1f / windowScale);
			view.Center = new Vector2f(width / 2f, height / 2f);
			_window.SetView(view);
		}

		public void MainLoop()
		{
			var texture = new Texture(@"P:\CS\otter-rpg\otter-rpg-engine\Graphics\smw.png")
			{
				Smooth   = false,
				Repeated = true,
			};
			var sprite = new Sprite(texture)
			{
				TextureRect = new IntRect(0, 0, (int)(texture.Size.X * 3), (int)(texture.Size.Y * 2)),
			};

			var font = new SFML.Graphics.Font(@"P:\CS\otter-rpg\otter-rpg-engine\Graphics\PAPYRUS.TTF");
			var text = new Text("Test", font);

			while (_window.IsOpen)
			{
				_window.DispatchEvents();

				_window.Clear();

				foreach (var drawable in Drawables)
				{
					_window.Draw(drawable);
				}

				_window.Display();
			}
		}
	}
}
