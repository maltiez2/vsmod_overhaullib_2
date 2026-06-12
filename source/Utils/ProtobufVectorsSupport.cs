using OpenTK.Mathematics;
using ProtoBuf;
using ProtoBuf.Meta;
using Vintagestory.API.Common;

namespace OverhaulLib.Utils.ProtoBufSupport;

public class OpenTKProtoBufSupportSystem : ModSystem
{
    public override void StartPre(ICoreAPI api)
    {
        Register();
    }


    private static bool _registered;
    private static readonly Lock _lock = new();

    private static void Register()
    {
        lock (_lock) // to avoid double resgitering from server and client threads, if for some reason this method will be called at the same time, though should not happen in practice
        {
            if (_registered) return;

            RuntimeTypeModel model = RuntimeTypeModel.Default;

            AddSurrogateIfNotExists<Vector2, Vector2Surrogate>(model);
            AddSurrogateIfNotExists<Vector3, Vector3Surrogate>(model);
            AddSurrogateIfNotExists<Vector4, Vector4Surrogate>(model);
            AddSurrogateIfNotExists<Vector2i, Vector2iSurrogate>(model);
            AddSurrogateIfNotExists<Vector3i, Vector3iSurrogate>(model);
            AddSurrogateIfNotExists<Vector4i, Vector4iSurrogate>(model);
            AddSurrogateIfNotExists<Vector2d, Vector2dSurrogate>(model);
            AddSurrogateIfNotExists<Vector3d, Vector3dSurrogate>(model);
            AddSurrogateIfNotExists<Vector4d, Vector4dSurrogate>(model);
            AddSurrogateIfNotExists<Vector2h, Vector2hSurrogate>(model);
            AddSurrogateIfNotExists<Vector3h, Vector3hSurrogate>(model);
            AddSurrogateIfNotExists<Vector4h, Vector4hSurrogate>(model);

            _registered = true;
        }
    }

    private static void AddSurrogateIfNotExists<TOriginal, TSurrogate>(RuntimeTypeModel model)
    {
        if (!model.IsDefined(typeof(TOriginal)))
        {
            model.Add(typeof(TOriginal), false).SetSurrogate(typeof(TSurrogate));
        }
    }
}

#region Surroage types
[ProtoContract]
internal struct Vector2Surrogate
{
    [ProtoMember(1)] public float X { get; set; }
    [ProtoMember(2)] public float Y { get; set; }

    public static implicit operator Vector2(Vector2Surrogate s) => new(s.X, s.Y);
    public static implicit operator Vector2Surrogate(Vector2 v) => new() { X = v.X, Y = v.Y };
}

[ProtoContract]
internal struct Vector3Surrogate
{
    [ProtoMember(1)] public float X { get; set; }
    [ProtoMember(2)] public float Y { get; set; }
    [ProtoMember(3)] public float Z { get; set; }

    public static implicit operator Vector3(Vector3Surrogate s) => new(s.X, s.Y, s.Z);
    public static implicit operator Vector3Surrogate(Vector3 v) => new() { X = v.X, Y = v.Y, Z = v.Z };
}

[ProtoContract]
internal struct Vector4Surrogate
{
    [ProtoMember(1)] public float X { get; set; }
    [ProtoMember(2)] public float Y { get; set; }
    [ProtoMember(3)] public float Z { get; set; }
    [ProtoMember(4)] public float W { get; set; }

    public static implicit operator Vector4(Vector4Surrogate s) => new(s.X, s.Y, s.Z, s.W);
    public static implicit operator Vector4Surrogate(Vector4 v) => new() { X = v.X, Y = v.Y, Z = v.Z, W = v.W };
}

[ProtoContract]
internal struct Vector2iSurrogate
{
    [ProtoMember(1)] public int X { get; set; }
    [ProtoMember(2)] public int Y { get; set; }

    public static implicit operator Vector2i(Vector2iSurrogate s) => new(s.X, s.Y);
    public static implicit operator Vector2iSurrogate(Vector2i v) => new() { X = v.X, Y = v.Y };
}

[ProtoContract]
internal struct Vector3iSurrogate
{
    [ProtoMember(1)] public int X { get; set; }
    [ProtoMember(2)] public int Y { get; set; }
    [ProtoMember(3)] public int Z { get; set; }

    public static implicit operator Vector3i(Vector3iSurrogate s) => new(s.X, s.Y, s.Z);
    public static implicit operator Vector3iSurrogate(Vector3i v) => new() { X = v.X, Y = v.Y, Z = v.Z };
}

[ProtoContract]
internal struct Vector4iSurrogate
{
    [ProtoMember(1)] public int X { get; set; }
    [ProtoMember(2)] public int Y { get; set; }
    [ProtoMember(3)] public int Z { get; set; }
    [ProtoMember(4)] public int W { get; set; }

    public static implicit operator Vector4i(Vector4iSurrogate s) => new(s.X, s.Y, s.Z, s.W);
    public static implicit operator Vector4iSurrogate(Vector4i v) => new() { X = v.X, Y = v.Y, Z = v.Z, W = v.W };
}

[ProtoContract]
internal struct Vector2dSurrogate
{
    [ProtoMember(1)] public double X { get; set; }
    [ProtoMember(2)] public double Y { get; set; }

    public static implicit operator Vector2d(Vector2dSurrogate s) => new(s.X, s.Y);
    public static implicit operator Vector2dSurrogate(Vector2d v) => new() { X = v.X, Y = v.Y };
}

[ProtoContract]
internal struct Vector3dSurrogate
{
    [ProtoMember(1)] public double X { get; set; }
    [ProtoMember(2)] public double Y { get; set; }
    [ProtoMember(3)] public double Z { get; set; }

    public static implicit operator Vector3d(Vector3dSurrogate s) => new(s.X, s.Y, s.Z);
    public static implicit operator Vector3dSurrogate(Vector3d v) => new() { X = v.X, Y = v.Y, Z = v.Z };
}

[ProtoContract]
internal struct Vector4dSurrogate
{
    [ProtoMember(1)] public double X { get; set; }
    [ProtoMember(2)] public double Y { get; set; }
    [ProtoMember(3)] public double Z { get; set; }
    [ProtoMember(4)] public double W { get; set; }

    public static implicit operator Vector4d(Vector4dSurrogate s) => new(s.X, s.Y, s.Z, s.W);
    public static implicit operator Vector4dSurrogate(Vector4d v) => new() { X = v.X, Y = v.Y, Z = v.Z, W = v.W };
}

[ProtoContract]
internal struct Vector2hSurrogate
{
    // raw ushort bits instead of OpenTK.Mathematics.Half, cause it doesnt serialize cleanly
    [ProtoMember(1)] public ushort X { get; set; }
    [ProtoMember(2)] public ushort Y { get; set; }

    public static implicit operator Vector2h(Vector2hSurrogate s) => new(new OpenTK.Mathematics.Half(s.X), new OpenTK.Mathematics.Half(s.Y));
    public static implicit operator Vector2hSurrogate(Vector2h v) => new() { X = v.X.IsNaN ? (ushort)0 : (ushort)v.X, Y = v.Y.IsNaN ? (ushort)0 : (ushort)v.Y };
}

[ProtoContract]
internal struct Vector3hSurrogate
{
    [ProtoMember(1)] public ushort X { get; set; }
    [ProtoMember(2)] public ushort Y { get; set; }
    [ProtoMember(3)] public ushort Z { get; set; }

    public static implicit operator Vector3h(Vector3hSurrogate s) => new(new OpenTK.Mathematics.Half(s.X), new OpenTK.Mathematics.Half(s.Y), new OpenTK.Mathematics.Half(s.Z));
    public static implicit operator Vector3hSurrogate(Vector3h v) => new() { X = (ushort)v.X, Y = (ushort)v.Y, Z = (ushort)v.Z };
}

[ProtoContract]
internal struct Vector4hSurrogate
{
    [ProtoMember(1)] public ushort X { get; set; }
    [ProtoMember(2)] public ushort Y { get; set; }
    [ProtoMember(3)] public ushort Z { get; set; }
    [ProtoMember(4)] public ushort W { get; set; }

    public static implicit operator Vector4h(Vector4hSurrogate s) => new(new OpenTK.Mathematics.Half(s.X), new OpenTK.Mathematics.Half(s.Y), new OpenTK.Mathematics.Half(s.Z), new OpenTK.Mathematics.Half(s.W));
    public static implicit operator Vector4hSurrogate(Vector4h v) => new() { X = (ushort)v.X, Y = (ushort)v.Y, Z = (ushort)v.Z, W = (ushort)v.W };
}
#endregion
