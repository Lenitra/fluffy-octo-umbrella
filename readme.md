MODELS 3D : https://sketchfab.com/MRowa/models

## Todo List
- Gestion client de l'affichage du territoire avec un code couleur
- Visuel de déplacement des unités





Inspirations : 
- Mini jeu de conquête for honor 
- Risk, jeu de plateau 

Cartes 
Jeu de conquête avec des cartes bonus débloquables et une gestion d'unités.
Terrain en tuiles hexagonales contenant un nombre de points d'unités permettant l'attaque et la défense contre les tuiles adjacentes
Une tuile appartient à un joueur dès lors qu'il a au moins une unité dessus.
Une tuile conquises peut déplacer des troupes sur les tuiles adjacentes pour attaquer des tuiles ou pour défendre des tuiles
Le joueur possède une tuile QG améliorable avec des missions individuelles qui génère des unités dans la durée.
Bonus de tuiles invisibles (carte face cachée) 

Système de deck permettant au joueur de définir un deck de X cartes qui pourraient être tirés aléatoirement. Le joueur peut débloquer des cartes aléatoires avec des missions individuelles.


Missions individuelles qui permettent de gagner du gold/xp qui permet d'augmenter le QG et d'acheter des lootboxes de cartes.
- tuile a conquérir/défendre en une certaine durée
- rassembler un maximum d'unités sur une tuile


Cartes qui s'obtiennent sur la durée, plus il y a de tuiles conquises plus on obtient des cartes rapidement. 
Liste des bonus de cartes
- diminue l'intervalle de récupération de cartes
- boost les points d'une tuile (%)
- génère des unités sur la tuile
- diminue les unités de n'importe quelle tuile
- augmentation des unités d'une tuile
- révéler si il y a un bonus sur la tuile ciblée
- ...



## Installation sur serveur 
sauvegarder le dossier HexWarServer/data et le rename en tmpHexWar
- cp ...
supprimer le dossier serveur
- rm -rf HexWarServer
Cloner le repo git 
- git clone ...
Extraire le dosser Server et le renommer en HexWarServer
- CP
supprimer le dossier HexWarServer/data
- rm -rf HexWarServer/data
déplacer le dossier tmpHexWar et le renommer HexWarServer/data
- cp ...


################################################################################
### Prochain git commit/push : 
Sync repo sur pc portable
Upgrades et get prices pour nouveaux builds
Couleurs et batiments des autres joueurs
