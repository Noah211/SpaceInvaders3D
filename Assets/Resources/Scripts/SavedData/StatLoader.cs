using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Text;
using UnityEngine;

// Author: Noah Logan

public static class StatLoader
{
    public static int LoadHighScore()
    {
        int highScore = 0;
        XmlDocument doc = new XmlDocument();
        doc.Load(Constants.SAVED_STATS);

        foreach (XmlNode node in doc.DocumentElement)
        {
            if (node.Name.Equals("HighScore") && node.HasChildNodes)
            {
                highScore = int.Parse(node.ChildNodes.Item(0).InnerText);
            }
        }

        return highScore;
    }
}
