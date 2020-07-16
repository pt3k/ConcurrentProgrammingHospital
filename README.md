# Szpitalny odział ratunkowy
### Aplikacja wykonana na zaliczenie z przedmiotu "Programowanie Współbieżne" symulująca działanie szpitalnego oddziału ratunkowego.
![](https://raw.githubusercontent.com/pt3k/ConcurrentProgrammingHospital/master/dzialanie.png?token=ADG6UFOHJDUD7OMNDXMQ2GK7DGPLG)
### Zasady działania:
- dyżuruje kliku lekarzy o różnej specjalizacji
- pacjenci są kierowani w rejestracji do specjalisty ze względu na swoją „dolegliwość”
- lekarze mogą zlecać wykonanie dodatkowych badań
- ciężkie przypadki są obsługiwane poza kolejnością.
/
### Każdy lekarz to osobny wątek, a przesyłanie i obsługiwanie pacjentów jest synchronizowane za pomocą mechanizmu Lock i Monitor tak aby zachować własność żywotności i bezpieczeństwa programu.
