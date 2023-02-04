using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;

public class InputDecoder
{
    #region Variabels
    public static bool PausedHere  = false;
    public static bool atQuestion = false;

    //Character
    public static List<Character> CharacterList = new List<Character>();
    private static GameObject ImageCharacter = Resources.Load("Prefabs/CharacterSideImage") as GameObject;
    private static GameObject characterSide = GameObject.Find("CharacterImage");


    //Command Storage
    [NonSerialized]
    public static List<string> Commands = new List<string>();
    public static List<Label> LabelList = new List<Label>();
    public static int CommandLine = 0;
    public static string lastCommand = "";

    //find and define UIElements
    public static GameObject InterfaceElements = GameObject.Find("UI_Elements");

    //find and define the background image
    private static GameObject canvas = GameObject.Find("ImageLayers");
    private static GameObject ImageBackground = Resources.Load("Prefabs/Background") as GameObject;

    //find and define text fields
    public static GameObject DialogText = GameObject.Find("DialogueText");
    public static GameObject NameText = GameObject.Find("NameText");
    public static GameObject QuestionText = GameObject.Find("QuestionBox");
    private static GameObject ChooseText = Resources.Load("Prefabs/ChooseText") as GameObject;
    #endregion

    public static void ParseInputLine(string command) //TODO restructure
    {
        command = command.Replace("\t", "");
        if (command.StartsWith("\""))
        {
            Say(command);
        }
        else
        {
            string[] SeperatingString = { " ", "\'", "\"", "(", ")", ":" };
            string[] args = command.Split(SeperatingString, StringSplitOptions.RemoveEmptyEntries);

            foreach (Character character in CharacterList)
            {
                if (args[0] == character.shortname || args[0] == character.fullname)
                {
                    Say(character.fullname, SplitToSay(command, character), character.color);
                }
            }
            if (args[0] == "show")
            {
                showImage(command);
            }
            else if (args[0] == "clrscr")
            {
                clearScreen(null, true);
            }
            else if (args[0] == "character")
            {
                createNewCharacter(command);
            }
            else if (args[0] == "jump")
            {
                JumpTo(command);
            }
            else if (args[0] == "appear")
            {
                AppearCharacter(command);
            }
            else if (args[0] == "disappear")
            {
                DisappearCharacter(command);
            }
            else if (args[0] == "end")
            {
                Application.Quit();
                //UnityEditor.EditorApplication.isPlaying = false;
            }
            else if (args[0] == "question")
            {
                Question(command);
            }
        }
    }

    #region Say

    public static string SplitToSay(string say, Character character)
    {
        int startQuote = say.IndexOf("\"") + 1;
        int endQuote = say.Length - 1;

        return say.Substring(startQuote, endQuote - startQuote);
    }

    public static void Say(string say)
    {
        if(!InterfaceElements.activeInHierarchy) InterfaceElements.SetActive(true);
        DialogText.GetComponent<TextMeshProUGUI>().text = say.Substring(1, say.Length - 2);
        NameText.GetComponent<TextMeshProUGUI>().text = "";
        PausedHere = true;
    }

    public static void Say(string who, string what, Color color)
    {
        if (!InterfaceElements.activeInHierarchy) InterfaceElements.SetActive(true);
        DialogText.GetComponent<TextMeshProUGUI>().text = what.Substring(0, what.Length );
        NameText.GetComponent<TextMeshProUGUI>().text = who;
        NameText.GetComponent<TextMeshProUGUI>().color = color;
        PausedHere = true;
    }

    public static void Question(string command)
    {
        DialogText.GetComponent<TextMeshProUGUI>().text = "";
        string[] questions = command.Replace("question ", "").Split(';')[0].Split('~');
        List<GameObject> qs = new List<GameObject>();
        for(int x = 0; x<questions.Length; x++)
        {
            string[] q1 = questions[x].Split(":");
            GameObject PictureInstance = GameObject.Instantiate(ChooseText);
            PictureInstance.transform.SetParent(QuestionText.transform, false);
            PictureInstance.GetComponent<ChooseText>().key = (x+1).ToString();
            PictureInstance.GetComponent<ChooseText>().text = q1[0];
            PictureInstance.GetComponent<ChooseText>().command = q1[1];
            qs.Add(PictureInstance);
            atQuestion = true;
        }
        foreach(GameObject g in qs)
        {
            g.GetComponent<ChooseText>().allQuestions = qs;
        }
    }

    #endregion

    #region Images

    public static void showImage(string image) //TODO restructure
    {
        string ImageToShow = null;
        bool fadeEffect = false;
        bool doClear = false;
        var ImageToUse = new Regex(@"show (?<ImageFileName>[^.]+)");
        var ImageToUseTransition = new Regex(@"show (?<ImageFileName>[^.]+) with (?<TransitionName>[^.]+)");
        var ImageToUseTransitionToDoScreenClear = new Regex(@"show (?<ImageFileName>[^.]+) with (?<TransitionName>[^.]+) do (?<ScreenName>[^.]+)");

        var matches = ImageToUse.Match(image);
        var altMatches = ImageToUseTransition.Match(image);
        var firstMatches = ImageToUseTransitionToDoScreenClear.Match(image);
        if (firstMatches.Success)
        {
            ImageToShow = altMatches.Groups["ImageFileName"].ToString();
            fadeEffect = true;
            doClear= true;
        }
        else if (altMatches.Success) 
        { 
            ImageToShow = altMatches.Groups["ImageFileName"].ToString();
            fadeEffect= true;
        }
        else if(matches.Success)
        {
            ImageToShow = matches.Groups["ImageFileName"].ToString();
        }

        GameObject PictureInstance = GameObject.Instantiate(ImageBackground);
        PictureInstance.transform.SetParent(canvas.transform, false);
        PictureInstance.GetComponent<ImageInstance>().FadeIn = fadeEffect;
        PictureInstance.GetComponent<Image>().color = Color.white;
        PictureInstance.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/" + ImageToShow);
        if (doClear)
        {
            clearScreen(PictureInstance, false);
        }
    }

    public static void clearScreen(GameObject image, bool removeAll)
    {
        if (removeAll == true)
        {
            foreach (Transform t in canvas.transform)
            {
                MonoBehaviour.Destroy(t.gameObject);
            }
            foreach (Transform t in characterSide.transform)
            {
                MonoBehaviour.Destroy(t.gameObject); 
            }
            InterfaceElements.SetActive(false);
        }
        else
        {
            foreach (Transform t in canvas.transform)
            {
                if (image != null && t != image.transform)
                {
                    MonoBehaviour.Destroy(t.gameObject, 1f);
                }
            }
        }
    }

    #endregion

    #region Character

    public static void createNewCharacter(string command) //TODO restructure
    {
        string shortname = null;
        string fullname = null;
        Color color = Color.white;
        string sideImage = null;

        var character = new Regex(@"character\((?<shortName>[a-zA-Z0-9_]+), (?<fullName>[a-zA-Z0-9_]+), color=(?<color>[a-zA-Z0-9_]+), image=(?<sideImage>[a-zA-Z0-9_]+)\)");
        var characterA = new Regex(@"character\((?<shortName>[a-zA-Z0-9_]+), (?<fullName>[a-zA-Z0-9_]+), color=(?<color>[a-zA-Z0-9_]+)\)");
        var characterB = new Regex(@"character\((?<shortName>[a-zA-Z0-9_]+), (?<fullName>[a-zA-Z0-9_]+)\)");
        var characterC = new Regex(@"character\((?<shortName>[a-zA-Z0-9_]+), (?<fullName>[a-zA-Z0-9_]+), image=(?<sideImage>[a-zA-Z0-9_]+)\)");

        if(character.IsMatch(command))
        {
            var matches = character.Match(command);
            shortname = matches.Groups["shortName"].ToString();
            fullname = matches.Groups["fullName"].ToString();
            color = Color.clear; 
            ColorUtility.TryParseHtmlString(matches.Groups["color"].ToString(), out color);
            sideImage = matches.Groups["sideImage"].ToString();

        }
        else if(characterA.IsMatch(command))
        {
            var matches = characterA.Match(command);
            shortname = matches.Groups["shortName"].ToString();
            fullname = matches.Groups["fullName"].ToString();
            color = Color.clear;
            ColorUtility.TryParseHtmlString(matches.Groups["color"].ToString(), out color);
        }
        else if (characterB.IsMatch(command))
        {
            var matches = characterB.Match(command);
            shortname = matches.Groups["shortName"].ToString();
            fullname = matches.Groups["fullName"].ToString();
        }
        else if (characterC.IsMatch(command))
        {
            var matches = characterC.Match(command);
            shortname = matches.Groups["shortName"].ToString();
            fullname = matches.Groups["fullName"].ToString();
            sideImage = matches.Groups["sideImage"].ToString();
        }

        CharacterList.Add(new Character(shortname, fullname, color, sideImage));
    }

    public static void AppearCharacter(string command)
    {
        var characterCheck = new Regex(@"appear (?<Name>[a-zA-Z0-9_]+)");
        var characterCheckA = new Regex(@"appear (?<Name>[a-zA-Z0-9_]+), (?<Position>[a-zA-Z]+)");


        if (characterCheckA.IsMatch(command))
        {
            foreach(Character c in CharacterList)
            {
                if(c.fullname == characterCheckA.Match(command).Groups["Name"].ToString() || c.shortname == characterCheckA.Match(command).Groups["Name"].ToString())
                {
                    GameObject PictureInstance = GameObject.Instantiate(ImageCharacter);
                    PictureInstance.transform.SetParent(characterSide.transform, false);
                    PictureInstance.GetComponent<Image>().color = c.color;
                    PictureInstance.GetComponent<Image>().name = c.fullname;
                    if(c.sideImage != null)
                    {
                        PictureInstance.GetComponent<Image>().color = Color.white;
                        PictureInstance.GetComponent<CharacterSideImage>().size = new Vector2(Resources.Load<Sprite>("Images/Character/" + c.sideImage).rect.width, Resources.Load<Sprite>("Images/Character/" + c.sideImage).rect.height);
                        PictureInstance.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Character/" + c.sideImage);
                        if (characterCheckA.IsMatch(command)){
                            PictureInstance.GetComponent<CharacterSideImage>().position = characterCheckA.Match(command).Groups["Position"].ToString();
                        }
                    }
                }
            }
        }
        else if(characterCheck.IsMatch(command))
        {
            foreach (Character c in CharacterList)
            {
                if (c.fullname == characterCheck.Match(command).Groups["Name"].ToString() || c.shortname == characterCheck.Match(command).Groups["Name"].ToString())
                {
                    GameObject PictureInstance = GameObject.Instantiate(ImageCharacter);
                    PictureInstance.transform.SetParent(characterSide.transform, false);
                    PictureInstance.GetComponent<Image>().color = c.color;
                    PictureInstance.GetComponent<Image>().name = c.fullname;
                    if (c.sideImage != null)
                    {
                        PictureInstance.GetComponent<Image>().color = Color.white;
                        PictureInstance.GetComponent<CharacterSideImage>().size = new Vector2(Resources.Load<Sprite>("Images/Character/" + c.sideImage).rect.width, Resources.Load<Sprite>("Images/Character/" + c.sideImage).rect.height);
                        PictureInstance.GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/Character/" + c.sideImage);
                    }
                }
            }
        }
    }

    public static void DisappearCharacter(string command)
    {
        var character = new Regex(@"disappear (?<Name>[a-zA-Z0-9_]+)");
        if (character.IsMatch(command)) 
        {
            foreach (Transform t in characterSide.transform)
            {
                if (t.gameObject.name == character.Match(command).Groups["Name"].ToString())
                {
                    t.GetComponent<CharacterSideImage>().KillMe();
                    MonoBehaviour.Destroy(t.gameObject, 2f);
                    break;
                }
            }
        }
    }
    #endregion

    #region Labels

    public static void ReadScript(string file)
    {
        TextAsset commandFile = Resources.Load(file) as TextAsset;
        var commandArray = commandFile.text.Replace("\r", "").Split("\n");

        foreach(string line in commandArray)
        {
            Commands.Add(line);
        }

        for ( int x=0; x< Commands.Count; x++ )
        {
            if (Commands[x].StartsWith("label"))
            {
                var labelSplit = Commands[x].Split(' ');
                LabelList.Add(new Label(labelSplit[1], x));
            }
        }
    }

    public static void JumpTo(string line) 
    {
        var tempString = line.Split(' ');
        foreach(Label d in LabelList) 
        {
            if(d.LabelName == tempString[1])
            {
                CommandLine = d.LabelIndex;
                PausedHere = false;
            }
        }
    }

    #endregion
}
