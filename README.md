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
4) /api/film/3/favorite - проверка является ли фильм избранным
5) /api/film/3/favorite/add -добавить в избранное
6) /api/film/3/favorite/remove -удалить из избранного

## Картинки для постеров
соотношение сторон должно быть 3:2
