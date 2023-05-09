# ApiITTP
Реализован API сервис для работы с сущностями User

Добавлена поддержка Docker

Сущности User хранятся в базе MongoDb, которая эмулируется с использованием Docker

Если БД не создана, то при первом запуске, она создается с добавление  в нее User с логином Eugene и паролем 12345 и с правами админа

Добавлена авторизация с использованием JWT токенов

Процесс авторизации:

1. В методе Auth можно получить JWT токен по логину и паролю

2. Скопировать полученный токен , нажать кнопку Authorize

3. Вставить в открывшуюся строку Bearer полученный токен

Например: 

Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkV1Z2VuZSIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTY4MzYyNjc4NSwiZXhwIjoxNjgzNjI3Mzg1LCJpYXQiOjE2ODM2MjY3ODV9.IMvpHhAPDALCr8ZIbtqSkvArO_2zUEbkKm7ukHSrR6OBDORozJ3aa2JY-GiBjKUzCbfi7cjVJm2OUYPsiaJTaw

Для каждго CRUD метода добавлены соответствующие фильтры авторизации

Также для каждого метода добавлена документация с описанием его назначения и параметров

