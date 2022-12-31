using System;
using System.Net;
using System.Xml.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
public class mvc2powerapp
{
    static void Main(string[] args)
    {
        List<CLabel> acLabel = new List<CLabel>();

        acLabel.Add(new CLabel { Name = "Label 1", x = 40, y= 80});
        acLabel.Add(new CLabel { Name = "Label 2", x = 40, y = 100 });
        acLabel.Add(new CLabel { Name = "Label 3", x = 40, y = 120 });

        CScreen cs = new CScreen();
        cs.Name = "Main";
        cs.Labels = acLabel.ToArray();


        WriteYaml(cs);



    }

    public static void WriteYaml(CScreen cs)
    {

        StreamWriter sw = new StreamWriter(@"C:\\powerapp\\Src\\Screen3.fx.yaml");
        
        //Write header
        sw.WriteLine(cs.Name + " As screen:");

        //Loop labels
        int nzIndex = 1;
        foreach(CLabel cl in cs.Labels) 
        {
            sw.WriteLine(Space(4) + ParentName() + " As label:");
            sw.WriteLine(Space(8) + "Text: =\""+cl.Name+"\"");
            sw.WriteLine(Space(8) + "X: =" + cl.x.ToString() );
            sw.WriteLine(Space(8) + "Y: =" + cl.y.ToString());

        }
        
        sw.Close();

    }

    public static string Space(int Count)
    {
        string strReturn = string.Empty;
        for(int nIndex = 0;nIndex < Count;nIndex++) 
        {
            strReturn += " ";
        }
        return strReturn;
    }
    public static string ParentName()
    {
        return Guid.NewGuid().ToString().Replace("-", "");       
    }
}

public class CScreen
{
    public string Name { get; set; }
    public CLabel[] Labels { get; set; }
}

public class CLabel
{
     public string Name { get; set; }
    public int x { get; set; }
    public int y { get; set; }

}