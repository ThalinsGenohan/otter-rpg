﻿using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using Shellblade.Graphics.UI;
using Text = Shellblade.Graphics.UI.Text;

namespace Shellblade.Graphics
{
	public class Textbox : UIElement
	{
		public static Dictionary<string, Func<string>> Strings { get; set; } = new Dictionary<string, Func<string>>();

		private readonly RectangleShape _background;
		private readonly Text           _text;

		private ulong _timer = 0;

		public uint TextDelay { get; set; } = 50;

		public string Text
		{
			get => _text.String;
			set => _text.String = value;
		}

		private Vector2i Inside => Size - new Vector2i(16, 16);

		public Textbox(Vector2i pos, Vector2i size)
		{
			Position = pos;
			Size     = size;

			_text = new Text
			{
				Position    = Position + new Vector2i(8, 8),
				Size        = Inside,
				LineSpacing = 1,
				Instant     = false,
			};

			_background = new RectangleShape((Vector2f)Size)
			{
				Position = (Vector2f)Position,
				Texture = new Texture(@"P:\CS\otter-rpg\otter-rpg-engine\Graphics\testbox.png")
				{
					Repeated = true,
					Smooth   = false,
				},
				TextureRect      = new IntRect(0, 0, size.X, size.Y),
				OutlineColor     = Color.White,
				OutlineThickness = -6f,
				FillColor        = new Color(0xffffff55),
			};
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(_background, states);
			target.Draw(_text,       states);
		}

		public void Next()
		{
			if (!_text.PageDone)
			{
				_text.DrawIndex = _text.CurrentPage.Count - 1;
				return;
			}

			if (_text.LastPage) return;

			_text.PageIndex++;
		}

		public void UpdateScroll(int ms)
		{
			if (_text.PageDone) return;

			_timer += (ulong)ms;
			while (_timer >= TextDelay)
			{
				_timer -= TextDelay;
				_text.DrawIndex++;
			}
		}
	}
}
