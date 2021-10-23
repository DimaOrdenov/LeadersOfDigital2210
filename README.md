# SelfTrip

SelfTrip - мобильное приложение для путешественников

Приложение позволяет построить маршрут путешествия исходя из бюджета, участников группы и других предпочтений пользователя, закрывая недостатки пакетных туров\
На время путешествия предлагается программа развлечений (посещение музеев, достопримечательностей) и питания

Стек решения: Backend - .Net, docker. Frontend - Xamarin.Native\
Уникальность: Подбор на основе предпочтений группы путешественников, гибкая кастомизация, бизнес-аккаунты для планирования командировок\
Стоимость и сроки внедрения: пилотная версия - 6 мес./3млн, полная версия - 10 мес./5млн.

На стадии прототипа внедрен следующий функционал:
- Выбор маршрута 
- Отображение маршрута на карте внутри приложения и переход в стороннее приложение
- Выбор транспорта
- Построение маршрута с учетом: группы, бюджета, вида транспорта
- Выбор билетов на самолет (интеграция с Aviasales API), переход на покупку билетов
- Подбор и выбор отеля (интеграция с Hotellook.ru)


Анализ:\
Статистика Ростуризма\
<a href="https://tourism.gov.ru/upload/iblock/5e0/%D0%A1%D1%82%D0%B0%D1%82%D0%B8%D1%81%D1%82%D0%B8%D1%87%D0%B5%D1%81%D0%BA%D0%B8%D0%B5%20%D0%BF%D0%BE%D0%BA%D0%B0%D0%B7%D0%B0%D1%82%D0%B5%D0%BB%D0%B8,%20%D1%85%D0%B0%D1%80%D0%B0%D0%BA%D1%82%D0%B5%D1%80%D0%B8%D0%B7%D1%83%D1%8E%D1%89%D0%B8%D0%B5%20%D1%82%D1%83%D1%80%D0%B8%D1%81%D1%82%D1%81%D0%BA%D1%83%D1%8E%20%D0%BE%D1%82%D1%80%D0%B0%D1%81%D0%BB%D1%8C.pdf">Статистические показатели, характеризующие развитие туристской отрасли в Российской Федерации</a>\
<a href="https://tourism.gov.ru/upload/iblock/ba3/%D0%A7%D0%B8%D1%81%D0%BB%D0%B5%D0%BD%D0%BD%D0%BE%D1%81%D1%82%D1%8C%20%D0%B3%D1%80%D0%B0%D0%B6%D0%B4%D0%B0%D0%BD%20%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D0%B9%D1%81%D0%BA%D0%BE%D0%B9%20%D0%A4%D0%B5%D0%B4%D0%B5%D1%80%D0%B0%D1%86%D0%B8%D0%B8,%20%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%BD%D1%8B%D1%85%20%D0%B2%20%D0%BA%D0%BE%D0%BB%D0%BB%D0%B5%D0%BA%D1%82%D0%B8%D0%B2%D0%BD%D1%8B%D1%85%20%D1%81%D1%80%D0%B5%D0%B4%D1%81%D1%82%D0%B2%D0%B0%D1%85%20%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%B8%D1%8F%20%D0%BF%D0%BE%20%D0%BA%D0%B2%D0%B0%D1%80%D1%82%D0%B0%D0%BB%D0%B0%D0%BC%202018%20-%202020%20%D0%B3%D0%B3.xls">Численность граждан Российской Федерации, размещенных в коллективных средствах размещения по кварталам 2018–2020 гг.</a>\
<a href="https://tourism.gov.ru/upload/iblock/dad/%D0%A7%D0%B8%D1%81%D0%BB%D0%B5%D0%BD%D0%BD%D0%BE%D1%81%D1%82%D1%8C%20%D0%B3%D1%80%D0%B0%D0%B6%D0%B4%D0%B0%D0%BD%20%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D0%B9%D1%81%D0%BA%D0%BE%D0%B9%20%D0%A4%D0%B5%D0%B4%D0%B5%D1%80%D0%B0%D1%86%D0%B8%D0%B8,%20%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%BD%D1%8B%D1%85%20%D0%B2%20%D0%BA%D0%BE%D0%BB%D0%BB%D0%B5%D0%BA%D1%82%D0%B8%D0%B2%D0%BD%D1%8B%D1%85%20%D1%81%D1%80%D0%B5%D0%B4%D1%81%D1%82%D0%B2%D0%B0%D1%85%20%D1%80%D0%B0%D0%B7%D0%BC%D0%B5%D1%89%D0%B5%D0%BD%D0%B8%D1%8F%20%D0%B2%202018%20-%202020%20%D0%B3%D0%B3.xls">Численность граждан Российской Федерации, размещенных в коллективных средствах размещения в 2018–2020 гг.</a>