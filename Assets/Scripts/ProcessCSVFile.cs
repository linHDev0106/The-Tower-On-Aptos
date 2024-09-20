using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProcessCSVFile : MonoBehaviour
{
    //This is the file we are writing to/reading from
    public TextAsset file;

    public List<LanguageItem> itemLangList = new List<LanguageItem>();

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData()
    {
       
        //This is to get all the lines using Method 1
        string[] lines = file.text.Split("\n"[0]);

        for (var i = 1; i < lines.Length; i++)
        {
            LanguageItem item = new LanguageItem();
            //This is to get every thing that is comma separated
            string[] parts = lines[i].Split(","[0]);
            //Debug.Log("Line " + i + " " + lines[i]);

            item.ID = parts[0].ToString();
            item.EnumID = (i-1).ToString();

            item.Dutch = parts[11];
            item.English = parts[1];
            item.French = parts[5];
            item.German = parts[6];
            item.Japanese = parts[8];
            item.Korean = parts[7];
            item.Norwegian = parts[10];
            item.Portuguese = parts[4];
            item.Spanish = parts[3];
            item.Vietnamese = parts[2];
            item.ChineseTraditional = parts[9];

            itemLangList.Add(item);
        }

        SaveFile();
    }

    public void SaveFile()
    {
        string prefixStr = "ID,EnumID,Dutch,English,French,German,Japanese,Korean,Norwegian,Portuguese,Spanish,Vietnamese,ChineseTraditional";


        // The target file path e.g.
        var folder = Application.streamingAssetsPath;

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        var filePath = Path.Combine(folder, "Localization.csv");

        //This is the writer, it writes to the filepath
        StreamWriter writer = new StreamWriter(filePath);

        //This is writing the line of the type, name, damage... etc... (I set these)
        writer.WriteLine(prefixStr);
        //This loops through everything in the inventory and sets the file to these.
        for (int i = 0; i < itemLangList.Count; ++i)
        {
            writer.WriteLine(itemLangList[i].ID +
                "," + itemLangList[i].EnumID +
                "," + itemLangList[i].Dutch +
                "," + itemLangList[i].English +
                "," + itemLangList[i].French +
                "," + itemLangList[i].German +
                "," + itemLangList[i].Japanese +
                "," + itemLangList[i].Korean +
                "," + itemLangList[i].Norwegian +
                "," + itemLangList[i].Portuguese +
                "," + itemLangList[i].Spanish +
                "," + itemLangList[i].Vietnamese +
                "," + itemLangList[i].ChineseTraditional
                );
        }
        writer.Flush();
        //This closes the file
        writer.Close();

    }
    private string getPath()
    {
#if UNITY_EDITOR
        return Application.dataPath + "/CSV/" + "Saved_Inventory.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_Inventory.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_Inventory.csv";
#else
        return Application.dataPath +"/"+"Saved_Inventory.csv";
#endif
    }
}

[System.Serializable]
public class LanguageItem
{
    public string ID;

    public string EnumID;

    public string Dutch;

    public string English;

    public string French;

    public string German;

    public string Japanese;

    public string Korean;

    public string Norwegian;

    public string Portuguese;

    public string Spanish;

    public string Vietnamese;

    public string ChineseTraditional;
}