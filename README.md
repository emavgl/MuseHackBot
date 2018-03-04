# MuseHackBot

![MuseHackBot Conversation](https://github.com/emavgl/MuseHackBot/raw/master/botesempio2.jpg)

MuseHackBot is a demo of a bot developed using Microsoft Bot Framework for the [OPEN DATA HACKABOT TRENTINO 2018](https://www.odhb2018.net/ ).  
The aim of the bot is to guide a new visitor of the museum [MUSE](http://www.muse.it/it/Pagine/default.aspx) before, during and after the visit.   

MuseHackBot is powered by *Azure Cognitive APIs* in order to extract user's info from his picture (selfie) and perform a sentimental analysis on the user's feedback.

### How to fork
1. Git clone the repo: git clone ```https://github.com/emavgl/MuseHackBot.git```
2. Restore Nuget Packages: ```Update-Package -reinstall``` (or look [here](https://docs.microsoft.com/en-us/nuget/consume-packages/reinstalling-and-updating-packages))
3. Add Keys in ```Web.config```

### Try it
The **Telegram** bot will be online for a while. Click [here](https://telegram.me/@MusaTestBot) to start.

### Usage
Use any message to start the chat.  
If you want to restart and clean all the history, type the command ```/deleteprofile```
