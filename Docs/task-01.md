*[�����](./../README.md)*  
  
### ������� ��� ����� �1  
  
�������� ���� ���������� � ������ � ���, ������� �� ������������� ��������� ����������������  
  
- ����������� ��������� ����������� � ��������� �����  
- ����������� ��������������� ���������� ����������� � ��������� �����  
- ����������� ������� ���������� ����������� � ��������� ���������� �������  
- ����������� ��������� ������ ����������� ����������� �� ��������� ���������� �������  
  
---
  
### ��������� �� ���������� �������  
  
������ ���������� ������� ��������� ��������� � ������ �������� ����������� �� ������������ ����.  
������ ������ ������� ���� WeatherForecast, � ������� �������� �������� ����������� � ���� � ������� ��� ���� �������������  
  
� ����������� ����������� ��������� ������  

| ���                      | 	�������� |
|--------------------------|----------------|
|**create**                | ������� ������� � ������  
|**read**                  | ������ ��� �������� �� ������  
|**readValue**             | ������ ������� �� ������ �� ������������ ����  
|**readInterval**          | ������ �������� ������ �� ��������� ���������� ���  
|**update**                | �������� ������� � ������ �� ��������� ����  
|**delete**                | ������� ������� �� ������ �� ��������� ����  
|**deleteAll**             | ������� ��� �������� �� ������  
  
---
  
������� �������� � ����������� (����� curl)  
```css  
//�������� �������� � ������  
curl -d -L -X POST "http://localhost:51684/api/weatherforecast/create?date=2020-01-01&temperature=-30"  
curl -d -L -X POST "http://localhost:51684/api/weatherforecast/create?date=2020-01-15&temperature=-25"  
curl -d -L -X POST "http://localhost:51684/api/weatherforecast/create?date=2020-02-02&temperature=-10"  
curl -d -L -X POST "http://localhost:51684/api/weatherforecast/create?date=2020-02-21&temperature=11"  
  
//�������� ���� ��������� ������                      
curl -L -X GET "http://localhost:51684/api/weatherforecast/read"  
  
//�������� �������� �� �������� ���  
curl -L -X GET "http://localhost:51684/api/weatherforecast/readinterval?datefrom=2020-01-15&dateto=2020-02-10"  
  
//�������� �������� �� ��������� ����  
curl -L -X GET "http://localhost:51684/api/weatherforecast/readvalue?date=2020-01-15"  
  
//��������� �������� �� ������������ ����  
curl -d -L -X PUT "http://localhost:51684/api/weatherforecast/update?date=2020-01-15&temperature=-22"  
  
//�������� �������� �� ������������ ����  
curl -d -L -X DELETE "http://localhost:51684/api/weatherforecast/delete?date=2020-01-15"  
  
//�������� ���� ��������� �� ������  
curl -d -L -X DELETE "http://localhost:51684/api/weatherforecast/deleteall"  
```  
  