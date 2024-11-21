using BossModReborn.Util;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BossMod;

class DebugTeleport
{
    private Vector3 inputCoordinates = new Vector3(0, 0, 0);
    private Vector3 playerCoordinates = new Vector3(PlayerEx.Position.X, PlayerEx.Position.Y, PlayerEx.Position.Z);
    public unsafe void Draw()
    {
        ImGui.BeginGroup();
        ImGui.Text("Current Player Coordinates:");
        ImGui.Text("X: " + PlayerEx.Position.X.ToString("F3"));
        ImGui.Text("Y: " + PlayerEx.Position.Y.ToString("F3"));
        ImGui.Text("Z: " + PlayerEx.Position.Z.ToString("F3"));
        ImGui.EndGroup();
        ImGui.Separator();
        ImGui.BeginGroup();
        ImGui.Text("Enter Target Coordinates:");
        if (ImGui.Button("Set Position"))
        {
            SetPlayerPosition(inputCoordinates);
        }
        ImGui.SetNextItemWidth(150);
        ImGui.InputFloat("X Coordinate", ref inputCoordinates.X, 1.0f);
        ImGui.SetNextItemWidth(150);
        ImGui.InputFloat("Y Coordinate", ref inputCoordinates.Y, 1.0f);
        ImGui.SetNextItemWidth(150);
        ImGui.InputFloat("Z Coordinate", ref inputCoordinates.Z, 1.0f);
        ImGui.EndGroup();
    }

    private void SetPlayerPosition(Vector3 position)
    {
        try
        {
            if (Service.ClientState.LocalPlayer != null)
            {
                // Assuming PlayerEx.SetPosition accepts a Vector3
                PlayerEx.SetPosition = position;
                Service.Log($"Player position set to: X = {position.X}, Y = {position.Y}, Z = {position.Z}");
            }
            else
            {
                Service.Log("LocalPlayer is null. Unable to set position.");
            }
        }
        catch (Exception ex)
        {
            Service.Log($"An error occurred while setting position: {ex.Message}");
        }
    }
}
