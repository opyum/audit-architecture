# Dossier de conception de Dimming Programs

- [Dossier de conception de Dimming Programs](#dossier-de-conception-de-dimming-programs)
  - [Description du domaine](#description-du-domaine)
    - [Aggregate root](#aggregate-root)
    - [Entités](#entités)
  - [Cas d'utilisation](#cas-dutilisation)
    - [Diagramme général](#diagramme-général)
    - [Fiches des Uses Cases](#fiches-des-uses-cases)
      - [Récupération d'un programme de fonctionnement du microservice Energy.DimmingPrograms :](#récupération-dun-programme-de-fonctionnement-du-microservice-energydimmingprograms-)
        - [Diagramme de cas d'utilisation détaillé](#diagramme-de-cas-dutilisation-détaillé)
        - [Diagramme d'activité](#diagramme-dactivité)
        - [Diagramme de séquence](#diagramme-de-séquence)
        - [Requête](#requête)
        - [Réponse](#réponse)
      - [Création d'un programme de fonctionnement du microservice Energy.DimmingPrograms :](#création-dun-programme-de-fonctionnement-du-microservice-energydimmingprograms-)
        - [Diagramme de cas d'utilisation détaillé](#diagramme-de-cas-dutilisation-détaillé-1)
        - [Diagramme d'activité](#diagramme-dactivité-1)
        - [Diagramme de séquence](#diagramme-de-séquence-1)
        - [Requête](#requête-1)
        - [Réponse](#réponse-1)
        - [Erreurs](#erreurs)
      - [Vérification de l'existence d'un programme de fonctionnement du microservice Energy.DimmingPrograms :](#vérification-de-lexistence-dun-programme-de-fonctionnement-du-microservice-energydimmingprograms-)
        - [Diagramme de cas d'utilisation détaillé](#diagramme-de-cas-dutilisation-détaillé-2)
        - [Diagramme de séquence](#diagramme-de-séquence-2)
    - [Actions utilisateurs / systèmes](#actions-utilisateurs--systèmes)
    - [Evènements du domaine](#evènements-du-domaine)
      - [Détail des évènements](#détail-des-évènements)
        - [Energy-DimmingProgramChanged](#energy-dimmingprogramchanged)
        - [DimmingPrograms-DimmingCalendarChangeRequested](#dimmingprograms-dimmingcalendarchangerequested)
        - [DimmingCalendars-DimmingCalendarChangeRequested](#dimmingcalendars-dimmingcalendarchangerequested)
        - [HistorizeItemRequested](#historizeitemrequested)
    - [Règles métiers](#règles-métiers)
  - [Interfaçage avec l'extérieur](#interfaçage-avec-lextérieur)
    - [Persistance des données de référence](#persistance-des-données-de-référence)
  - [Implémentation technique](#implémentation-technique)
    - [Infrastructure utilisée](#infrastructure-utilisée)
  - [Cas de tests](#cas-de-tests)
  - [Annexes](#annexes)
    - [Api publique](#api-publique)
    <br/>

## Description du domaine

Ce µService est responsable des programmes de fonctionnement incluant:

- La création d'un programme.
- La modification d'un programme (à venir).
- La suppression d'un programme (à venir).

 Les volumétries estimées sont :
    - au maximum 3 000 programmes de fonctionnement.

<br/>

**Entités du domaine:**
![domain entities image](images/domain_entities.png)Enti

### Aggregate root
<br/>

 - **DimmingProgram**

Représente l'_aggregate root_ du domaine des programmes de fonctionnement.

| Propriété        | Type        | Description                         | Requis | Contraintes |
| ---------------- | ----------- | ----------------------------------- | ------ | ----------- |
| DimmingProgramId | Guid        | Identifiant du programme            | Oui    | Unique      |
| Code             | String      | Code du programme                   | Oui    | Unique      |
| Label            | String      | Libellé du programme                | Oui    |             |
| Color            | String      | Couleur du programme                | Oui    |             |
| Mode             | Enum        | Mode de fonctionnement du programme | Oui    |             |
| FixedHours       | FixedHour[] | Heures fixes du programme           | Non    |             |
| SolarHours       | SolarHour[] | Heures solaire du programme         | Non    |             |

<br/>

### Entités
<br/>

- **OperatingMode**

Enum décrivant les mode de fonctionnement

| Nom              | Value | Description      |
| ---------------- | ----- | ---------------- |
| MidnightMidnight | 0     | Minuit -> Minuit |
| NoonNoon         | 1     | Midi -> Midi     |

<br>


- **FixedHour**

Objet décrivant une heure fixe

| Propriété | Type           | Description                                        | Requis | Contraintes |
| --------- | -------------- | -------------------------------------------------- | ------ | ----------- |
| Time      | DateTimeOffset | Heure à laquelle commence la puissance d'éclairage | Oui    |             |
| Power     | Int            | Puissance de l'éclairage                           | Oui    |             |

<br/>

- **SolarHour**

Objet décrivant une heure solaire

| Propriété     | Type      | Description                                        | Requis | Contraintes |
| ------------- | --------- | -------------------------------------------------- | ------ | ----------- |
| MinutesBefore | Int       | Nombre de minutes avant le lever/coucher du soleil | Non    |             |
| MinutesAfter  | Int       | Nombre de minutes après le lever/coucher du soleil | Non    |             |
| Mode          | SolarMode | Mode lever ou coucher du soleil                    | Oui    |             |
| Power         | Int       | Puissance de l'éclairage                           | Oui    |             |

<br/>

- **SolarMode**

Enum décrivant les référentiels d'éphémérides

| Nom     | Value | Description                      |
| ------- | ----- | -------------------------------- |
| Sunset  | 0     | En fonction du coucher de soleil |
| Sunrise | 1     | En fonction du lever de soleil   |

<br>

- **DayOfWeek**

Enum décrivant les jours de la semaine

| Nom       | Value | Description |
| --------- | ----- | ----------- |
| Monday    | 0     | Lundi       |
| Tuesday   | 1     | Mardi       |
| Wednesday | 2     | Mercredi    |
| Thursday  | 3     | Jeudi       |
| Friday    | 4     | Vendredi    |
| Saturday  | 5     | Samedi      |
| Sunday    | 6     | Dimanche    |

<br>

- **ExceptionType**

Enum décrivant le type d'exception (fixe ou calendaire)

| Nom      | Value | Description                        |
| -------- | ----- | ---------------------------------- |
| Fixed    | 0     | Exception fixe répété annuellement |
| Calendar | 1     | Exception calendaire unique        |

<hr/>


## Cas d'utilisation

<br/>

### Diagramme général

<br/>

![use cases image](images/use-cases.png)

<br/>

### Fiches des Uses Cases


| Nom du Use case                                                    | Acteur                                                                                                                          | Intervenants et intérêts | Contexte d'utilisation                                                                                                                                                                                                                                                                                                                                                                                             |
| ------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------- | ------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Récupération d'un programme                                        | L'acteur est utilisateur de l'API du µService . Il peut s'agir d'une application web (comme MUSE Energy) ou d'un autre service. | /                        | Récupérer un programme de fonctionnement.<br />- [ART-1](https://muse-software.aha.io/features/ART-1)<br />                                                                                                                                                                                                                                                                                                        |
| Création de programme                                              | L'acteur est utilisateur de l'API du µService . Il peut s'agir d'une application web (comme MUSE Energy) ou d'un autre service. | /                        | Créer un programme de fonctionnement, qui sera affiché dans la liste des programmes ou utilisé dans l'affectation de régimes<br />- [ART-143](https://muse-software.aha.io/features/ART-143)<br />                                                                                                                                                                                                                 |
| Vérification de l'existence d'un  programme et la validité de mode | L'acteur est le systéme embarqué pour la chorégraphie de gestion d'un régime de fonctionnement.                                 | /                        | La réception de l'événement DimmingPrograms-DimmingCalendarChangeRequested déclenche une vérification sur l'existence de programme de fonction par ID et si le mode de tout la liste des programmes sont les mêmes que le mode de régime et émet l'évenement DimmingCalendars-DimmingCalendarChangeRequested contenant les résultats pour la création d'un régime par la suite par le µService Dimming Calendars . |
<br/>

<hr/>

<br/>

#### Récupération d'un programme de fonctionnement du microservice Energy.DimmingPrograms :

<center>

 ##### Diagramme de cas d'utilisation détaillé

![diagram cas d'utilisation image](images/diagram_use_case_recuperation.png)

<br/>

##### Diagramme d'activité

![diagram activities image](images/diagram_activity_get_programs.png)

<br/>

##### Diagramme de séquence

<br/>

![diagram sequence image](images/diagram_sequence_get_programs.png)

</center>
<br/>
<hr/>

##### Requête

* Url : /api/v1/dimming-programs/{id} *avec {id} l'identifiant du programme à retourner de type Guid*
* Méthode : GET
* Entête : Pas de données dans l'entête
* Contenu : Pas de données dans le corps

##### Réponse
* Code : 200
* Contenu : 
```json
  {
    "dimmingProgramId": "c7c2aef0-f1fc-428c-b593-c8d06d17b55a",
    "code": "1236",
    "label": "Test programme",
    "color": "#fff",
    "mode": "NoonNoon",
    "fixedHours": [
        {
            "time": "2020-01-01T13:00:00",
            "power": 34
        },
        {
            "time": "2020-01-01T19:00:00",
            "power": 98
        }
    ],
    "solarHours": [
        {
            "minutesBefore": 0,
            "minutesAfter": 0,
            "mode": "Sunset",
            "power": 98
        },
        {
            "minutesBefore": 0,
            "minutesAfter": 0,
            "mode": "Sunrise",
            "power": 98
        }
    ]
}
```

<br/>

#### Création d'un programme de fonctionnement du microservice Energy.DimmingPrograms :

<center>

##### Diagramme de cas d'utilisation détaillé

![diagram cas d'utilisation image](images/diagram_use_case_creation.png)

<br/>

##### Diagramme d'activité

![diagram activities image](images/diagram_activity_post_dimming_program.png)

<br/>

##### Diagramme de séquence

![diagram sequence image](images/diagram_sequence_post_dimming_program.png)

</center>
<br/>

##### Requête

* Url : /api/v1/dimming-programs
* Méthode : POST
* Entête : Les données standards
* Contenu : 
  ```json
     {   
        "code": "1234",
        "label": "Test programme",
        "color": "#759E88",
        "mode": 1,
        "fixedHours": [
          {
            "time": "8/2/2022 12:00:00 AM" ,
            "power": 2
          },
          ...
        ],
        "solarHours": [
          {
            "minutesBefore": 22,
            "minutesAfter": 23,
            "mode": 1,
            "power": 2
          },
          ...
        ]
      }
  ```
  <br/>

<br/>

##### Réponse

* Code : 201
* Contenu : Pas de données dans la réponse

<br/>

#####  Erreurs

| Code | Détail                                              |
| ---- | --------------------------------------------------- |
| 400  | Programme de fonctionnement non conforme            |
| 400  | Le code doit être unique                            |
| 500  | Erreur de sauvegarde du programme de fonctionnement |

<br/>
<hr/>

#### Vérification de l'existence d'un programme de fonctionnement du microservice Energy.DimmingPrograms :

<center>

##### Diagramme de cas d'utilisation détaillé

![diagram cas d'utilisation image](images/diagram_use_case_verifi.png)

<br/>

Dans le diagramme de séquence ci-dessous, nous allons traiter le cas de vérification sur l'existence d'un  programme de fonctionnement du microservice Energy.DimmingPrograms dans le contexe de mettre en place la chorégraphie:

##### Diagramme de séquence

![diagram sequence image](images/diagram_sequence_verif.png)
</center>

<br/>
<hr/>

### Actions utilisateurs / systèmes

### Evènements du domaine


Cette application écoute et/ou publie les évènements suivants :

| Evènement                                       | Sens   | Topic                                           | Description                                                                                      |
| ----------------------------------------------- | ------ | ----------------------------------------------- | ------------------------------------------------------------------------------------------------ |
| Energy-DimmingProgramChanged                    | Publie | energy-dimmingprogramchanged                    | Notifie des changements sur les programmes de fonctionnement                                     |
| DimmingPrograms-DimmingCalendarChangeRequested  | Ecoute | dimmingprograms-dimmingcalendarchangerequested  | Reçoit une demande de création d'un régime de fonctionnement                                    |
| DimmingCalendars-DimmingCalendarChangeRequested | Publie | dimmingcalendars-dimmingcalendarchangerequested | Notifie des vérifications sur l'exitence des programmes de fonctionnement et la validité de mode |
| HistorizeItemRequested                          | Publie | historize-item-requested                        | Notifie une historisation d'un programme de fonctionnement                                       |

#### Détail des évènements

##### Energy-DimmingProgramChanged
<br/>

  - **Cas d'ajout** : Permet de notifier sur l'enregistrement d'un programme de fonctionnement.

  - **Cas de modification** : Entraine une mise à jour  et elle ne sera pas traiter en ce PI.

  - **Cas de suppression** : Entraine une suppression d'un programmme de fonctionnement mais elle ne sera pas traiter en ce PI.

| Propriété     |                  | Type   | Description                                                 |
| ------------- | ---------------- | ------ | ----------------------------------------------------------- |
| EventId       |                  | Guid   | Identifiant de l'évènement                                  |
| Timestamp     |                  | Date   | Date d'émission de l'évènement                              |
| EventType     |                  | Enum   | Statut de l'évènement 0 = CREATED, 2 = UPDATED, 4 = DELETED |
| CorrelationId |                  | Guid   | Identifiant de corrélation                                  |
| Content       |                  | Object | Contenu de l'évènement                                      |
|               | DimmingProgramId | Guid   | Identifiant du programme                                    |
|               | Code             | String | Code du programme                                           |
|               | Label            | String | Libellé du programme                                        |
|               | Color            | String | Couleur du programme                                        |
|               | Mode             | Enum   | 0 = Minuit -> Minuit, 1 = Midi -> Midi                      |

Exemple de contenu:
```json
    {   
      "EventId": "0d4f4a88-9428-4d09-b4db-64130eb7a33f",
      "Timestamp": "2021-12-22T10:33:40.2899005+00:00",
      "CorrelationId": "e60b7ad3-65fd-4d44-83ee-3831613ae4a1",
      "EventType": "CREATED",
      "Content": {
          "DimmingProgramId": "56DADA0A-DAC6-4A51-A709-6BA681128698",
          "Code": "1234",
          "Label": "Test programme",
          "Color": "#759E88",
          "Mode": "NoonNoon",
	    }
    }
```

##### DimmingPrograms-DimmingCalendarChangeRequested
<br/>

Cet évènement va prendre deux cas pratiques : 

- cas de succès : si la chorégraphie s'est bien passée entre les différents services, on va envoyer un message avec l'objet "ValidationErrors" vide.

**Message :** DimmingPrograms-DimmingCalendarChangeRequested

**Topic :** dimmingprograms-dimmingcalendarchangerequested

| Propriété        |                             |                  | Type     | Description                                                            |
| ---------------- | --------------------------- | ---------------- | -------- | ------------------------------------------------------------           |
| EventId          |                             |                  | Guid     | Identifiant de l'évènement                                             |
| Timestamp        |                             |                  | Date     | Date d'émission de l'évènement                                         |
| EventType        |                             |                  | Enum     | Statut de l'évènement 6 = CREATING, 7 = UPDATING, 8 = DELETING         |
| CorrelationId    |                             |                  | Guid     | Identifiant de corrélation                                             |
| ValidationErrors |                             |                  | Object[] | Liste d'erreurs                                                        |
|                  | AppId                       |                  | String   | Identifiant du µService à l'origine de l'erreur                        |
|                  | Timestamp                   |                  | Date     | Timestamp de l'erreur                                                  |
|                  | Code                        |                  | Number   | Code de l'erreur                                                       |
|                  | Message                     |                  | String   | Message de l'erreur                                                    |
| Content          |                             |                  | Object   | Contenu de l'évènement                                                 |
|                  | Id                          |                  | Guid     | Identifiant du régime                                                  |
|                  | Mode                        |                  | Enum     | 0 = Minuit -> Minuit, 1 = Midi -> Midi                                 |
|                  | Code                        |                  | String   | Code du régime                                                         |
|                  | Label                       |                  | String   | Libellé du régime                                                      |
|                  | Description                 |                  | String   | Description du régime                                                  |
|                  | DimmingProgramsWeekDays     |                  | Object[] | Programmes affectés à chaque jour de la semaine                        |
|                  |                             | DayOfWeek        | Enum     | 0 = Lundi => 6 = Dimanche                                              |
|                  |                             | DimmingProgramId | Guid     | Identifiant du programme                                               |
|                  | DimmingProgamsExceptionDays |                  | Object[] | Programmes affectés à un jour répété annuellement ou un jour calendaire|
|                  |                             | StartDate        | DateOnly | Date de début de l'exception                                           |
|                  |                             | EndDate          | DateOnly | Date de fin de l'exception                                             |
|                  |                             | DimmingProgramId | Guid     | Identifiant du programme                                               |
|                  |                             | ExceptionType    | Enum     | 0 = Jour fixe, 1 = Jour calendaire                                     |
|                  | UnknownFields               |                  | Object[] | Dictionnaire reprenant les propriétés inconnues                        |

Exemple de contenu :
```json
  {   
		"EventId": "0d4f4a88-9428-4d09-b4db-64130eb7a33f",
		"Timestamp": "2021-12-22T10:33:40.2899005+00:00",
		"EventType": "CREATING",
    "CorrelationId": "e60b7ad3-65fd-4d44-83ee-3831613ae4a1",
    "ValidationErrors": [],
		"Content": {
      "Id":"56DADA0A-DAC6-4A51-A709-6BA681128698",
			"Mode": "NoonNoon",
			"Description":"Description du régime",
			"Code": "1234",
			"Label": "Test Régime",
      "DimmingProgramsWeekDays": [
          {
          "DayOfWeek": "Monday",
          "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a"
          },
         {
          "DayOfWeek": "Tuesday",
          "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a"
         },
        ...
      ],
      "DimmingProgramsExceptionDays": [
        {
          "StartDate": "01/01/2022",
          "EndDate": "01/01/2022",
          "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
          "ExceptionType": "Fixed"
        },
        {
          "StartDate": "01/05/2022",
          "EndDate": "01/05/2022",
          "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
          "ExceptionType": "Fixed"
        },
        {
          "StartDate": "05/26/2022",
          "EndDate": "05/26/2022",
          "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
          "ExceptionType": "Calendar"
        },
        ...
      ],
      "UnknownFields": []
		}
  }
```

- cas d'erreur: si un service rencontre une erreur, dans ce cas on va envoyer le même message que de cas précédente y compris l'objet "ValidationErrors" ayant les valeurs d'erreurs.

**Message :** DimmingPrograms-DimmingCalendarChangeRequested

**Topic :** dimmingprograms-dimmingcalendarchangerequested

Exemple du contenu :
```json
  {   
		"EventId": "0d4f4a88-9428-4d09-b4db-64130eb7a33f",
		"Timestamp": "2021-12-22T10:33:40.2899005+00:00",
		"EventType": "CREATING",
    "CorrelationId": "e60b7ad3-65fd-4d44-83ee-3831613ae4a1",
    "ValidationErrors": [
        {
         "AppId": "bdica-dimming-calendars",
         "Timestamp": "2021-12-22T10:33:40.2899005+00:00",
         "Code": 400,
         "Message": "Mode has not a valid value"
        }
      ],
    "Content": {
      "Id":"56DADA0A-DAC6-4A51-A709-6BA681128698",
			"Mode": "NoonNoon",
			"Description":"Description du régime",
			"Code": "1234",
			"Label": "Test Régime",
      "DimmingProgramsWeekDays": [
            {
            "DayOfWeek": "Monday",
            "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a"
            },
            {
            "DayOfWeek": "Tuesday",
            "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a"
            },
              ...
            ],
      "DimmingProgramsExceptionDays": [
            {
            "StartDate": "01/01/2022",
            "EndDate": "01/01/2022",
            "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
            "ExceptionType": "Fixed"
            },
            {
            "StartDate": "01/05/2022",
            "EndDate": "01/05/2022",
            "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
           "ExceptionType": "Fixed"
            },
            {
            "StartDate": "05/26/2022",
            "EndDate": "05/26/2022",
            "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
            "ExceptionType": "Calendar"
            },
          ...
         ],
     "UnknownFields": []
		}
  }
```

Elle émet les évènements suivants :

##### DimmingCalendars-DimmingCalendarChangeRequested
<br/>

Cet évènement va prendre deux cas pratiques : 

- cas de succès : si la chorégraphie est bien passée entre les différents services, on va envoyer un message avec l'objet "ValidationErrors" vide.

**Message :** DimmingCalendars-DimmingCalendarChangeRequested

**Topic :** dimmingcalendars-dimmingcalendarchangerequested

Exemple du contenu  :
```json
  {   
		"EventId": "0d4f4a88-9428-4d09-b4db-64130eb7a33f",
		"Timestamp": "2021-12-22T10:33:40.2899005+00:00",
		"EventType": "CREATING",
    "CorrelationId": "e60b7ad3-65fd-4d44-83ee-3831613ae4a1",
    "ValidationErrors": [],
		"Content": {
      "Id":"56DADA0A-DAC6-4A51-A709-6BA681128698",
			"Mode": "NoonNoon",
			"Description":"Description du régime",
			"Code": "1234",
			"Label": "Test Régime",
      "DimmingProgramsWeekDays": [
              {
              "DayOfWeek": "Monday",
              "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a"
              },
              {
              "DayOfWeek": "Tuesday",
              "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a"
              },
             ...
            ],
      "DimmingProgramsExceptionDays": [
             {
             "StartDate": "01/01/2022",
             "EndDate": "01/01/2022",
             "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
             "ExceptionType": "Fixed"
             },
             {
             "StartDate": "01/05/2022",
             "EndDate": "01/05/2022",
             "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
             "ExceptionType": "Fixed"
             },
             {
             "StartDate": "05/26/2022",
             "EndDate": "05/26/2022",
             "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
             "ExceptionType": "Calendar"
             },
           ...
         ],
      "UnknownFields": []
		}
  }
```

| Propriété        |                             |                  | Type     | Description                                                            |
| ---------------- | --------------------------- | ---------------- | -------- | ------------------------------------------------------------           |
| EventId          |                             |                  | Guid     | Identifiant de l'évènement                                             |
| Timestamp        |                             |                  | Date     | Date d'émission de l'évènement                                         |
| EventType        |                             |                  | Enum     | Statut de l'évènement 6 = CREATING, 7 = UPDATING, 8 = DELETING         |
| CorrelationId    |                             |                  | Guid     | Identifiant de corrélation                                             |
| ValidationErrors |                             |                  | Object[] | Liste d'erreurs                                                        |
|                  | AppId                       |                  | String   | Identifiant du µService à l'origine de l'erreur                        |
|                  | Timestamp                   |                  | Date     | Timestamp de l'erreur                                                  |
|                  | Code                        |                  | Number   | Code de l'erreur                                                       |
|                  | Message                     |                  | String   | Message de l'erreur                                                    |
| Content          |                             |                  | Object   | Contenu de l'évènement                                                 |
|                  | Id                          |                  | Guid     | Identifiant du régime                                                  |
|                  | Mode                        |                  | Enum     | 0 = Minuit -> Minuit, 1 = Midi -> Midi                                 |
|                  | Code                        |                  | String   | Code du régime                                                         |
|                  | Label                       |                  | String   | Libellé du régime                                                      |
|                  | Description                 |                  | String   | Description du régime                                                  |
|                  | DimmingProgramsWeekDays     |                  | Object[] | Programmes affectés à chaque jour de la semaine                        |
|                  |                             | DayOfWeek        | Enum     | 0 = Lundi => 6 = Dimanche                                              |
|                  |                             | DimmingProgramId | Guid     | Identifiant du programme                                               |
|                  | DimmingProgamsExceptionDays |                  | Object[] | Programmes affectés à un jour répété annuellement ou un jour calendaire|
|                  |                             | StartDate        | DateOnly | Date de début de l'exception                                           |
|                  |                             | EndDate          | DateOnly | Date de fin de l'exception                                             |
|                  |                             | DimmingProgramId | Guid     | Identifiant du programme                                               |
|                  |                             | ExceptionType    | Enum     | 0 = Jour fixe, 1 = Jour calendaire                                     |
|                  | UnknownFields               |                  | Object[] | Dictionnaire reprenant les propriétés inconnues                        |


- cas d'erreur: si un service rencontre une erreur, dans ce cas on va envoyer le même message que de cas précédente y compris l'objet "ValidationErrors" ayant les valeurs d'erreurs.

**Message :** DimmingCalendars-DimmingCalendarChangeRequested

**Topic :** dimmingcalendars-dimmingcalendarchangerequested

Exemple du contenu  :
```json
  {   
		"EventId": "0d4f4a88-9428-4d09-b4db-64130eb7a33f",
		"Timestamp": "2021-12-22T10:33:40.2899005+00:00",
		"EventType": "CREATING",
    "CorrelationId": "e60b7ad3-65fd-4d44-83ee-3831613ae4a1",
    "ValidationErrors": 
      [
         {
         "AppId": "bdipr-dimming-programs",
         "Timestamp": "2021-12-22T10:33:40.2899005+00:00",
         "Code": 400,
         "Message": "Dimming Program doesn't exist"
         }
      ],
	  "Content": {
      "Id":"56DADA0A-DAC6-4A51-A709-6BA681128698",
			"Mode": "NoonNoon",
			"Description":"Description du régime",
			"Code": "1234",
			"Label": "Test Régime",
      "DimmingProgramsWeekDays":
          [
          {
          "DayOfWeek": "Monday",
          "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a"
          },
          {
          "DayOfWeek": "Tuesday",
          "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a"
          },
           ...
          ],
           "DimmingProgramsExceptionDays": [
          {
          "StartDate": "01/01/2022",
          "EndDate": "01/01/2022",
          "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
          "ExceptionType": "Fixed"
          },
          {
          "StartDate": "01/05/2022",
          "EndDate": "01/05/2022",
          "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
          "ExceptionType": "Fixed"
          },
          {
          "StartDate": "05/26/2022",
          "EndDate": "05/26/2022",
          "DimmingProgramId": "870ca9d9-0064-4bbf-9c43-a5335b95fc3a",
          "ExceptionType": "Calendar"
          },
          ...
        ],
      "UnknownFields": []
		}
  }
```

##### HistorizeItemRequested
<br/>

Ce message est envoyé pour historiser les modifications apportées à la création, modification ou suppression d'un programme de fonctionnement.

**Topic :** historize-item-requested

[Voir documentation du µservice 'musepf-mvp-vch-change-history'](https://ctgmusepf.visualstudio.com/MusePF/_git/musepf-mvp-vch-change-history?path=/Documentation/ChangeHistory.md&_a=preview)

<br/>

### Règles métiers

- On ne gère pas de permissions d'accès pour le moment.
- La validation de la création d'un programme inclue les règles suivantes :
  - Code : requis, longueur max 50, unique pour le domaine
  - Label : requis, longueur max 50
  - Mode : requis, soit 0 soit 1
  - Color : requis, longueur min 4 max 7, format '#rrggbb'
- La validation des heures fixes inclue les règles suivantes :
  - Time : requis
  - Les deux bornes doivent être présentes : 
    - Mode 0 MidnightToMidnight : bornes 23:59:xx et 00:00:xx
    - Mode 1 NoonToNoon : bornes 11:59:xx et 12:00:xx
  - Chaque heure fixe doit être unique pour son couple hh:mm
  - La puissance varie entre 0% et 100% .
- La validation des heures solaires inclue les règles suivantes :
  - Mode : requis, soit 0 soit 1
  - Les durées avant le lever ou après le coucher de soleil varient entre 0 et 360 minutes .
  - La puissance varie entre 0% et 100% .

<br/>

## Interfaçage avec l'extérieur

### Persistance des données de référence

- **Type du store** : Redis
- **Nom du store** : *dimming-programs-store*
- **Clé de stockage** : *dimming-programs*

- **Structure de stockage** :

```json
    [
      {
          "DimmingProgramId": "56DADA0A-DAC6-4A51-A709-6BA681128698",
          "Code": "01651209651845184",
          "Label": "programme numéro 1",
          "Color": "#759E88",
          "Mode": "MidnightMidnight",
          "FixedHours": [],
          "SolarHours": []
      },
      ...
    ]
```


## Implémentation technique


### Infrastructure utilisée

Les composants logiciels utilisés sont les suivants :

- Dapr : Environnement d'exécution permettant la construction d'applications distribuées de type micro-services, avec la mise à disposition de blocs d'infrastructure intégrés.

- Redis : Redis est une base de données NoSQL en mémoire, orientée clé/valeur, qui est utilisé pour le stockage des données.

- MediatR : Permet de gérer la communication entre le bloc API et le bloc Domaine.
  C'est une implémentation du "Mediator Pattern", qui permet de réduire le couplage et les dépendances entre les classes/modules.
  
- Serilog : Framework utilisé pour la journalisation.

- Seq : Permet la centralisation des logs.

- Zipkin : Outil de monitoring à partir des logs.

  

## Cas de tests

- **Feature** : Get dimming program

- **Scenario** : When a user is logged in, he can get dimming program

  - **Given** a connected user

  - **When** the user requests the dimming program

  - **Then** dimming program should be returned

    ------
  
  - **Given** a connected user
  
  - **And** dimming program not stored
  
  - **When** the user search the dimming program
  
  - **Then** an empty list of dimming program should be returned

</br>

- **Feature** : Post dimming program

- **Scenario** : When a user is logged in and has rights to create a program, he can create a dimming program

  - **Given** a connected user, without right to create a program

  - **When** the user post a dimming program

  - **Then** 401 error should be returned

    ------

  - **Given** a connected user, with right to create a program

  - **When** the user post a dimming program

  - **And** the mandatory fields are given

  - **Then** the program should be created successfully

    ------

  - **Given** a connected user, with right to create a program

  - **When** the user post a dimming program

  - **And** on or more mandatory are missing

  - **Then** an exception should be thrown indicating that the one or more fields are mandatory.

</br>

## Annexes

### Api publique

Les Apis publiques ont pour url de base l'url suivante : [Host Api Gateway]/[Environnement]/energy

| Url publique                          | Url interne                     | Description                        |
| ------------------------------------- | ------------------------------- | ---------------------------------- |
| *[Base-Url]/v1/dimming-programs*      | */api/v1/dimming-programs*      | Url de création des programmes     |
| *[Base-Url]/v1/dimming-programs/{id}* | */api/v1/dimming-programs/{id}* | Url de récupération d'un programme |
