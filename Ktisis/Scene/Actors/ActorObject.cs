﻿using ImGuiNET;

using Dalamud.Logging;

using FFXIVClientStructs.FFXIV.Client.Graphics.Render;

using Ktisis.Services;
using Ktisis.Scene.Skeletons;
using Ktisis.Interface.Dialog;
using Ktisis.Structs.Actor;
using Ktisis.Structs.Poses;

namespace Ktisis.Scene.Actors {
	public class ActorObject : Manipulable, Transformable, HasSkeleton {
		// ActorObject

		private int Index;

		private string? Nickname;

		public ActorObject(int x) {
			Index = x;
			AddChild(new SkeletonObject());
		}

		// Manipulable

		public override uint Color => 0xFF6EE266;

		public unsafe override string Name {
			get {
				var actor = GetActor();
				return actor != null ? actor->GetNameOrId() : "INVALID";
			}
			set => Nickname = value;
		}

		public override void Select() {
			if (ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left)) {
				PluginLog.Information($"Target {Index}");
			} else {
				var ctrl = ImGui.IsKeyDown(ImGuiKey.LeftCtrl);
				EditorService.Select(this, ctrl);
			}
		}

		public override void Context() {
			var ctx = new ContextMenu();

			ctx.AddSection(new() {
				{ "Select", Select },
				{ "Set nickname...", null! }
			});

			ctx.AddSection(new() {
				{ "Open appearance editor", null! },
				{ "Open animation control", null! },
				{ "Open gaze control", null! }
			});

			ctx.Show();
		}

		// Actor

		internal unsafe Actor* GetActor()
			=> (Actor*)DalamudServices.ObjectTable.GetObjectAddress(Index);

		// SkeletonObject

		public unsafe Skeleton* GetSkeleton() {
			var actor = GetActor();
			return actor != null ? actor->GetSkeleton() : null;
		}

		// Transformable

		public unsafe object? GetTransform() {
			var actor = GetActor();
			if (actor == null || actor->Model == null) return null;
			return Transform.FromHavok(actor->Model->Transform);
		}

		public unsafe void SetTransform(object trans) {
			var actor = GetActor();
			if (actor == null || actor->Model == null) return;
			actor->Model->Transform = ((Transform)trans).ToHavok();
		}
	}
}