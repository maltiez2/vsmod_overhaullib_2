using Cairo;
using OpenTK.Mathematics;
using OverhaulLib.Utils;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace OverhaulLib;

public sealed class OverhaulLibSystem : ModSystem
{
    public override double ExecuteOrder() => 0;
    
    public override void StartPre(ICoreAPI api)
    {
        LogLoadedMods(api);

        _shapesCache = new(api, "Shapes", TimeSpan.FromMinutes(10), threadSafe: true);
        ShapeLoadingUtil.ShapesCache = _shapesCache;
    }

    public override void AssetsLoaded(ICoreAPI api)
    {
        if (api is ICoreClientAPI clientApi)
        {
            LoadIcons(clientApi);
        }
    }

    public override void Dispose()
    {
        _shapesCache?.Dispose();
    }



    private static readonly Vector4 _iconScale = new(-0.1f, -0.1f, 1.2f, 1.2f);
    private const string _iconsFolder = "sloticons";
    private const string _iconsPath = $"textures/{_iconsFolder}/";
    private ObjectCache<string, Shape>? _shapesCache;


    private static void LoadIcons(ICoreClientAPI api)
    {
        List<IAsset> icons = api.Assets.GetManyInCategory("textures", _iconsFolder, loadAsset: false);
        StringBuilder loadedIconsList = new();
        loadedIconsList.AppendLine("Loaded .svg icons: ");
        foreach (IAsset icon in icons)
        {
            string iconPath = icon.Location.ToString();
            string iconCode = icon.Location.Domain + ":" + icon.Location.Path[_iconsPath.Length..^4].ToLowerInvariant();

            if (!iconPath.ToLowerInvariant().EndsWith(".svg"))
            {
                Log.Verbose(api, typeof(OverhaulLibSystem), $"Icon should have '.svg' format, skipping. Path: {iconPath}");
                return;
            }

            loadedIconsList.AppendLine(iconCode);

            RegisterCustomIcon(api, iconCode, iconPath);
        }
        Log.Verbose(api, typeof(OverhaulLibSystem), loadedIconsList.ToString());
    }

    private static void LogLoadedMods(ICoreAPI api)
    {
        StringBuilder modsList = new();
        api.ModLoader.Mods
            .Select(mod => $"'{mod.Info.ModID}'\t'{mod.Info.Version}'\t'{mod.Info.Name}'\t'{mod.FileName}''\t'{AggregateModAuthors(mod.Info.Authors, mod)}")
            .Foreach(mod => modsList.AppendLine(mod));
        api.Logger.Event("Loaded mods:\n" + modsList.ToString());
    }

    private static string AggregateModAuthors(IEnumerable<string> list, Mod mod)
    {
        if (!list.Any())
        {
            mod.Logger.Warning($"Mod '{mod.Info.Name} ({mod.FileName})' has no authors specified in mod info.");
            return "-";
        }

        return list.Aggregate((f, s) => $"{f}, {s}");
    }

    private static void RegisterCustomIcon(ICoreClientAPI api, string key, string path)
    {
        api.Gui.Icons.CustomIcons[key] = delegate (Context ctx, int x, int y, float w, float h, double[] rgba)
        {
            int value = ColorUtil.ColorFromRgba(75, 75, 75, 255);

            if (rgba.Length == 4)
            {
                value = ColorUtil.ColorFromRgba(rgba);
            }

            if (rgba[0] == 0 && rgba[1] == 0 && rgba[2] == 0 && rgba[3] == 0.2) // To override vanilla clothes and armor icon color
            {
                value = ColorUtil.ColorFromRgba(75, 75, 75, 190);
            }

            AssetLocation location = new(path);
            IAsset svgAsset = api.Assets.TryGet(location);
            Surface target = ctx.GetTarget();

            int xNew = x + (int)(w * _iconScale.X);
            int yNew = y + (int)(h * _iconScale.Y);
            int wNew = (int)(w * _iconScale.W);
            int hNew = (int)(h * _iconScale.Z);

            api.Gui.DrawSvg(svgAsset, (ImageSurface)(object)((target is ImageSurface) ? target : null), xNew, yNew, wNew, hNew, value);
        };
    }
}