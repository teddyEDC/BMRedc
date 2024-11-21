using CSGameObject = FFXIVClientStructs.FFXIV.Client.Game.Object.GameObject;

namespace BossMod.Util;
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
            return localPlayer?.Position;
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
            return localPlayer != null ? localPlayer.Position : Vector3.Zero;
        }
    }

    public static void SetPlayerPosition(Vector3 position)
    {
        try
        {
            if (Service.ClientState.LocalPlayer != null)
            {
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
