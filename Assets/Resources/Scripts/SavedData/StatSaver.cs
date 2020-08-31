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
        // Clear the XML File if it exists
        string[] fileArray = Directory.GetFiles(Constants.SAVED_DATA, "*.xml");

        if (fileArray.Length > 0)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Constants.SAVED_STATS);
            doc.DocumentElement.RemoveAll();
            doc.Save(Constants.SAVED_STATS);
        }

        // Save stats to XML File
        XmlTextWriter writer = new XmlTextWriter(Constants.SAVED_STATS, Encoding.UTF8);
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
