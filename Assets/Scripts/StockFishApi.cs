using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Diagnostics;

public class StockFishApi : MonoBehaviour
{

    public Text OutPutText;

    string outPut = "";
    public static Process mProcess;

    [SerializeField] InputField FENInput;
    [SerializeField] InputField DepthInput;
    [SerializeField] InputField TimeInput;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Setup();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        OutPutText.text = outPut;
    }

    public void Setup()
    {
        // since the apk file is archived this code retreives the stockfish binary data and
        // creates a copy of it in the persistantdatapath location.
        #if UNITY_ANDROID
        string filepath = Application.persistentDataPath + "/" + "stockfish-10-armv7";
        if (!File.Exists(filepath))
        {
            WWW executable = new WWW("jar:file://" + Application.dataPath + "!/assets/" + "stockfish-10-armv7");
            while (!executable.isDone)
            {
            }
            File.WriteAllBytes(filepath, executable.bytes);

            //change permissions via plugin
           
        }
        var plugin = new AndroidJavaClass("com.chessbattles.jeyasurya.consoleplugin.AndroidConsole");
            string command = "chmod 777 "+filepath;
            outPut = plugin.CallStatic<string>("ExecuteCommand",command);
            
        #else
        string filepath = Application.streamingAssetsPath+ "/" + "stockfish_10_x64.exe";
        #endif
        // creating the process and communicating with the engine
        mProcess = new Process();
        ProcessStartInfo si = new ProcessStartInfo()
        {
            FileName = filepath,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        };
        mProcess.StartInfo = si;
        mProcess.OutputDataReceived += new DataReceivedEventHandler(MProcess_OutputDataReceived);
        mProcess.Start();
        mProcess.BeginErrorReadLine();
        mProcess.BeginOutputReadLine();

        SendLine("uci");
        SendLine("isready");

    }

    public void GetMove(){
        string Fen = FENInput.text;
        string DepthValue = DepthInput.text;
        string processTime = TimeInput.text;

        if(Fen==null || Fen == ""){
            UnityEngine.Debug.LogError("Enter proper Fen");
            outPut = "Enter proper Fen";
            return;
        }

        SendLine("position fen "+Fen);

        if(processTime != ""){
            SendLine("go movetime "+processTime);
        }
        else{
            SendLine("go depth "+DepthValue);
        }

    }


    public void SendLine(string command) {
        mProcess.StandardInput.WriteLine(command);
        mProcess.StandardInput.Flush();
    }

    void MProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {

        UnityEngine.Debug.Log("Output:"+e.Data);

        outPut = e.Data;
        
        
    }
}
