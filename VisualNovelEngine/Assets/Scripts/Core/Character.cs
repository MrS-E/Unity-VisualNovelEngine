using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public string shortname;
    public string fullname;
    public Color color;
    public string sideImage;

    public Character(string shortname, string fullname, Color color, string sideImage)
    {
        this.shortname = shortname;
        this.fullname = fullname;
        this.color = color;
        this.sideImage = sideImage;
        check();
    }

    public Character(string shortname, string fullname, Color color)
    {
        this.shortname = shortname;
        this.fullname = fullname;
        this.color = color;
        this.sideImage = null;
        check();
    }

    public Character(string shortname, string fullname)
    {
        this.shortname = shortname;
        this.fullname = fullname;
        this.color = Color.white;
        this.sideImage = null;
        check();
    }

    private void check()
    {
        if (this.shortname == null || this.fullname == null)
        {
            throw new InvalidePropertyException("short- and fullname must containe string");
        }
    }
}
