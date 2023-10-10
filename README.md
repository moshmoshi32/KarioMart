# Andrew Kart

Created by Mehmet Tanis

Andrew Kart

# Setup
To run Andrew Kart, you can either extract the zip file that is included with this project and run the executable
or you can add it into your Unity list by clicking on Open and then selecting the folder that you downloaded after the project has opened you can navigate to-
the Resources folder to the initalization scriptable object and choose which level you want to start at, and afterwards you can press play and enjoy the game.

The unity version is: 2022.3.8f1

# The Game
The game is a mario kart clone with 2 player being able to race against each other, there are power ups that looks like squares that you can pick up
After a race is finished you have several options to choose from, you can also save your best time in that level by entering your name in an input field

# Game Mechaincs
The game also features power ups which the players can pick up and use, each level is seperate from eachother so that you can easily start at any level without needing to start-
from a specific level, so if you wanted to try out X level, you would set it in the initzilation scriptable object that would then setup everything and
send you to the level that you wanted to start from. It also features a leaderboard that tracks the best 5 players for each level.

Andrew Kart is a 2 player mario kart esque game where the goal is to reach the finish line as fast as possible.
Each level can be configured with different spawn points, check points and laps. You can edit these in the Level Data Scriptable object

# Issues and mechanics I thought was fun to make.
One of the first things I thought about during the project was: how do I make it as easy as possible to go through scenes and test different mechanics?

And what I used was the methods that Sebastian described in his lectures which was Initalize the game before even starting the game using RuntimeInitializeOnLoadMethod attribute
which I used to get data from other scriptable objects to make sure I have everything I need before loading any scene, so if I wanted to go to a level, I would make sure I have-
all of my data for cars and player input manager before entering in that scene, afterwards the game manager would take care of spawning in the players and telling the rest of the-
game to start. 

This was extremely fun and satisfying to work with as I could go to any level through the Editor without any hassle of missing a manager or having to manually setup each scene
to have everything avaliable.

Some of the difficult parts were how I would setup the UI, the UI could be done MUCH better than what I have made right now, as I had a main menu script take care of-
what happens in one set of menus while I had another UIManager take care of the other set of UI which I thought was horribly made however, it does work and it is pretty easy to-
modify to your needs as it is independent from everything else.

Another difficult issue I had was the leaderboard, I had quite a lot of issues with the UI and how to setup the text so that they would form a line from first to last, but then I remembered
about horizontal and vertical layout groups, which worked perfectly in my case as all I needed to do was to sort the list depending on the players time and it worked out great.

# Conclusion
It was a very fun project to work on, it was a bit slow in the end as I got a bit demotivated to work on it as I kept seeing some flaws that would require heavy rewriting to fix
but in the end I felt like I learned a lot by doing this project.



Created by Mehmet Tanis
