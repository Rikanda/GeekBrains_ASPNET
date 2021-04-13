*[�����](./../README.md)*  
  
### ������� ��� ����� �5  
  
- [X] 1	���������� ������ ��� ����� ������ RAM, HDD � gc-heap-size. 
�������� ��� � ������� perfomance-counters. ������ ������ �������� ������� ��� � 5 ������ � ���������� �� � ��������� ���� ������  
- [X] 2	���������� �����������, ������� ����� �������� ������ �� ���������� ��������  
- [X] 3	������������ FluentMigrator 
  
---  
  
### ��������� �� ���������� �������  
  
�������� ��������� ���� ������ ���� ������ �� ������ startup.cs � ����������� ����� FluentMigrator.  
  
���� ���������� ���������� Quartz � ��������� ������ �� ����� ������ �� �������.  
  
����� �������� ��������� �������:  

| ��������     | 	���������        | ��� ��������         | ��������� |  
|--------------|-------------------|----------------------|-----------|  
| **Cpu**      | Processor         | % Processor Time     | _Total  
| **DotNet**   | .NET CLR Memory   | # Bytes in all Heaps | _Global _  
| **Hdd**      | LogicalDisk       | Free Megabytes       | _Total  
| **Network**  | Network Interface | Bytes Received/sec   | All  
| **Ram**      | Memory            | Available MBytes     |  
  
� ������������ ��� ������ ����� Create ��� ������ ��������� ������ � ���� ������.  
  
---  
  
� ������������ ������ ������ ������� ����� �������� � ������� ������� �� ��������� ������� ��� Hdd � Ram ������������.  
��� �� ������ ��� ������� �������������� ����� request ����������.  
�� ���� �������� � DTO ����� ������������� �������� ID, �� �������������.  
  
������� �������� � ������������ ������� MetricsAgent (����� curl)  
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
  
�������������, ��� ���������� �� ������������� ����� � SQL ��������, 
��� ������ ��������� IMySqlSettings.cs � ��������������� �������, 
� ������� �������� ��� ������ � ���� ������ (������ �����������, ����� ������ � �����).  
������ � ������ ���������� ����� �������� � �����������.  ������� � ������������ ������ ��������������� enum'� (Tables � Rows)  
������ �������� � ������������ ��������� �� ��� �����������, � ��� �� � �������� ���� ������.  
  
������ SQL ������ �� �����������:  
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
  
�������������.  
����� ������ � �������������� ����������� �� CpuMetricsController ������ ������, �.�. ��� ������������ ����������� � ��������� ������.  
  
