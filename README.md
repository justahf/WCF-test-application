# WCF-test-application

Задание:

Написать WCF службы, имитирующие работу с адресной книгой. Службы должны работать с сущностью Контактное лицо (ФИО, телефон) и поддерживать следуюшие операции:

Получение всех контактов
Поиск контакта по телефону
Поиск контакта по ФИО
Добавление контакта в адресную книгу
Изменение телефону у конктактного лица
Удление контакта из адресной книги


Реализация:

HostProcess - консольное приложение, хостит WCF-службы. При первом вызове создает и инициализирует базу данных.
FormsClient - приложение-клиент (WinForms), предоставляет интерфейс для работы со службами.
WPFClient - заготовка для приложения-клиента на базе WPF.

Инструкция:
1. Запустить HostProcess.exe
2. Запустить FormsClient.exe
3. Нажать <Refresh>
4. Выполнить желаемые дейсвтия с базой
5. Закрыть FormsClient.exe
6. Сделать окно HostProcess.exe активным и нажать <Enter>

Программа предоставляет самый базовый функционал, возможные ошибки и уязвимости представляю, не вносил соответствующие коррективы для экономии времени. Это мой первый опыт использования WCF, поэтому, возможно, сделал слегка не то, что от меня ожидали.
