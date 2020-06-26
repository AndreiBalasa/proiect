# proiect Andrei Balasa

Proiectul consta in simularea unui/unor turnee cu toate elementele corespunzatoare( echipe,membrii, prize, rounds,etc)

Am incercat sa citesc/salvez datele atat in fisier cat si intr o baza de date . Din moment ce baza de date este locala,
voi seta proiectul sa faca operatiile necesare cu fisiere( acest switch se poate face din Program.cs , la linia 23 din instructiunea:
TrackerLibrary.GlobalConfig.InitializeConnections(DatabaseType.TextFile); )  metoda initializeConnections foloseste un enum care seteaza ca datele de acces
sa fie ori SQL ori Textfile 

Pentru a putea seta calea unde vor fi create/updatate fisierele se intra in app.config si se modifica path-ul din:
<add key="filePath" value="C:\Users\andre\Desktop\Proiect Danciu\Turneu\TournamentTracker\files"/>

Interfata IDataConnection este folosita ca un contract pentru clasele ce vor rezolva conexiunea la sql/txt. Metodele din interfata trebuiesc implementate
corespunzator atat in TextConnector cat si in SqlConnector pentru fiecare tip de date din fiecare form(createPrize, createTeam...etc)

Clasa textConnectorProcessor face conexiunea cu fisierele :
metoda FullFilePath ne va oferi path-ul complet al fisierului
metoda LoadFile ne va ajuta sa incarcam intr un List<string> continutul fisierului
metodele convertTO... ne vor ajuta sa convertim datele in list<T> unde T este tipul obiectului de care avem nevoie in functie de datele claselor Models
(PersonModel,PrizeModel...etc)

Am reusit sa implementez so far:

salvarea in fisier/sql a datelor din formul CreatePrize!
salvarea/citirea in fisier/sql a datelor din formul createTeam! ( cu tot cu link-urile dintre add member-create member si team memberslistbox)
iar in formul CreateTournament am reusit sa cuplez cei 2 pasi de mai sus: 
(Acest form nu este inca finalizat deoarece inca mai am de implementat salvarea datelor din matchup-uri atat pentru baza de date cat si pentru txt,
voi incerca sa implementez pe viitor acest lucru din moment ce trebuie sa implementez o logica pt Matchup-uri ( cum se vor desfasura meciurile,cum se vor
salva datele meciurilor precedente..etc)

Deci sunt functionale procesele: insert Prize in csv/sql , insert/read Teams in csv/sql ,create tournament va putea fi in momentul in care matchups va fi implementat,
insa acesta este functional in SQL la momentul actual , in csv nu.

Pentru sql am folosit dapper.
Procedurile folosite pentru datele din SQL le am creat in storedProcedures (de exemplu :dbo.spPeople_Insert ce insereaza in SQL datele de tip PersonModel) 


Stiu ca m am apucat tarziu de acest proiect , insa iti voi trimite ce am implementat pana acum. Daca e nevoie sa testam si functionalitatea SQL sunt disponibil
sa ne intalnim pe zoom/discord..etc
Eu o sa tot continui sa l imbunatatesc day by day , dar in cazul in care ce am so far nu este suficient pentru o nota de trecere , imi asum!



