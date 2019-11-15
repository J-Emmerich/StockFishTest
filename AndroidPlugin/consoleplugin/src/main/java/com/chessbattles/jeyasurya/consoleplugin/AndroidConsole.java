package com.chessbattles.jeyasurya.consoleplugin;

import java.io.IOException;

public class AndroidConsole {
    public static String ExecuteCommand(String path){

        try {
            String[] cmd = { "chmod", "744", path };
            Runtime.getRuntime().exec(cmd);
        } catch (IOException e) {
            e.printStackTrace();
            return e.toString();
        }
        return "success";

    }

    public static String test(){
        return "working";
    }
}
