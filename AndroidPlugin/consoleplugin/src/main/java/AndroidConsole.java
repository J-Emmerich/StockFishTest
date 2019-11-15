import java.io.IOException;

public class AndroidConsole {

    public static void ExecuteCommand(String command){
        try {
            Runtime.getRuntime().exec(command);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public static String test(){
        return "working";
    }
}
