using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;

public class LogsManager
{

#if UNITY_EDITOR
    string path = "Assets/Resources/LogFile";
#elif UNITY_STANDALONE
    string path = "MyGame_Data/Resources/LogFile"
#endif

    /// <summary>
    /// Save the current commands into a file, so it can be retreived later
    /// </summary>
    /// <param name="logs"> the raw string of the logs</param>
    public void SaveLogs(string logs)
    {

        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(logs);
            }
        }
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    /// <summary>
    /// Load the logs, expensive function
    /// </summary>
    /// <returns>List of Commands done previously</returns>
    public List<ICommand> LoadLogs()
    {
        List<ICommand> commands = new List<ICommand>() ;

        
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            using (StreamReader reader = new StreamReader(fs))
            {
                string commandString;
                string[] stringArray;
                string classString;
                float ts;
                IEnumerable<String> paramsEnum;

                while (reader.Peek() >= 0)
                {
                    commandString = reader.ReadLine();

                    //character $ used to separate 
                    stringArray = commandString.Split('$');

                    //needs at least to have the name and the ts
                    if (stringArray.Length > 1)
                    {
                        classString = stringArray[0];
                        if (float.TryParse(stringArray[1], out ts))
                        {
                            Type type = Type.GetType(classString);

                            paramsEnum = stringArray.Skip(2);
                            foreach(string str in paramsEnum)
                            {
                                str.Replace("$", "");
                            }
                            //check if it implements ICommand
                            if (type.GetInterfaces().Any(x => x == typeof(ICommand)))
                            {
                                if(paramsEnum.Count() > 0)
                                    commands.Add((ICommand)Activator.CreateInstance(type, ts, paramsEnum.ToArray()));
                                else
                                    commands.Add((ICommand)Activator.CreateInstance(type, ts));
                            }
                        }
                    }
                }
            }
        }

            return commands;
    }
}
