# U3d_Flips

Small test project where I focuse on the MVx like achitecture.
MVx, UniRx, composite root, strategy, scriptable objects, DoTween, Odin

The project based on Test task the text of which I quote below:

Платформа Standalone Windows
● 3D сцена
○ Статичная плоскость стола
● 3rd person камера, направленная в центр стола
● Объекты сцены
○ Кодом создаем в сцене 10 объектов двух типов (см.ниже), начальное
расположение произвольной сеткой, соотношение по типам 50/50
○ Тип Flat: плоский тонкий примитив
○ Тип Volume: объемный примитив, например: куб, пирамида, шар и т.д.
○ Каждый из 10 объектов имеет уникальную произвольную текстуру
■ Все текстуры лежат в Streaming Assets (при загрузке учесть
возможность замены путей на Url в вебе)
■ Натягивать текстуры можно прямо на примитивы простым
способом, пропорции, тайлинг и т.п. не важны, главное видеть
разные текстуры на разных объектах
● Интерактив
○ Объекты можно двигать мышью в плоскости стола. Можно использовать
простую физику, чтобы исключить визуальное пересечение объектов,
пусть передвигаемый объект двигает остальные
○ При клике на объект он выделяется заменой цвета материала на серый,
выделение снимается (возвращаем белый цвет) при клике на другой
объект или на сцену
○ Если выделен объект типа Flat, внизу экрана появляются две кнопки: Flip
и Rotate
○ Если выделен объект типа Volume, внизу экрана появляется только
кнопка Rotate
○ Flip переворачивает объект на 180 градусов, с анимацией
○ Rotate поворачивает объект в плоскости стола на 90 градусов, с
анимацией
Архитектура
● Заложить: типы объектов будут добавляться, нужен понятный способ
добавления нового типа
● Заложить: инструментов (Flip/Rotate) может быть больше, аналогично пункту
выше
● Предусмотреть выключения инструментов, пример: выключить Rotate простым
и понятным способом
