# Unity-VisualNovelEngine
> This Projet is based on a Guide of Game Developer Training. The Guilde can be found under https://youtu.be/Yvo90A-LWuQ?list=PLKdE0Vv4UA59pfcfktuO6tLjmK4W8gaC9.

## To use
Everything is written under `Assets/Resources/Script/script.txt`. Images and other assets are under `Assets/Resources/`. Backgrounds and other images are under `/Images` and character pictures are at `/Images/Character`.

## Commands
### Create Character
Creates a Character to use for the story. <br>
`character(shortname, fullname, color=color, image=asset_01)`<br>
*shortname*: A internal name to make the commands shorter. <br>
*fullname*: The name witch is displayed can also used in some commands. <br>
*color*: Html color, membervariable of class **Color**. <br>
*asset_01*: Any image in the character folder at **/Images/Character**.

### Background Image
This sets the background of the story. It can be changed at every point of time. <br>
`show bg_01 [with Fade [do clear]]` <br>
*bg_01*: Any image with serves as background. It can be found and placed under **/Images**. <br>
*width Fade*: This is an optional attribute witch lets fade the background slowly (over one second) in. <br>
*do clear*: This is an optional attributes witch removes all unused backgrounds to safe RAM. <br>

### Display Text
This sets a text in the dialog field. <br>
`[shortname/fullname] "some text"` <br>
*shortname/fullname*: This is an optional attributes witch specified from who the text comes. If the character is not defined, none will be displayed. <br>
*some text*: This text is the text witch is displayed. <br>

### Appear Champion
This little command lets the image of a character appear. <br>
`appear shortname/fullname[, direction]` <br>
*shortname/fullname*: To specify from who the appears. <br>
*direction*: This is an optional attributes witch defines on what side the character is displayed. It can be sat to **left**,**right**,**center**. The default value is **center**. It's used if nothing or something other is defined. <br>

### Disappear Champion
This little command lets the image of a character disappears again after it's appeared. <br>
`disappear shortname/fullname` <br>
*shortname/fullname*: To specify from who the disappears. <br>

### Question
A question can be used to let the user deside how the story will go further. <br>
`question "question":command,"question":command;` <br>
*question*: The question witch is displayed.<br>
*command*: The command witch is executed if the question is selected.<br>
You can add as many questions as you wish, but it is not advised to use more than four.

### Label
A label is an important component. It's main purpose is to set a point into the story and jumps to them to switch between storylines. <br>
`label name` <br>
`jump name` <br>
*name*: Sets a name for **label**. If the same name is used by **jump** the code jumps to this line.
  
### Clear Screen
This clears the screen completely. <br>
`clrscr` <br>
 
### Quite
This ends the game and closes the application. Because of an bug please keep an empty line of code at the bottom. <br>
`end` <br>
