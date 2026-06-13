using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace OverhaulLib.Utils;

public static class ParsingUtils
{
    public static TObject? LoadObjectFromFile<TObject>(IAsset asset, ICoreAPI? api = null, object? caller = null, Type? type = null) where TObject : class
    {
        TObject? fileConent = null;
        try
        {
            fileConent = asset.ToObject<TObject>();
        }
        catch (Exception exception)
        {
            if (caller != null)
            {
                Log.Error(api, caller, $"Failed to load object from '{asset.Location}'. Exception: {exception}");
            }
            else if (type != null)
            {
                Log.Error(api, type, $"Failed to load object from '{asset.Location}'. Exception: {exception}");
            }
            else
            {
                Log.Error(api, typeof(ParsingUtils), $"Failed to load '{nameof(TObject)}' from '{asset.Location}'. Exception: {exception}");
            }
        }
        return fileConent;
    }
}
