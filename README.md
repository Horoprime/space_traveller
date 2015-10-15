# space_traveller
Unity project for learning

# Alpha ver 0.1

Управление:
Передвижение корабля происходит по одной ячейке. Установлено на клавиши WSAD
Переключение между кораблями выполняется с помощью нажатия на числа на клавиатуре от 0 до 9.
Нажатие на пробел - начало новой игры.

Что сделано:
- Игрок появляется посреди карты 20*20
- У игрока область видимости 9 ячеек
- Контент карты вычисляется только в пределах области видимости. Когда ячейка выходит за неё, информация о содержимом удаляется.
- При повторном посещении какого-либо участка карты игрок видит тот же контент, что и был ранее.
- Контент появляется согласно указанной в задании вероятности.
- Корабль игрока и корабль под управлением компьютера отличаются по цвету.
- На карте всего присутствуют 10 космических кораблей, 9 из которых действуют согласно логике, описанной в задании. Корабли, выходящие за область видимости, также не видны.
- Корабли под управлением компьютера имеют 2 состояния: выбор цели, передвижение к цели.
- Реализовано переключение между кораблями, при этом бывший под управлением игрока корабль передаётся под управление компьютеру. Также у кораблей поменяются цвета.
- Реализован рестарт игры, при этом контент карты будет отличаться от предыдущего. 
