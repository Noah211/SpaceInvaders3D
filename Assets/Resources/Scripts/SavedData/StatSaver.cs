using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine;

// Author: Noah Logan

public static class StatSaver
{
    public static void SaveStats(int highScore)
    {
        string directory = Application.persistentDataPath + Constants.SAVED_DATA;
        string fileName = Application.persistentDataPath + Constants.SAVED_STATS;

        // If the directory does not exist, create it
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Clear the XML File if it exists
        string[] fileArray = Directory.GetFiles(directory, "*.xml");

        if (fileArray.Length > 0)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            doc.DocumentElement.RemoveAll();
            doc.Save(fileName);
        }

        // Save stats to XML File
        XmlTextWriter writer = new XmlTextWriter(fileName, Encoding.UTF8);
        writer.WriteStartDocument(true);
        writer.Formatting = Formatting.Indented;
        writer.WriteStartElement("Stats");
        writer.WriteStartElement("HighScore");
        writer.WriteString(highScore.ToString());
        writer.WriteEndElement();
        writer.WriteEndElement();
        writer.Close();
    }
}
