*[Назад](./../README.md)*  
  
### Задание для урока №5  
  
- [X] 1	Реализуйте задачи для сбора метрик RAM, HDD и gc-heap-size. 
Сделайте это с помощью perfomance-counters. Задачи должны собирать метрики раз в 5 секунд и записывать их в созданную базу данных  
- [X] 2	Реализуйте контроллеры, которые будут отдавать данные по собираемым метрикам  
- [X] 3	Использовать FluentMigrator 
  
---  
  
### Пояснения по выполнению задания  
  
Создание структуры базы данных было убрано из класса startup.cs и реализовано через FluentMigrator.  
  
Была подключена библиотека Quartz и добавлены задачи по сбору метрик по времени.  
  
Агент собирает следующие метрики:  

| Название     | 	Категория        | Имя счетчика         | Экземпляр |  
|--------------|-------------------|----------------------|-----------|  
| **Cpu**      | Processor         | % Processor Time     | _Total  
| **DotNet**   | .NET CLR Memory   | # Bytes in all Heaps | _Global _  
| **Hdd**      | LogicalDisk       | Free Megabytes       | _Total  
| **Network**  | Network Interface | Bytes Received/sec   | All  
| **Ram**      | Memory            | Available MBytes     |  
  
В репозиториях был создан метод Create для записи собранных метрик в базу данных.  
  
---  
  
В контроллерах Агента метрик изменил имена запросов и добавил запросы по интервалу времени для Hdd и Ram контроллеров.  
Так же теперь все запросы осуществляются через request контейнеры.  
Во всех метриках и DTO убрал использование свойства ID, за ненадобностью.  
  
Примеры запросов к контроллерам проекта MetricsAgent (через curl)  
```css  
curl -k -L -X GET "https://localhost:5001/api/metrics/cpu/from/2021-01-01/to/2021-04-12"  
  
curl -k -L -X GET "https://localhost:5001/api/metrics/dotnet/from/2021-01-01/to/2021-04-12"  
  
curl -k -L -X GET "https://localhost:5001/api/metrics/hdd/from/2021-01-01/to/2021-04-12"  
curl -k -L -X GET "https://localhost:5001/api/metrics/hdd/left"  
  
curl -k -L -X GET "https://localhost:5001/api/metrics/network/from/2021-01-01/to/2021-04-12"  
  
curl -k -L -X GET "https://localhost:5001/api/metrics/ram/from/2021-01-01/to/2021-04-12"  
curl -k -L -X GET "https://localhost:5001/api/metrics/ram/available"  
```  
  
---  
  
Дополнительно, для избавления от повторяющихся строк в SQL командах, 
был создан интерфейс IMySqlSettings.cs с соответствующим классом, 
в котором хранятся все данные о базе данных (строка подключения, имена таблиц и рядов).  
Доступ к данным реализован через свойства и индексаторы.  Ключами в индексаторах служат соответствующие enum'ы (Tables и Rows)  
Сделал синглтон и инъектировал интерфейс во все репозитории, а так же в миграцию базы данных.  
  
Пример SQL вызова из репозитория:  
```cs  
	connection.Execute(  
	$"INSERT INTO {mySql[Tables.CpuMetric]}" +  
	$"({mySql[Rows.Value]}, {mySql[Rows.Time]})" +  
	$"VALUES (@value, @time);",  
	new  
	{  
		value = metric.Value,  
		time = metric.Time.TotalSeconds,  
	});  
```  
  
---  
  
Дополнительно.  
Убрал запрос с использованием перцентилей из CpuMetricsController Агента метрик, т.к. его исплозование планируется в Менеджере метрик.  
  
