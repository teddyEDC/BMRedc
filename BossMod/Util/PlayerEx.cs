using BossMod;
using FFXIVClientStructs.FFXIV.Client.Game.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGameObject = FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject;

namespace BossModReborn.Util;
public static unsafe class PlayerEx
{
    public static CSGameObject* GameObject
    {
        get
        {
            var localPlayer = Service.ClientState.LocalPlayer;
            return localPlayer != null ? (CSGameObject*)localPlayer.Address : null;
        }
    }

    public static Vector3? SetPosition
    {
        get
        {
            var localPlayer = Service.ClientState.LocalPlayer;
            return localPlayer != null ? localPlayer.Position : (Vector3?)null;
        }
        set
        {
            if (GameObject != null && value.HasValue)
            {
                GameObject->SetPosition(value.Value.X, value.Value.Y, value.Value.Z);
            }
        }
    }

    public static Vector3 Position
    {
        get
        {
            var localPlayer = Service.ClientState.LocalPlayer;
            return (Vector3)(localPlayer != null ? localPlayer.Position : (Vector3?)null);
        }
    }

    public static void SetPlayerPosition(Vector3 position)
    {
        try
        {
            if (Service.ClientState.LocalPlayer != null)
            {
                // Assuming PlayerEx.SetPosition accepts a Vector3
                SetPosition = position;
                Service.Log("Setting player position to: " + position.ToString());

            }
            else
            {
                Service.Log("LocalPlayer is null");
            }
        }
        catch (Exception ex)
        {
            Service.Log("Error in SetPlayerPosition" + ex);
        }
    }
}
