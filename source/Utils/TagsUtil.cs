using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace OverhaulLib.Utils;

public static class TagsUtil
{
    public static ICoreAPI? Api { get; set; }

    public static TagSet Get(params string[] tags) => Api!.CollectibleTagRegistry.CreateTagSet((ReadOnlySpan<string>)tags);

    public static TagSet Union(this TagSet first, TagSet second)
    {
        IEnumerable<string> firstTags = Api!.CollectibleTagRegistry.SlowEnumerateTagNames(first);
        IEnumerable<string> secondTags = Api!.CollectibleTagRegistry.SlowEnumerateTagNames(second);
        return Api!.CollectibleTagRegistry.CreateTagSet(firstTags.Concat(secondTags));
    }

    public static TagSet Union(this TagSet first, params string[] tags) => first.Union(Get(tags));

    public static TagSet Intersect(this TagSet first, TagSet second)
    {
        IEnumerable<string> firstTags = Api!.CollectibleTagRegistry.SlowEnumerateTagNames(first);
        IEnumerable<string> secondTags = Api!.CollectibleTagRegistry.SlowEnumerateTagNames(second);
        return Api!.CollectibleTagRegistry.CreateTagSet(firstTags.Intersect(secondTags));
    }

    public static TagSet Intersect(this TagSet first, params string[] tags) => first.Intersect(Get(tags));

    public static TagSet Except(this TagSet first, TagSet second)
    {
        IEnumerable<string> firstTags = Api!.CollectibleTagRegistry.SlowEnumerateTagNames(first);
        IEnumerable<string> secondTags = Api!.CollectibleTagRegistry.SlowEnumerateTagNames(second);
        return Api!.CollectibleTagRegistry.CreateTagSet(firstTags.Except(secondTags));
    }

    public static TagSet Except(this TagSet first, params string[] tags) => first.Except(Get(tags));

    public static TagSet SymmetricExcept(this TagSet first, TagSet second)
    {
        IEnumerable<string> firstTags = Api!.CollectibleTagRegistry.SlowEnumerateTagNames(first);
        IEnumerable<string> secondTags = Api!.CollectibleTagRegistry.SlowEnumerateTagNames(second);
        HashSet<string> firstSet = [.. firstTags];
        HashSet<string> secondSet = [.. secondTags];
        firstSet.SymmetricExceptWith(secondSet);
        return Api!.CollectibleTagRegistry.CreateTagSet(firstSet);
    }

    public static TagSet SymmetricExcept(this TagSet first, params string[] tags) => first.SymmetricExcept(Get(tags));

    public static bool IsSubsetOf(this TagSet first, TagSet second)
    {
        return first.IsFullyContainedIn(second);
    }

    public static bool IsSubsetOf(this TagSet first, params string[] tags) => first.IsSubsetOf(Get(tags));

    public static bool IsSupersetOf(this TagSet first, TagSet second)
    {
        return second.IsFullyContainedIn(first);
    }

    public static bool IsSupersetOf(this TagSet first, params string[] tags) => first.IsSupersetOf(Get(tags));

    public static bool IsProperSubsetOf(this TagSet first, TagSet second)
    {
        return first.IsFullyContainedIn(second) && first != second;
    }

    public static bool IsProperSubsetOf(this TagSet first, params string[] tags) => first.IsProperSubsetOf(Get(tags));

    public static bool IsProperSupersetOf(this TagSet first, TagSet second)
    {
        return second.IsFullyContainedIn(first) && first != second;
    }

    public static bool IsProperSupersetOf(this TagSet first, params string[] tags) => first.IsProperSupersetOf(Get(tags));

    public static bool Overlaps(this TagSet first, params string[] tags) => first.Overlaps(Get(tags));

    public static bool SetEquals(this TagSet first, TagSet second)
    {
        return first == second;
    }

    public static bool SetEquals(this TagSet first, params string[] tags) => first.SetEquals(Get(tags));

    public static TagSet GetTagSet(this ICoreAPI api, params string[] tags) => api.CollectibleTagRegistry.CreateTagSet((ReadOnlySpan<string>)tags);

    public static void RegisterTags(this ICoreAPI api, params string[] tags) => api.CollectibleTagRegistry.Register((ReadOnlySpan<string>)tags);
}