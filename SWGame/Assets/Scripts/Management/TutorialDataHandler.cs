﻿namespace SWGame.Management
{
    static class TutorialDataHandler
    {
        private static string[] _tutorialChapters;

        static TutorialDataHandler()
        {
            _tutorialChapters = new string[20];
            _tutorialChapters[0] = "\tВы - контрабандист, недавно перевозивший ценный груз Фа'атре Хатту, стоимостью 1 000 000 кредитов.\n" +
                "\tНа Ваш корабль было совершено нападение, груз был уничтожен, напарник погиб. Фа'атра Хатт, недовольный таким расположением дел, назначил награду за Вашу голову в организации \"Обмен\"" +
                " в размере 1 000 000 кредитов.\n" +
                "\tТеперь Вам необходимо решить проблему с долгом, и продолжить свой путь в Галактике. Из инфопанели своего погибшего напарника Вы узнали несколько способов решения данной проблемы:\n\n" +
                "\t1. Заплатить 1 000 000 кредитов непосредственно Фа'атре, что повлечет отмену заказа.\n" +
                "\t2. Заплатить 1 000 000 кредитов организации \"Обмен\", вследствие чего заказ также будет отменен.\n" +
                "\t3. Добиться защиты Ордена Джедаев. Необходимо заработать 15 000 очков мудрости (подробнее в разделе \"Анклав джедаев\").\n" +
                "\t4. Добиться защиты Ордена Ситхов. Необходимо заработать 15 000 очков престижа (подробнее в разделе \"Академия ситхов\")." +
                "\n\tУ Вас в кармане лишь 5000 кредитов и множество проблем. Развивайте своего персонажа, выполняйте различную игровую активность и становитесь самым богатым и свободным жителем Галактики!" +
                "\n\n\n\t\tПриятной игры!";
            _tutorialChapters[1] = "\tПазаак - карточная игра, цель которой набрать 20 баллов, или приблизиться к ней ближе чем соперник, но без перебора. Игрок с суммой очков наиболее близкой к 20 выигрывает раунд, а игрок, выигравший 3 раунда, выигрывает матч. Если в конце раунда очки противников равны, раунд не засчитывается ни одному из игроков.\n" +
                "\tВ пазааке есть 3 разные колоды карт. Главная, собранная из карт с номиналами 1-10 (общее число карт в этой колоде не задано точно, хотя существовала тенденция набирать по 4 карты каждого номинала). Также у каждого игрока есть своя боковая колода, которая должна быть собрана самим игроком и иметь ровно 10 карт." +
                "В начале игры каждому игроку случайным образом выбирается 4 карты из боковой колоды. Эти 4 карты составляют «Руку» игрока. После этого начинается собственно игра. Игрок берёт карту из главной колоды и кладёт её на игровой стол. После этого он имеет право выложить на игровой стол карту из «Руки» либо закончить ход. За один ход может быть использована только одна карта из «Руки», карты из боковой колоды во время матча в «Руку» не добавляются.\n" +
                "Существует два варианта окончания хода:\n" +
                "\t1.Закончить ход: Если игрок просто оканчивает ход, то в начале следующего хода он берёт карту из главной колоды и выкладывает её на игровой стол. Так продолжается до тех пор, пока оба игрока не спасуют, не заполнят слоты на своих игровых столах, один из игроков не наберет 20 очков или пока у одного из игроков не окажется перебора.\n" +
                "\t2.Пас: Если игрок пасует, то сумма его карт замораживается до конца раунда, он больше не имеет права играть какими-либо картами. Оппонент, тем не менее, всё ещё может продолжить игру до тех пор, пока также не спасует, не наберет 20 или перебор. Если сумма карт игрока оказывается равной 20, то игрок автоматически пасует.\n" +
                "\tПосле окончания хода, игра переходит к другому игроку, который ходит по таким же правилам. Игра продолжается до тех пор, пока кто-нибудь не выиграет раунд. Победа в трёх раундах обеспечивает победу в матче." +
                "\n\tВсего есть три способа победить в матче:\n" +
                "\tНабрав большее количество очков: После того, как оба игрока спасовали, выигрывает тот, у кого сумма больше (при условии, что его сумма не больше 20).\n" +
                "\tПеребор: Если сумма игрока превышает 20 и у него нет возможности снизить её, используя особые карты, он «сгорает» и выигрывает его оппонент.\n" +
                "\tЗаполнив все слоты на игровом столе: В некоторых редких случаях, если игрок смог разместить карты на всех девяти слотах игрового стола без перебора, он автоматически выигрывает. Он также выигрывает даже если его сумма меньше, чем у оппонента или их суммы равны. Ничья засчитывается в том случае, если оба игрока заполнили свои игровые столы без перебора.\n";
            _tutorialChapters[2] = "\tБоковая колода составляется из карт самого игрока. Игрок волен самостоятельно продавать или обменивать карты по всей галактике и оставлять себе те, которые он хотел бы видеть у себя в боковой колоде. Существует два типа карт: Плюс-карты(обычно синего цвета) и Минус-карты(красные). Также есть Улучшенные карты(которые обычно золотого цвета). Карты главной колоды — зелёные, а в старых наборах для игры — золотистые.\n" +
                "\tПлюс-карты добавляют к сумме карт игрока свой номинал. Значения этих карт колеблются от 1 до 6.\n" +
                "\tМинус-карты похожи на положительные, но в отличие от них отнимают свой номинал от суммы игрока. Их значения также колеблются от 1 до 6.\n" +
                "\tПлюс-минус-карты объединяют в себе достоинства плюс-карт и минус-карт. Игрок сам выбирает, как трактовать знак карты. Это самые дорогие и высоко ценящиеся карты в галактике. Их значения также находятся в диапазоне от 1 до 6.\n" +
                "\tПлюс-минус 1 или 2 карта — очень редкая плюс-минус карта, чей номинал может быть 1 или 2, в зависимости от пожеланий игрока.\n" +
                "\tКарты-перевёртыши — улучшенные карты с нулевым номиналом, меняющие знак некоторых карт на игровом столе. Например, если игрок сыграл картой «2&4», все +2 и +4 на столе станут −2 и −4, в то время как −2 и −4 станут +2 и +4 соответственно. Плюс- и Минус-карты(как и Плюс-минус-карты) также меняют свои значения. На просторах галактики доступны варианты «2&4» и «3&6» карт-перевертышей.\n" +
                "\tДабл-карта — редкая карта, удваивающая номинал последней сыгранной карты. Например, если игрок сыграл сначала 2, потом 9, потом использовал Дабл-карту, то 9-ка станет 18, и общая сумма карт станет равной 20.\n" +
                "\tПоследнее слово чемпиона — крайне редкая карта, действующая подобно +/- 1, но в случае ничьей, игрок, использовавший эту карту, выигрывал.\n";
            _tutorialChapters[3] = "\tГробницы Коррибана - место погребения величайших лордов ситхов, таких, как Марка Рагнос, Аджанта Полл, Нага Садоу, Тулак Хорд и ряда других.\n" +
                "\n\tДля раскопок в древней гробнице необходим голокрон ситхов, который можно купить в магазине при Академии ситхов, либо выкопать в древней гробнице.\n" +
                "В древней гробнице, кроме голокрона ситхов, можно выкопать следующие предметы:\n\t1)Световой меч ситхов;" +
                "\n\t2)Красный кристалл;\n\t3)Кодекс ситхов;" +
                "\n\tЭти предметы помогут Вам добиться покровительства Ордена ситхов.\n" +
                "\n\n\n\tНо будьте осторожны! Духи древних лордов не дремлют, и могут атаковать Вас, что повлечет за собой потерю голокрона.";
            _tutorialChapters[4] = "\tРакатанские руины на Дантуине были разрушенным ракатанским храмом «Звёздной кузницы», сооружённом около 30000 ДБЯ в эпоху наивысшего расцвета Бесконечной Империи.\n\t" +
                "\n\tДля раскопок в древних руинах необходим медальон джедаев, который можно купить в магазине при Анклаве джедаев, либо выкопать в древних руинах. Помимо медальона джедаев в древних руинах можно получить следующие предметы:\n" +
                "\t1)Световой меч;\n\t2)Великий голокрон;\n\t3)Голокрон джедаев;\n" +
                "\tЭти предметы помогут Вам добиться покровительства Ордена джедаев.\n" +
                "\n\n\n\tНо будьте осторожны! Древний ракатанский дроид еще активен, и может атаковать Вас, что повлечет за собой потерю медальона джедаев.";
            _tutorialChapters[5] = "\tХрам джедаев, располагавшийся на планете Дантуин, был академией Ордена джедаев. Как и в Храме на Корусанте, юные ученики-джедаи обучались здесь путям Силы. Анклав имел собственный совет, состоявший из четырёх членов и подчинявшийся Высшему Совету на Корусанте.\n" +
                "\n\tВ Храме джедаев Вы можете рассчитывать на защиту Ордена джедаев в случае наличия 15 000 очков мудрости. Для получения защиты необходимо нажать на значок Совета джедаев. Очки мудрости можно получить, сдавая предметы, выкопанные в древних руинах. Стоимости всех предметов:\n" +
                "\t1)Великий голокрон - 350 очков мудрости;\n" +
                "\t2)Медальон джедаев - 200 очков мудрости;\n" +
                "\t3)Световой меч - 250 очков мудрости;\n" +
                "\t4)Голокрон джедаве - 150 очков мудрости;\n" +
                "\n\n\n\tПри наличии 15 000 очков мудрости, по нажатию на Анклав, Вам будет предложено принять защиту Ордена джедаев.";
            _tutorialChapters[6] = "\tАкадемия ситхов на Коррибане была центром Империи ситхов в течении Гражданской войны джедаев, направленной на тренировки чувствительных к Силе, которые решили встать на путь Тёмной стороны, чтобы в дальнейшем отправляться на поиски древних артефактов и знаний в Долину Тёмных лордов и стать мастерами.\n" +
                "\n\tДля прохода в Академию необходим ситский медальон, который можно купить в магазине при Академии. В самой Академии Вы можете рассчитывать на защиту Ордена ситхов в случае наличия 2 500 очков престижа. Очки престижа можно получить, сдавая предметы, выкопанные в древней гробнице. Стоимости всех предметов:\n" +
                "\t1)Голокрон ситхов - 300 очков престижа;\n" +
                "\t2)Световой меч ситхов - 250 очков престижа;\n" +
                "\t3)Красный кристалл - 50 очков престижа;\n" +
                "\t4)Кодекс ситхов - 150 очков престижа;\n" +
                "\n\n\n\tПри наличии 2 500 очков престижа, по нажатию на знак Темного совета, Вам будет предложено принять защиту Ордена ситхов." +
                "\n\n\tОднако будьте осторожны! Академия полна враждебных аколитов, которые не прочь атаковать Вас ради наживы или просто забавы для. В случае поражения вы потеряете ситский медальон.";
            _tutorialChapters[7] = "\tКрупнейшая луна Нал-Хатты, поверхность которой, подобно Корусанту, с 24500 ДБЯ по 26 ПБЯ была покрыта экуменополисом. Однако, в отличие от главной галактической планеты-города Корусанта, Нар-Шаддаа была довольно трущобным миром, с загрязнённой атмосферой, а также с огромным количеством\nпреступников и многих контрабандистов." +
                "\n\tВ игре представлена двумя локациями: \"Верхний Нар-Шаддаа\" и \"Нижний Нар-Шаддаа\"." +
                "\n\tНа верхнем Нар-Шаддаа можно завершить сюжет игры, сняв заказ на свою голову в организации \"Обмен\". Также тут можно сыграть в пазаак с противником, использующим простые и карты-перевертыши руки. Еще тут есть магазин, в котором можно купить золотые карты пазаака." +
                "\n\tНа нижнем Нар-Шаддаа можно сыграть в казино и приобрести некоторые игровые предметы. Результатом игры в казино может быть победа, при которой ставка возвращается в двойном размере, поражение — потеря ставки, и возврат ставки." +
                " \n\n\n\tНыне контролируется организацией \"Обмен\".";
            _tutorialChapters[8] = "\tПланета, находящаяся в системе Й’Тауб Среднего Кольца, которая является столицей Пространства хаттов. На преступном сленге планета называлась «Хаттой». Её спутник Нар-Шаддаа — пристанище пиратов, всевозможных торговцев оружием и контрабандистов." +
                "\n\tВ игре Нал-Хатта представлена двумя локациями: \"Кантина Нал-Хатты\" и \"Город Нал-Хатты\". " +
                "\n\tКантина является стартовой локацией игры, так что в ней можно приобрести стартовый набор игрока в пазаак и провести свой первый матч. Противник в данной локации может использовать только простые карты руки." +
                "\n\tВ городе можно приобрести более широкий ассортимент карточек пазаака, вернуть долг хаттам, завершая сюжетную линию. Также в нем есть космопорт, посредством которого можно начать свое путешествие по Галактике." +
                "\n\n\n\tНыне контролируется Картелем Хаттов.";
            _tutorialChapters[9] = "\tРодина чистокровных ситхов и священное место для их Ордена. В древние времена здесь строили гробницы для сильнейших правителей,поскольку планета имела сильную ауру Тёмной стороны Силы. После Великой гиперпространственной войны Коррибан был покинут. Позже, в 3958 ДБЯ, здесь была реконструирована Академия ситхов, и планета стала обитаемой вновь." +
                "\n\tВ игре представлен тремя локациями: \"Вход в Академию ситхов\", \"Долины Гробниц\", \"Академия ситхов\"." +
                "\n\tВ Академии ситхов можно сдавать предметы за очки престижа, принять защиту Ордена ситхов. Подробнее об Академии в пункте \"Академия ситхов\" выше." +
                "\n\tВ Долине гробниц можно выкопать различные предметы, имеющие ценность при сдаче в Академии. Подробнее о Долине можно почитать выше, в пункте \"Гробницы Коррибана\"." +
                "\n\tВ локации \"Вход в Академию ситхов\" можно приобрести необходимые квестовые предметы для прохода в Академию ситхов и Долины гробниц, а также карты-перевертыши для игры в пазаак." +
                "\n\n\n\tКонтролируется Орденом ситхов.";
            _tutorialChapters[10] = "\tКрасивый мир зеленых равнин, тихих рек и чистых озёр. Дантуин значительно отстоял от основных торговых маршрутов Галактики, его население было незначительным и сильно разбросанным по планете в виде небольших поселений с обширным землевладением. Разумные расы были представлены людьми-фермерами, а также примитивными племенами дантари." +
                "\n\tВ игре представлен тремя локациями: \"Анклав джедаев\", \"Руины Дантуина\", \"Храм джедаев\"." +
                "\n\tВ Анклаве джедаев можно приобрести медальон джедаев, необходимый для раскопок в руинах и карты-перевертыши для игры в пазаак." +
                "\n\tВ руинах можно выкопать предметы, сдаваемые в Храме за очки мудрости. Подробнее о руинах можно прочитать в разделе \"Руины Дантуина\" выше." +
                "\n\tВ Храме можно сдавать предметы за очки мудрости и принять защиту Ордена джедаев, нажав на значок Совета джедаев. Подробнее о Храме можно прочитать выше, в разделе \"Храм джедаев\"." +
                "\n\n\n\tКонтролируется Орденом джедаев.";
            _tutorialChapters[11] = "\tПланета Внешнего Кольца, расположенная в отдаленном крае Галактики. Она находится на пересечении многих гиперпространственных маршрутов, а потому многие торговцы используют космопорт Мос-Эйсли в качестве пересадочного пункта. Татуин стал пристанищем для различного рода авантюристов — контрабандистов, наёмников, охотников за головами." +
                "\n\tВ игре представлен двумя локациями: \"Мос-Эйсли\" и \"Фермы Татуина\"." +
                "\n\tВ Мос-Эйсли можно приобрести набор взрывчатки для охоты на крайт-дракона, продать жемчужину крайт-дракона. Также можно приобрести карточки пазаака. Здесь можно сразиться в пазаак с компьютером, оппонент сможет использовать простые и карты-перевертыши руки." +
                "\n\tНа фермах можно произвести охоту на крайт-дракона. Для этого необходим набор взрывчатки, приобретаемый в Мос-Эйсли. Вероятность успеха составляет 10%, однако награда стоит этого." +
                "\n\n\n\tНыне контролируется Картелем Хаттов.";
            _tutorialChapters[16] = "\tT - вызов справки;\n\n" +
                "\tEsc - выход из игры;\n\n" +
                "\tF - перевернуть карту (работает при наличии выбранной одинарным щелчком мыши карты типа +/- либо золотой карты);\n\n" +
                "\tC - просмотреть информацию об игроке (для просмотра обновленной информации необходимо закрыть и открыть окно игрока с помощью данной клавиши);\n\n" +
                "\tI - просмотреть инвентарь игрока (для просмотра обновленной информации необходимо закрыть и открыть окно игрока с помощью данной клавиши);\n\n" +
                "\tCtrl+P - открыть окно настроек.";
            _tutorialChapters[12] = "\tОрден джедаев - древняя духовно-рыцарская организация, объединенная принципами и взглядами на Силу. Джедаи были защитниками мира и справедливости в Галактической Республике и стали самой известной из всех группировок, связанных с Силой. Возглавляемый чередой Советов джедаев, Орден расширялся на протяжении тысячелетий, несмотря на многочисленные испытания, большая часть которых была связана с противостоянием ситхам, носителям тёмной стороны Силы.\n" +
                "\n\n\tГлавная база в игре - Анклав джедаев на Дантуине.";
            _tutorialChapters[13] = "\tОрден ситхов был древней сектой чувствительных к Силе, избравших её Тёмную сторону. По своей сути ситхи являлись полной противоположностью джедаев, поклонявшихся светлой стороне." +
                        "Орден ситхов по праву был самой известной религиозной организацией, поклонявшейся тёмной стороне Силы. В течение всего своего существования ситхи командовали несколькими империями и инициировали множество галактических войн. Кроме того, орден породил бесчисленное число сект, вдохновленных ситхами." +
                        "Во главе ордена ситхов стоял Тёмный лорд ситхов. Восхождение нового Тёмного лорда ситхов зачастую сопровождалось серьезными реформациями в ордене — неизменными оставались только задачи по уничтожению Ордена джедаев и получения безграничной власти в галактике." +
                        "Эта религия пережила все известные этапы галактической истории, подвергнувшись при этом множествам видоизменений и реинкарнаций.\n" +
                        "\n\n\tГлавная база в игре - Академия ситхов на Коррибане.";
            _tutorialChapters[14] = "\tОфициально Картель Хаттов является деловым альянсом нескольких кланов, которые работают сообща над общими целями богатства и процветания. Члены картеля контролируют огромное количество ресурсов, бессчетное число кредитов и множество независимых миров - не последние среди них Хатта, мир, который они сделали домом, и Нар Шаддаа, жемчужина в короне преступного мира. Хотя у хаттов нет формального правительства, на их планетах Картель действует как административный совет. Неофициально, Картель является безжалостным преступным синдикатом, а цели его членов не всегда столь схожи. Война за сферы влияния и соперничество кланов часто превращают картель в разобщенную толпу. Зависть, жадность, личные обиды и ожесточенная конкуренция может привести к тому, что вчера два клана воевали друг с другом на улицах, а сегодня их лидеры жмут друг другу руки. Исторически, Картель во всех галактических конфликтах занимал нейтральное положение, но ни Империя, ни Республика не оставляют попыток склонить его на свою сторону. Остаётся лишь наблюдать, к чему это сможет привести и какое влияние на галактику в целом будет оказывать Картель.";
            _tutorialChapters[15] = "\tОбмен был преступной организацией, проявлявший активность приблизительно в 4000 ДБЯ, и возможно был самой могущественной организацией того времени, они занимались контрабандой спайса, вымогательством, торговлей оружием, работорговлей и охотой за головами. Организация была активна на многих планетах и использовала множество охотников за головами, в том числе и печально известного Кало Норда. Во времена первой чистки джедаев они назначили огромную награду за живого джедая, настолько огромную, что на нее можно было купить целую планету.";
            _tutorialChapters[17] = "\tДля создания сетевого матча в Пазаак вам необходимо прилететь на планету Корусант, и нажать на изображение карт, оформленное в зеленом цвете. Вы увидите окно с открытыми вызовами, где сможете принять любой из них, либо создать свой." +
                "\n\tДля того, чтобы создать вызов, вам необходимо иметь не менее 4х карточек Пазаака и ввести ставку, не превышающую ваш бюджет. В противном же случае, будет отображено соответствующее сообщение об ошибке." +
                "\n\tАналогичные требования налагаются и на прием вызовов других игроков." +
                "\n\tПосле приема вами/вашего вызова начинается матч в Пазаак по классическим правилам, до 3х побед. Внутриигровой налог на данные матчи отсутствует, поэтому победитель забирает весь банк, а при ничьей никто не остается в убытке." +
                "\n\tВ случае намерянного, либо случайного отключения игроку засчитывается поражение без возможности доиграть матч при обратном подключении. Также нельзя создать больше одной игры и присоединиться к нескольким играм одновременно.";
            _tutorialChapters[18] = "\tСверкающая планета, расположенная в сердце Галактики. Корусант тысячелетиями был политическим центром Галактики, где принимались самые важные решения, затрагивающие жизни триллионов." +
                "\n\tВ игре представлен единственной локацией - Верхний Корусант, где находится крупнейший магазин Галактики со всем карточками Пазаака и некоторыми другими важными игровыми предметами, а также есть возможность сыграть в Пазаак в оффлайн (бот среднего уровня сложности) и онлайн режимах. По поводу онлайн режима подробнее сказано в разделе \"Онлайн пазаак\"." +
                "\n\tПоскольку Корусант является столицей Республики, ценовая политика его магазина соответствует этому статусу.";
            _tutorialChapters[19] = "\tС появлением веб-ориентированной версии приложения в него были добавлены возможности просмотра базовой информации о игроках-соседях по локации и общения в игровом чате. Обе эти возможности действуют в рамках текущей локации игрока." +
                "\n\tДля просмотра информации об игроке-соседе необходимо кликнуть на его никнейм в списке \"Игроки\". В появившемся окне будет отображен его никнейм, статус (онлайн/оффлайн), аватар, число очков престижа и мудрости, медаль / ее отсутствие при пройденном игроком сюжете / его отсутствии. Аналогичную информацию о себе можно увидеть нажав на кнопку \"Профиль\", либо на клавишу С." +
                "\n\tДля общения в чате неободимо ввести свое сообщение в соответствующее поле и нажать кнопку \"Отправить\", либо левую клавишу Enter. Сообщение не должно быть пустым и превышать 200 символов. Отправленное сообщение будет находиться в самом верху области сообщений." +
                "\n\tВ области просмотра сообщений отображаются последние 100 сообщений, принадлежащие чату данной локации. В сообщении указано время отправки, автор сообщения, и непосредственно само содержание сообщения. Прокрутка сообщений идет снизу вверх (самые новые сообщения находятся вверху).";
        }

        public static string[] TutorialChapters { get => _tutorialChapters; set => _tutorialChapters = value; }
    }
}
