using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

MP3 mP3 = new MP3();
var path = "D:\\Sepehr Khalse - Ghalbe Banafsh 128.mp3";
mP3.Process(path);

public class MP3
{
    public const string TagIdentifierV2 = "ID3";
    public const string TagIdentifierV1 = "TAG";
    public bool ValidFormat { get; private set; }
    private List<char> data;
    public bool hasID3v2 = false;
    public bool hasID3v1 = false;
    public void Process(string path)
    {
        data = File.ReadAllText(path).ToArray().ToList();
        //data = Array.ConvertAll(File.ReadAllBytes(path), (byte t) => (char)t).ToList();
        bool hasTag = true;
        for (int i = 0; i < 3; i++)
        {
            if (data[i] != TagIdentifierV2[i])
            {
                hasTag = false;
                break;
            }
        }
        if (hasTag)
        {
            var iD3V2 = new ID3V2(data);
            var s = iD3V2.calcSize();
            hasID3v2 = true;
        }
        hasTag = true;
        for (int i = 0; i < 3; i++)
        {
            if (data[data.Count - 128 + i] != TagIdentifierV1[i])
            {
                hasTag = false;
                break;
            }
        }
        if (hasTag)
        {
            var iD3V1 = new ID3V1(data);

            //memcpy(&id3v1, &data[data.size() - 128], sizeof(ID3::ID3V1));
            //data.resize(data.size() - sizeof(ID3::ID3V1));
            hasID3v1 = true;
        }

    }



}

public struct ID3V1
{
    public ID3V1(List<char> data)
    {
        var index = data.Count - 128;
        int j = 0;
        for (int i = 0; i < header.Length; i++, j++)
        {
            header[i] = data[j + index];
        }
        for (int i = 0; i < title.Length; i++, j++)
        {
            title[i] = data[j + index];
        }
        for (int i = 0; i < artist.Length; i++, j++)
        {
            artist[i] = data[j + index];
        }
        for (int i = 0; i < album.Length; i++, j++)
        {
            album[i] = data[j + index];
        }
        for (int i = 0; i < year.Length; i++, j++)
        {
            year[i] = data[j + index];
        }
        for (int i = 0; i < comment.Length; i++, j++)
        {
            comment[i] = data[j + index];
        }
        genre =(byte) data[j++ + index];
        for (int i = 0; i < Length; i++)
        {
            data.RemoveAt(index);
        }
    }
    public char[] header = new char[3];
    public char[] title = new char[30];
    public char[] artist = new char[30];
    public char[] album = new char[30];
    public char[] year = new char[4];
    public char[] comment = new char[30];
    public byte genre = 0;
    public int Length = 128;
}
public struct ID3V2
{
    public ID3V2(List<char> data)
    {
        int j = 0;
        for (int i = 0; i < identifier.Length; i++, j++)
        {
            identifier[i] = data[j];
        }
        major = data[j++];
        minor = data[j++];
        flags = data[j++];
        for (int i = 0; i < size.Length; i++, j++)
        {
            size[i] = data[j];
        }
        for (int i = 0; i < Length; i++)
        {
            data.RemoveAt(0);
        }
    }
    public int Length = 10;
    public char[] identifier = new char[3];
    public char major = ' ';
    public char minor = ' ';
    public char flags = ' ';
    public char[] size = new char[4];
    public int calcSize()
    {
        int ret = 0;
        for (int i = 0; i < 4; i++)
        {
            ret += size[3 - i] << (7 * i);
        }
        return ret;
    }
};