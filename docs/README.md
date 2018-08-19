# BeerSommelierBot
Telegram Beer Sommelier Bot source code and some nifty docs


Altough there's plenty of "create Telegram bot with C# and .NET Core" tutorials, this one is about going all automated.
Assuming you're lazy as hell(like me), you may find this short guide helpful.
I will not cover basics of Telegram bot creation for the reason above. Instead I will give a short guide about hosting it.
In the "docs" folder you'll find everything you need to setup your bot in one click - which will be your PC power button.

Step by step guide:

a) Setting up environment and Telegram bot
There are tons of Telegram C# bot guides, so follow it, create .NET Core project, pump it with couple of methods, 
with the minimal set of two - messaging with Telegram and establishing webhook. And, when all is done, do the following: 

	0) Get any Unix distibutive, which supports systemd daemon services(I used Ubuntu 18.04, 16.04 will be OK too)
	1) Install nginx
	2) Publish and deploy your bot there (I assume, you have chosen righteous path of establishing connection between Telegram and your bot via webhook)
	3) Configure nginx as reverse proxy (using 'proxy_pass http://localhost:<port>' in the selected 'location' block)
	4) Run your bot as service(daemon) by configuring a new daemon, specified here as 'beer-sommelier-tg-bot.service'
	to do this you'll need to run commands in the follwing order:
		sudo systemctl daemon-reload
		systemctl enable beer-sommelier-tg-bot.service
		systemctl start beer-sommelier-tg-bot.service
		
	and then you can check your daemon status by running
		systemctl status beer-sommelier-tg-bot.service
	

b) Proxy (optional)
Then, you will need a proxy,if you're hosting your bot in some Telegram-unfriendly country(e.g. Russia or Iran).
Luckily, Telegram.Bot NuGet https://github.com/TelegramBots/telegram.bot by default supports using a proxy. However,
.NET Core does not have default implementation of IWebProxy, 
and there glorius HttpToSocks5Proxy https://github.com/TelegramBots/Telegram.Bot/wiki/Working-Behind-a-Proxy comes into play 

c) HTTPS tunnel
Telegram does not allow bots to host everywhere - only secure connections allowed. That's what this section is about
HTTPS tunnel will help you to establish connection between your bot and Telegram. 
Here's free (but request-per-minute limited - see free ngrok account limitations) way to do this in like 5 minutes
	0) Register at https://ngrok.com/ , download unix executable, register your auth token
	1) Specify your https tunnel parameters in ngrok.yml - again, example file awaits you in the "docs" folder
	2) Run ngrok as service(daemon) by configuring a new daemon, specified here as 'ngrok.service'
		to do this you'll need to run commands like in bot service

d) Automated to the core
We're too lazy to do any mechanical job every time our system stops, right? In this section we create another daemon,
which will wake up bot-Telegram webhook connection every time system reloads.
	0) Create shell script(.sh), titled 'tg-webhook.sh' which will curl your webhook establishing method ('curl' like HTTP-client, not another one)
	1) Run your .sh as service(daemon) by configuring a new daemon, specified here as 'tg-webhook.service'
	to do this you'll need to run commands like in bot service
	
	
And here it is - your bot is nearly unkillable: it starts up with every system boot and lies dead only when your server is at the same state
Now you can play with daemon configurations and even make your bot auto-recovering (example here is quick and dirty,
of course it should be done with more elegance) - feel free to contribute or answer the questions

As for future of the project, I'll maintain a more complex bot later,
but the most complicated part of connecting all the links of the chain together is (briefly) described above

