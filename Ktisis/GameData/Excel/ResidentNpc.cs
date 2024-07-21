﻿using FFXIVClientStructs.FFXIV.Client.Game.Character;

using Lumina.Data;
using Lumina.Excel;
using Lumina.Excel.GeneratedSheets2;

using Ktisis.Common.Extensions;
using Ktisis.GameData.Excel.Types;
using Ktisis.Structs.Characters;

namespace Ktisis.GameData.Excel;

[Sheet("ENpcResident", columnHash: 0xf74fa88c)]
public class ResidentNpc : ENpcResident, INpcBase {
	// Excel

	private LazyRow<EventNpc> EventNpc { get; set; } = null!;

	public override void PopulateData(RowParser parser, Lumina.GameData gameData, Language language) {
		base.PopulateData(parser, gameData, language);

		this.Name = this.Singular.FormatName(this.Article) ?? $"E:{this.RowId:D7}";
		this.EventNpc = new LazyRow<EventNpc>(gameData, this.RowId, language);
	}
	
	// INpcBase
	
	public string Name { get; set; } = string.Empty;
	
	public uint HashId { get; set; }

	public ushort GetModelId() => this.EventNpc.Value?.GetModelId() ?? ushort.MaxValue;

	public CustomizeContainer? GetCustomize() => this.EventNpc.Value?.GetCustomize();
	public EquipmentContainer? GetEquipment() => this.EventNpc.Value?.GetEquipment();

	public WeaponModelId? GetMainHand() => this.EventNpc.Value?.GetMainHand();
	public WeaponModelId? GetOffHand() => this.EventNpc.Value?.GetOffHand();
}