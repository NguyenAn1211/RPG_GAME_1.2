using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;



public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool encryptData = false;
    private string codeWord = "annguyen"; // Khóa mã hóa

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encryptData = false)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encryptData = _encryptData;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName + ".xml");

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialize object to XML
            XmlSerializer serializer = new XmlSerializer(typeof(GameData));

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                serializer.Serialize(stream, _data);
            }

            if (encryptData)
            {
                EncryptFile(fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error while saving to file: " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName + ".xml");
        GameData loadData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                if (encryptData)
                {
                    DecryptFile(fullPath);
                }

                // Deserialize XML to object
                XmlSerializer serializer = new XmlSerializer(typeof(GameData));

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    loadData = serializer.Deserialize(stream) as GameData;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error while loading data from file: " + fullPath + "\n" + e);
            }
        }

        return loadData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName + ".xml");

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    private void EncryptFile(string filePath)
    {
        try
        {
            string data = File.ReadAllText(filePath);
            string encryptedData = EncryptDecrypt(data);
            File.WriteAllText(filePath, encryptedData);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while encrypting file: " + filePath + "\n" + e);
        }
    }

    private void DecryptFile(string filePath)
    {
        try
        {
            string data = File.ReadAllText(filePath);
            string decryptedData = EncryptDecrypt(data);
            File.WriteAllText(filePath, decryptedData);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while decrypting file: " + filePath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";

        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        }

        return modifiedData;
    }
}

