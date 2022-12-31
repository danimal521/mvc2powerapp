using System;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
public class mvc2powerapp
{
    public const int m_nColumnTextBox                                   = 190;
    public const int m_nColumnLabel                                     = 40;
    public const int m_nRow                                             = 80;

    static void Main(string[] args)
    {
        //List<CLabel> acLabel = new List<CLabel>();
        //acLabel.Add(new CLabel { Name = "Label 1", x = 40, y= 80});
        //acLabel.Add(new CLabel { Name = "Label 2", x = 40, y = 160 });
        //acLabel.Add(new CLabel { Name = "Label 3", x = 40, y = 240 });

        //List<CTextbox> acTextbox = new List<CTextbox>();
        //acTextbox.Add(new CTextbox { Name = "Text 1", x = 140, y = 80 });
        //acTextbox.Add(new CTextbox { Name = "Text 2", x = 140, y = 160 });
        //acTextbox.Add(new CTextbox { Name = "Text 3", x = 140, y = 240 });

        //CScreen cs = new CScreen();
        //cs.Name = "Main";
        //cs.Labels = acLabel.ToArray();
        //cs.Textboxs = acTextbox.ToArray();


        CScreen cs = GetScreen(@"C:\Site\mvc\mvc2powerapp\mvc2powerapp\Views\Home\Index.cshtml");


        WriteYaml(cs);



    }

    private static CScreen GetScreen(string FileName)
    {
        CScreen csReturn = new CScreen();

        string strDocument = File.ReadAllText(FileName);

        int nCurentRow = m_nRow;

        int nOver = 0;

        //Look for text boxes
        List<CTextbox> acTextbox = new List<CTextbox>();
        List<CLabel> acLabel = new List<CLabel>();

        do {
            nOver = strDocument.IndexOf("TextBoxFor", nOver);
            if (nOver > -1)
            {
                int nNext = strDocument.IndexOf(".", nOver);
                nNext++; //For the .
                int nEnd = strDocument.IndexOf(")", nNext);
                string strElement = strDocument.Substring(nNext, nEnd - nNext);
                nOver = nEnd;

                acTextbox.Add(new CTextbox { Name = strElement, x = m_nColumnTextBox, y = nCurentRow });
                acLabel.Add(new CLabel { Name = strElement, x = m_nColumnLabel, y = nCurentRow });

                nCurentRow += m_nRow;
            }

        }
        while (nOver > -1);

        string strButton                                                = GetButton(FileName);
        if (strButton != string.Empty)
        {
            CButton cb                                                  = new CButton();
            cb.Name                                                     = strButton;
            cb.x                                                        = m_nColumnLabel;
            cb.y                                                        = nCurentRow;
            csReturn.Button                                             = cb;
        }

        csReturn.Name                                                   = GetTitle(FileName);
        csReturn.Labels                                                 = acLabel.ToArray();
        csReturn.Textboxs                                               = acTextbox.ToArray();

        return csReturn;
    }

    public static string GetTitle(string FileName)
    {
        string strDocument                                              = File.ReadAllText(FileName);
        string strSearch                                                = "ViewData[\"Title\"] = \"";
        int nOver                                                       = strDocument.IndexOf(strSearch);
        nOver                                                           += strSearch.Length;
        int nNext                                                       = strDocument.IndexOf("\";", nOver);
        string strReturn                                                = strDocument.Substring(nOver, nNext - nOver);
        return strReturn.Replace(" ", "");
    }

    public static string GetButton(string FileName)
    {
        string strDocument                                              = File.ReadAllText(FileName);
        string strSearch                                                = "<button type=\"submit\">";
        int nOver                                                       = strDocument.IndexOf(strSearch);
        nOver                                                           += strSearch.Length;
        int nNext                                                       = strDocument.IndexOf("<", nOver);
        string strReturn                                                = strDocument.Substring(nOver, nNext - nOver);
        return strReturn.Replace(" ", "");
    }

    public static void WriteYaml(CScreen cs)
    {
        StreamWriter sw = new StreamWriter(@"C:\\powerapp\\Src\\" + cs.Name + ".fx.yaml");

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
            sw.WriteLine(Space(8) + "ZIndex: =" + nzIndex.ToString());
            nzIndex++;
        }

        //Loop text
        nzIndex = 1;
        foreach (CTextbox cl in cs.Textboxs)
        {
            sw.WriteLine(Space(4) + ParentName() + " As text:");
            sw.WriteLine(Space(8) + "Text: =\"" + cl.Name + "\"");
            sw.WriteLine(Space(8) + "X: =" + cl.x.ToString());
            sw.WriteLine(Space(8) + "Y: =" + cl.y.ToString());
            sw.WriteLine(Space(8) + "ZIndex: =" + nzIndex.ToString());
            nzIndex++;
        }

        //Button
        if (cs.Button != null)
        {
            sw.WriteLine(Space(4) + ParentName() + " As button:");
            sw.WriteLine(Space(8) + "Text: =\"" + cs.Button.Name + "\"");
            sw.WriteLine(Space(8) + "X: =" + cs.Button.x.ToString());
            sw.WriteLine(Space(8) + "Y: =" + cs.Button.y.ToString());
            sw.WriteLine(Space(8) + "ZIndex: = 0" );
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
    public CTextbox[] Textboxs { get; set; }
    public CButton Button { get; set; }
}

public class CLabel : CBase
{ 
}

public class CTextbox : CBase
{
}

public class CButton : CBase
{
}
public class CBase
{
    public string Name { get; set; }
    public int x { get; set; }
    public int y { get; set; }

}