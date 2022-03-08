
using System;
using System.Linq;
using Google.Protobuf;
using GameProtocol;

class Message
{
    private const int bufferHead = 4;

    private static byte[] buffer = new byte[1024];

    private int startindex;

    public byte[] Buffer
    {
        get { return buffer; }
    }

    public int StartIndex
    {
        get { return startindex; }
    }
    /// <summary>
    /// Buffer剩余空间
    /// </summary>
    public int Remsize
    {
        get { return buffer.Length - startindex; }
    }

    public void ReadBuffer(byte[] bufBytes, Action<MainPack> handleResponse = null)
    {
        var length = bufBytes.Length;

        for (int i = 0; i < length; i++)
        {
            Buffer[i] = bufBytes[i];
        }

        startindex += length;

        if (startindex <= bufferHead)
        {
            return;
        }

        //int count = BitConverter.ToInt32(buffer, 0);  //golang 的byte为int8 最大256！！！

        int count = length - 4;

        int bufferAllCount = count + bufferHead;    //整条消息的长度

        while (true)
        {
            if (startindex >= (count + bufferHead))
            {
                MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, bufferHead, count);

                if (handleResponse != null)
                {
                    handleResponse(pack);
                }

                Array.Copy(buffer, bufferAllCount, buffer, 0, startindex - bufferAllCount);

                startindex -= bufferAllCount;
            }
            else
            {
                break;
            }
        }
    }

    public void ReadBuffer(int length, Action<MainPack> handleResponse = null)
    {
        startindex += length;

        if (startindex <= bufferHead)
        {
            return;
        }

        //int count = BitConverter.ToInt32(buffer, 0);  //golang 的byte为int8 最大256！！！

        int count = length - 4;

        int bufferAllCount = count + bufferHead;    //整条消息的长度

        while (true)
        {
            if (startindex >= (count + bufferHead))
            {
                MainPack pack = (MainPack)MainPack.Descriptor.Parser.ParseFrom(buffer, bufferHead, count);

                if (handleResponse != null)
                {
                    handleResponse(pack);
                }

                Array.Copy(buffer, bufferAllCount, buffer, 0, startindex - bufferAllCount);

                startindex -= bufferAllCount;
            }
            else
            {
                break;
            }
        }
    }

    public static byte[] PackData(MainPack pack)
    {
        byte[] data = pack.ToByteArray();//包体
        byte[] head = BitConverter.GetBytes(data.Length);//包头
        return head.Concat(data).ToArray();
    }

    public static byte[] PackDataUDP(MainPack pack)
    {
        return pack.ToByteArray();
    }
}

public class ProtoUtil
{
    public static MainPack BuildMainPack(RequestCode requestCode, ActionCode actionCode)
    {
        MainPack pack = new MainPack();
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        return pack;
    }
}