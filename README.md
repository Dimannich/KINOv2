# KINOv2

## Пути для API
1) /api/film - список всех фильмов
2) /api/film/5 - один фильм подробно
3) /api/film/featured - 4 фильма, на которых продано больше всего билетов
4) /api/session - все сеансы
5) /api/session/?film=1 - все сеансы одного фильма
6) /api/comments/?film=1 - все комментарии к одному фильму

## Авторизация
/api/profile/token/?username=admin&password=supersecretpassword - получение токена доступа. Хэшировать пароль не нужно, положись на HTTPS.

Чтобы запросы стали авторизованные, необходимо к каждому запросу в Headers добавлять такую пару:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoicm9vdCIsIm5iZiI6MTUyNDE3NjQ1MSwiZXhwIjoxNTI0MTgwMDUxLCJpc3MiOiJNeUF1dGhTZXJ2ZXIiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDMwNC8ifQ.IStjW43A0bs_aPOGPm_YqXwuvoeZvuGGGmpLaa3eh0A
```
Длинная строка - access_token
Подробнее о том, как это работает, можно почитать [здесь](https://metanit.com/sharp/aspnet5/23.7.php).
Как это работает для Реакта, можно почитать например [здесь](https://stackoverflow.com/questions/41471979/react-native-set-headers-with-the-linking-api).
### Пути, доступные после авторизации
1) /api/profile/info - информация о профиле
2) /api/profile/history - история заказов
3) /api/film/favorite - избранные фильмы 
4) /api/film/3/comment/?comment=Текст
    Важный момент, пробелы в параметре передать не получится, предлагаю почитать про url encode.
5) /api/comments/12/plus - плюс к комменту
6) /api/comments/12/minus - минус к комменту
7) /api/comments/withauth/ - работает так же как и №6 из первого списка, только добавляется еще одно поле: yourRate - оценка которую текущий пользователь поставил.
## Касса
Порядок действий при оплате с мобильного приложения:
#### Отправка запроса
```
POST: http://192.168.43.153/Home/CreateOrderMobile
```
Параметры запроса:
- session-cost - общая цена заказа
- session-link - link сеанса
- \_\_RequestVerificationToken - токен
- для каждого заказываемого места отправляется параметр: 
  - название: ticket-row2-number9, где цифры - ряд и место
  - значение: ряд\*1000 + место

Все эти параметры передаются не в параметрах запроса, а в form-data. В Postman это можно сделать следующим образом:
https://pp.userapi.com/c844724/v844724993/5827a/Mn6suyU_gJw.jpg

Нужны так же Headers:
```
Authorization: Bearer <token>
Content-Type: multipart/form-data
```

В ответ приходит html-страница, которую надо целиком запихать в WebView.
#### Работа в WebView
Страница, которая пришла в предыдущем пункте - страница платежного шлюза. По шагам надо сделать оплату, в конце, при успешной и неуспешной оплате будет кнопка "вернуться в магазин". По нажатию на нее, пользователя вернет на наш сайт. В этот момент надо словить событие OnMessage ("success" и "fail" для успешной и неуспешной оплаты соответсвенно) и закрыть WebView и перекинуть пользователя обратно в приложение. Желательно, сделать кнопку "Отменить" в верхнем баре, чтобы пользователь мог выйти обратно в приложение, даже если он проебал кнопку "вернуться в магазин".

### Правки
1) Отправка рейтинга

## Картинки для постеров
соотношение сторон должно быть 3:2
