# MonogameGame
# The adventures of the octocat
André Jardin,nº25978
Sara Silva, nº25973
Hélder Rodrigues, nº25958
A game made in Monogame
# Mecânicas
O nosso jogo tem 2 tipos de nível: nível com movimento do rato e nível com movimento através do teclado em que é possivel usar o poder de teletransporte apartir do clique do rato a cada 20 segundos.
Infelizmente não conseguimos acabar o nosso jogo. Era suposto ter 5 níveis e conseguimos fazer 3. Em cada nível o jogador desbloqueia um poder, os níveis não têm necessariamente ordem, apartir do momento que desbloqueia o poder este pode ser usado nos outros níveis. 
# Código 
Class Level Manager: para fazer o jogador ir de nivel a nivel;
Temos:
	enemy ai;
	animação;
	colisões,
	som;
	menus;
	
## Powers implementados
	portal;
	poder de pedra;

## Levels:
### level1(water)(feito):
    objetivo:
        pergar todos os coraçoes
    dificuldades:
        anda com um freio muito fraco, e tem umas minas aquaticas pelo mapa, que precisamos de desviar
    mundo(estilo/graficamente/tema):
        aquático;
        quando o player sobe da agua vê uma cascata;
    mecânicas:
        movimentar o personagem com o rato;
        anda livremente;

### level2(earth)(feito):
    objetivo:
        pergar todos os coraçoes
    dificuldades:
        polvo rocho que vai atras do player, e player foge;
    mundo(estilo/graficamente/tema):
        floresta;
    mecânicas:
        plataformas;
	
	
### level3(fire)(feito):
    objetivo:
        pergar 10 coraçoes
    dificuldades:
        2 tipos de en imigos: 2 a disparar e um a disparar e a perseguir, não pode tocar no chão
    mundo(estilo/graficamente/tema):
        vulcão
    mecânicas:
        plataformas



### level4(air)(por fazer):
    objetivo:
        pergar todos os corações
    dificuldades:
        polvo vermelho, fica fora do ecra a disparar contra o player;
    mundo(estilo/graficamente/tema):
        nuvems;
    mecânicas:
        andar livremente;


### level5(space)(boss)(por fazer):
    objetivo:
        derrotar o boss;
    dificuldades:
        boss(ideia: dependendo do numero de podres que o player pegou, o boss é mais fraco/forte, quanto mais poderes pegos, boss mais fraco a cada um);
    mundo(estilo/graficamente/tema):
        espaço;
    mecânicas:
        andar livremente;
