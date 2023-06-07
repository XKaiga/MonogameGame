# MonogameGame
A game made in Monogame

## Andre:
### Doing:
    ?

### To do:
    menus/telas(buttons, design, art, etc...):
        main menu(name of the game, buttons[start/exit]);
        options menu(input box, buttons[exit/apply/cancel]);
        help menu & help buttom(decide where to put it: main menu or options menu);
        level menu(various planets, each is a level);
        tela final(buttons[main menu]);
	tela de creditos;
    music(which music(a loop music) and maybe how to implement(it's easy i think));
    Level Design(onde é que tem plataformas e etc..., e onde os poderes estão escondidos, basicamente os mapas dos niveis);

## Helder:
### Doing:
    ?

## Sara:
### Doing:
    arte


# To Do:
	[sistema de highscore];
	[camâra focada no player];
	Class Level Manager: para fazer o jogador ir de nivel a nivel;
	sistema de save para 'guardar e sair do jogo';	
	extra i think: plataformas que se mexem;
	enemy ai;
	[powers]:
		buble shield(position = player_position, tranparency = ?%, collision with enemyWeapon);
		portal(idea: can only have two portals on the map, code: if exists 2 portals when player touches one teleports to the other(player.position = ?);
		air(pushes enemy, code: if in collision with enemy, dependendo de que lado vem manda y ou x para -valor);
	pedra com lava, se player toca leva dano a cada ?secs;

## Levels:
### level1(water):
    objetivo:
        pergar todas as coisinhas, quando pega todas no fim do mapa aparece uma seta e ai o player pode ir para o final da tela e sair do nivel
    dificuldades:
        anda com um freio muito fraco, e tem umas minas aquaticas pelo mapa, que precisamos de desviar
    mundo(estilo/graficamente/tema):
        aquático;
        quando o player sobe da agua vê uma cascata;
    mecânicas:
        movimentar o personagem com o rato;
        anda livremente;

### level2(fire):
    objetivo:
        pergar todas as coisinhas, quando pega todas no fim do mapa aparece uma seta e ai o player pode ir para o final da tela e sair do nivel
    dificuldades:
        rochas a cair do ceu por causa do vulcão;
    mundo(estilo/graficamente/tema):
        vulcão
    mecânicas:
        plataformas

### level3(earth):
    objetivo:
        pergar todas as coisinhas, quando pega todas no fim do mapa aparece uma seta e ai o player pode ir para o final da tela e sair do nivel
    dificuldades:
        polvo rocho que vai atras do player, e player foge;
    mundo(estilo/graficamente/tema):
        floresta;
    mecânicas:
        plataformas;

### level4(air):
    objetivo:
        pergar todas as coisinhas, quando pega todas no fim do mapa aparece uma seta e ai o player pode ir para o final da tela e sair do nivel
    dificuldades:
        polvo vermelho, fica fora do ecra a disparar contra o player;
    mundo(estilo/graficamente/tema):
        nuvems;
    mecânicas:
        andar livremente;


### level5(space)(boss):
    objetivo:
        derrotar o boss;
    dificuldades:
        boss(ideia: dependendo do numero de podres que o player pegou, o boss é mais fraco/forte, quanto mais poderes pegos, boss mais fraco a cada um);
    mundo(estilo/graficamente/tema):
        espaço;
    mecânicas:
        andar livremente;
