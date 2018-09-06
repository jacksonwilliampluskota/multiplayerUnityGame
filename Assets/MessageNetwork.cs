using UnityEngine;
using UnityEngine.Networking;

//msg.Serialize(writer);
public class MessageNetwork : MessageBase
{
    public uint netId;
    public NetworkHash128 assetId;
    public Vector3 position;
    public byte[] payload;

    // This method would be generated
    public override void Deserialize(NetworkReader reader)
    {
        netId = reader.ReadPackedUInt32();
        assetId = reader.ReadNetworkHash128();
        position = reader.ReadVector3();
        payload = reader.ReadBytesAndSize();
    }

    // This method would be generated
    public override void Serialize(NetworkWriter writer)
    {
        writer.WritePackedUInt32(netId);
        writer.Write(assetId);
        writer.Write(position);
        writer.WriteBytesFull(payload);
    }  
}
