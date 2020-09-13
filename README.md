# Incohearent
Incohearent - Xamarin/C# Application  
Server: https://github.com/josipamrsa/IncohearentServer

From the Amazon page: "Incohearent is the adult party game that will get you talking! Let the laughs begin as you compete to make sense out of gibberish from one of three categories — kinky, party and pop culture."  
This is a C#/Xamarin project which is a mobile game app inspired by the card game. 

## Technologies
* DB/Storage
  * SQLite
* Backend
  * Azure
  * ASP.NET (C#)
  * SignalR
* Frontend
  * XamarinForms

## Installation (for testing purposes)

* Download the Incohearent server (link above) and run it locally or on your own server
* Download the source code for this app and open it (Visual Studio 2019)
* Make sure to pick a server configuration in app settings (preferably setup the Azure server)
  * All app settings are located in file Constants.cs
* You can debug on a physical device or emulator, or just export the APK file  

## How to play

Make sure your group is using the same WiFi network, otherwise you will not be able to see
players in your lobby. Log into the app using whichever name you want. Once all your players
are logged in, only one player should start the game (bug currently). The person that started
the game will take on a role of judge (original phrase will be displayed to him), and the rest will be earning points depending on their
answers (guessing the scrambled phrase). The game continues for as long as you want. If you wish to end the game, the judge
should stop the game.


## Notes (remove later)
>> 7.6.2020. - Implementation of general network connection check  
>> 8.6.2020. - Testing general network connection, UI changes, basic controllers for User Database  
>> 13.6.2020. - General functionality descriptions and outline for views  
>> 27.6.2020. - IP addresses check, started on SQLite  
>> 28.6.2020. - SQLite database connection successful, Login now checks if user is connected to a WiFi network  
>> 6.7.2020. - Login rewritten into MVVM model, currently adds user, next up is lobby creation/assignment  
>> 10.7.2020. - Web server, lobby built with SignalR - basic functions of joining/leaving lobby, sign out  
>> 11.7.2020. - Lobby/application exit/enter, tests  
>> 13.7.2020. - Phrase generator (server)  
>> 14.7.2020. - Session screen started  
>> 17.7.2020. - Phrase display on session screen  
>> 20.7.2020. - Phrase display for GameMaster and other players, UI changes  
>> 22.7.2020. - Dynamically created elements for each player - GameMaster  
>> 25.7.2020. - Game logic - GameMaster/player interaction, simple point system and timer  
>> 26.7.2020. - Game logic - timer clock now controls the game flow  
>> 27.7.2020. - Game logic - end session, show statistic for other players, next up extensive testing and finishing up  
>> 28.7.2020. - UI modifications on all pages   
>> 29.7.2020. - Limitations and checks - setting the minimum player count, checking for network connection  
>> 30.7.2020. - General testing, code cleanup, code commenting
