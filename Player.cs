using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// variaveis do projeto
	public float velocidade, forcapulo;
	private bool estaNoChao;
	public Transform chaoVerificador;
	// spritePlayer servira para pegar o conteudo vinculado ao sprite heroi_sprite
	public Transform spritePlayer;
	private Animator animator;

	// Use this for initialization
	void Start () {
		// A variável spritePlayer irá pegar o conteúdo vinculado ao sprite heroi_sprite
		/*
			Dele nós queremos ter o controle de tudo que fizemos na aba Animator. 
			Por isso que fizemos dentro do método Start, ou seja, quando o personagem 
			for criado, nós iremos jogar as configurações da aba Animator do heroi_sprite 
			na variável animator do nosso script.
		*/
		animator = spritePlayer.GetComponent<Animator> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		// chama a funcao de movimentacao()
		movimentacao ();
	}

	void movimentacao () {

		/* este comando joga na var bool, true ou false, onde sera analisado se entre o personagem
		   (transform.position) e o objeto chaoVerificador(chaoVerificador.position), existe algum
		   conteudo com o layer Piso(1 << LayerMask.NameToLayer("Piso")), Caso exista sera atribuido
		   o valor verdadeiro no atributo estaNoChao.
		*/
		estaNoChao = Physics2D.Linecast (transform.position, chaoVerificador.position, 1 << LayerMask.NameToLayer("Piso"));
		animator.SetBool ("chao", estaNoChao);
		// condicao: se apertar a seta da horizontal "<-" ou "->"
		// se essa seta for pra direta, ou seja, em um vetor a seta aponta para o x positivo
		if(Input.GetAxisRaw ("Horizontal") > 0) {
			// pega o transform do objeto e faz ele se movimentar para a direita
			transform.Translate(Vector2.right * velocidade * Time.deltaTime);
			// pega o tranform do objeto e o rotaciona para posicao zero
			transform.eulerAngles = new Vector2 (0, 0);
		}
		// se essa seta for pra esquerda, ou seja, em um vetor a seta aponta para o x negativo
		if (Input.GetAxisRaw ("Horizontal") < 0) {
			/*
				* pega o transform do objeto e faz ele se movimentar para a esquerda
				* se reparar o codigo abaixo, é o mesmo que faz o objeto andar para a direita
				* o certo seria colocar um - antes do vector2, ficando assim:
				* transform.Translate (-Vector2.right * velocidade * Time.deltaTime);
				* porem, como temos o codigo para fazer o objeto rotacionar 180 para esquerda
				* com isso nosso objeto invertera o lado do vetor que ira andar
			*/
			transform.Translate (Vector2.right * velocidade * Time.deltaTime);
			// rotaciona o objeto a 180 graus no eixo y
			transform.eulerAngles = new Vector2 (0, 180);
		}

		/*
		 * Vamos entender agora o nosso método de Movimentacao. Foi adicionada uma nova 
		 * verificação, que analisa se o botão responsável pelo pulo, 
		 * Input.GetButtonDown(“Jump”), que por padrão é a tecla Espaço do teclado foi 
		 * pressionada. Caso tenha sido, ele irá entrar na condicional aonde será aplicada uma 
		 * força para cima contra a gravidade no rigidbody do nosso Player, graças ao comando 
		 * rigidbody2D.AddForce(transform.up * forcaPulo).
		*/
		// verifica se a tecla espaço foi pressionada e se o estaNoChao tem o valor true
		if (Input.GetButtonDown ("Jump") && estaNoChao) {
			// faz o objeto pular
			//rigidbody2D.addForce(transform.up * forcapulo);
			GetComponent<Rigidbody2D>().AddForce(transform.up * forcapulo);
		}
		/*
			Esta linha irá jogar dentro do parâmetro movimento, um valor 0 ou maior 
			que 0. O Mathf.abs serve para pegar o valor absoluto de Input.GetAxisRaw (“Horizontal”). 
			Sendo assim não importa se ele for para a direita ou para esquerda, o valor 
			retornado nunca será negativo. Com isto o parâmetro movimento receberá 0 quando o 
			personagem estiver parado e maior que zero, quando estiver se locomovendo.
		*/
		animator.SetFloat ("movimento", Mathf.Abs (Input.GetAxisRaw ("Horizontal")));
	
	}

}