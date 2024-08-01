namespace Wacton.Unicolour.Icc;

public record Tag(string Signature, uint Offset, uint Size, byte[] Data)
{
    public string Signature { get; } = Signature;
    public uint Offset { get; } = Offset;
    public uint Size { get; } = Size;
    public byte[] Data { get; } = Data;

    public override string ToString() => $"{Signature} @ byte {Offset} for {Size} bytes";
}
