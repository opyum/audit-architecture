## **Choregraphie impliquant le Bounded Context Dimming Calendars**

<br>

### _Description_

<br>

Ce µservice est impliqué dans la chorégraphie de gestion d'un régime de fonctionnement (Création, Mise à jour, Suppression).
La chorégraphie permet de :
- Vérifier l'existence des programmes que l'on souhaite affecter
- Gérer les permissions (à venir)

<br>

### _Documents de reference_

<br>

| Titre                         | Description                                            | Lien                                                                                                                                                      |
| ----------------------------- | ------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Muse Energy Dimming Calendars | Dossier de conception de Muse Energy Dimming Calendars | [Dossier de conception](https://ctgmusepf.visualstudio.com/MusePF/_git/musepf-mvp-energy-dimming-calendars?path=/docs/Application%20design.md&_a=preview) |
| Muse Energy Dimming Programs  | Dossier de conception de Muse Energy Dimming Programs  | [Dossier de conception](https://ctgmusepf.visualstudio.com/MusePF/_git/musepf-mvp-energy-dimming-programs?path=/docs/Application%20design.md&_a=preview)  |

<br>

### _Principe de base des choregraphies_

<br>

- La transmission d'un évènement de µService en µService via le bus de données, pour former une boucle. Il n'y a pas de parallélisme dans les envois.
- Le message transmis est propre à chaque µService, c'est-à-dire non issu d'une dll commune.
- Le message transmis ne doit pas perdre d'information au fur et à mesure des échanges, il doit uniquement être enrichi.
- Il est possible de publier les messages de la chorégraphie dans un même topic.
- Afin d'éviter le déclenchement de plusieurs actions simultanément il a été ajouter dans les messages la propriété "StepName" qui permet d'identifier la dernière étape réalisée dans la chorégraphie.
- Chaque étape de la chorégraphie devra donc écouter sur le même topic mais n'écoutera que les événements émis par l'étape l'étape précédente.
- Il est possible que ce mécanisme ne soit pas mis en place dans un autre domaine. Dans ce cas l'écoute d'un message en provenance de ce domaine devra être fait sur un topic séparé pour éviter toute écoute multiple. 

<br>

### _Choregraphie de creation d'un regime de fonctionnement_

<br>

La création d'un régime de fonctionnement nécessite une certaine chorégraphie, car des données de plusieurs domaines doivent être vérifiées :
- Les programmes de fonctionnement (Domaine Energy, µService DimmingPrograms)
- Les permissions de l'utilisateur (Domaine Energy, µService DimmingCalendarAuthorizations)(A venir)

<br>

### _Diagramme d'activites_

<br>

Dans le diagramme d'activité ci-dessous, nous allons traiter le cas de création d'un régime de fonctionnement du microservice Energy.DimmingCalendars sur l'ensemble de la chorégraphie:

<br>

![diagram activities create choregraphy](images/diagram_activities_create_choregraphy.png)

<br>

### _Diagramme de choregraphie_

<br>

![choregraphy image](images/choregraphy_create.png)

<br>

<hr>

<br>