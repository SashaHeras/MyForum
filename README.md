# MyForum

Это форум сделан по техническому заданию.

Описание к тому как запустить сайт локально.

1. Откройте MS SQL Server Managment:
	1.1. Откройте в нём папку MyForumScripts, что находиться в папке проекта;
	1.2. Откройте все скрипты для создания БД:
		1.2.1. Запустите все скрипты в последовательности:
			- DB.sql(перед запуском этого скрипта откройте его и измените путь к папке в который будет сохранена база данных);
			- User.sql;
			- Topic.sql;
			- Post.sql;
			- Mark.sql;
			- Comment.sql;
			- TopicSeed.sql;
2. Откройте проект в Visual Studio 2022;
	2.1. После открытия проекта, проверьте наличие установленых NuGet пакетов:
		- Microsoft.AspNet.Mvc;
		- Microsoft.AspNet.Mvc;
		- Microsoft.AspNetCore.Session;
		- Microsoft.EntityFrameworkCore;
		- Microsoft.EntityFrameworkCore.SqlServer;
		- Microsoft.EntityFrameworkCore.InMemory;
		- Microsoft.EntityFrameworkCore.Proxies;
		- Microsoft.EntityFrameworkCore.SqlServer.Design;
		- Microsoft.EntityFrameworkCore.Tools;
		- EntityFramework(v6.4.4 или всё что выше v6);
	2.2. После установки пакетов, зайдите в appsettings.json, там найдите строку "DefaultConnection": "Server=DESKTOP-OG202L1;Database=ForumUa;Trusted_Connection=True;"
вместо DESKTOP-OG202L1 вставьте название своего сервера(название ПК);
3. После этого сохраните изменения и попробуйте запустить. 
4. НАПОМИНАНИЕ!!! У Вас там не будет акаунта так что прийдёться нажать кнопку "Registration" или "You haven`t account?" чтобы перейти на страничку регистрации;
5. После регистрации Вы попадаете на страницу входа, вход осущиствляется с помощью почты и пароля;
	
