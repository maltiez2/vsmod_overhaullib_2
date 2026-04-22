using System.Text;
using Vintagestory.API.Common;

namespace OverhaulLib;

public static class LoadedModsLogger
{
    public static void LogLoadedMods(ICoreAPI api)
    {
        string[] headers = { "ModID", "Version", "Name", "FileName", "Dependencies", "Authors", "ModDB (if alias set by author)" };

        List<string[]> rows = api.ModLoader.Mods
            .Select(mod => new[]
            {
                mod.Info.ModID,
                mod.Info.Version,
                mod.Info.Name,
                mod.FileName,
                AggregateModDependencies(mod.Info.Dependencies),
                AggregateModAuthors(mod.Info.Authors, mod),
                $"https://mods.vintagestory.at/{mod.Info.ModID}"
            })
            .OrderBy(row => row[5])
            .ToList();

        int[] columnWidths = new int[headers.Length];
        for (int i = 0; i < headers.Length; i++)
        {
            columnWidths[i] = Math.Max(
                headers[i].Length,
                rows.Max(row => row[i]?.Length ?? 0)
            );
        }

        StringBuilder table = new();

        table.AppendLine(FormatRow(headers, columnWidths));

        foreach (string[]? row in rows)
        {
            table.AppendLine(FormatRow(row, columnWidths));
        }

        api.Logger.Notification($"Loaded mods ({rows.Count}):\n{table}");
    }

    private static string FormatRow(string[] cells, int[] columnWidths)
    {
        StringBuilder row = new();
        for (int i = 0; i < cells.Length; i++)
        {
            if (i > 0) row.Append("  ");
            row.Append((cells[i] ?? "").PadRight(columnWidths[i]));
        }
        return row.ToString();
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

    private static string AggregateModDependencies(IEnumerable<ModDependency> dependencies)
    {
        if (dependencies == null || !dependencies.Any())
        {
            return "-";
        }

        return dependencies
            .Select(dep => string.IsNullOrEmpty(dep.Version) ? dep.ModID : $"{dep.ModID}@{dep.Version}")
            .Aggregate((f, s) => $"{f}, {s}");
    }
}