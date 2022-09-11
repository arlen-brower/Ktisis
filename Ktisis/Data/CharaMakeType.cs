﻿using Lumina.Data;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets;

using Dalamud.Game.ClientState.Objects.Enums;

namespace Ktisis.Data {
	public enum MenuType : byte {
		List = 0,
		Select = 1,
		Color = 2,
		Unknown1 = 3,
		SelectMulti = 4,
		Slider = 5
	}

	public struct Menu {
		public string Name;
		public byte Default;
		public MenuType Type;
		public byte Count;
		// LookAt
		// SubMenuMask
		public CustomizeIndex Index;
		public uint[] Params;
		public byte[] Graphics;
	}

	[Sheet("CharaMakeType")]
	public class CharaMakeType : ExcelRow {
		// Consts

		public const int MenuCt = 28;
		public const int VoiceCt = 12;
		public const int GraphicCt = 10;

		// Properties

		public LazyRow<Race> Race { get; set; } = null!;
		public LazyRow<Tribe> Tribe { get; set; } = null!;
		public sbyte Gender { get; set; }

		public Menu[] Menus { get; set; } = new Menu[MenuCt];
		public byte[] Voices { get; set; } = new byte[VoiceCt];

		// Build sheet

		public override void PopulateData(RowParser parser, Lumina.GameData gameData, Language language) {
			base.PopulateData(parser, gameData, language);

			Race = new LazyRow<Race>(gameData, parser.ReadColumn<int>(0), language);
			Tribe = new LazyRow<Tribe>(gameData, parser.ReadColumn<int>(1), language);
			Gender = parser.ReadColumn<sbyte>(2);

			for (var i = 0; i < MenuCt; i++) {
				var ct = parser.ReadColumn<byte>(3 + 3 * MenuCt + i);
				var menu = new Menu() {
					Name = new LazyRow<Lobby>(gameData, parser.ReadColumn<uint>(3 + i), language).Value!.Text,
					Default = parser.ReadColumn<byte>(3 + 1 * MenuCt + i),
					Type = (MenuType)parser.ReadColumn<byte>(3 + 2 * MenuCt + i),
					Count = ct,
					Index = (CustomizeIndex)parser.ReadColumn<uint>(3 + 6 * MenuCt + i),
					Params = new uint[ct],
					Graphics = new byte[GraphicCt]
				};

				if (menu.Type == MenuType.List
				|| menu.Type == MenuType.Select
				|| menu.Type == MenuType.SelectMulti) {
					for (var p = 0; p < ct; p++)
						menu.Params[p] = parser.ReadColumn<uint>(3 + (7 + p) * MenuCt + i);
					for (var g = 0; g < GraphicCt; g++)
						menu.Graphics[g] = parser.ReadColumn<byte>(3 + (107 + g) * MenuCt + i);
				}

				Menus[i] = menu;
			}
		}
	}
}