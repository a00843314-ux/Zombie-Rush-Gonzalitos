using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TiendaManager : MonoBehaviour
{
	[System.Serializable]
	public class ItemTienda
	{
		public string nombre;        // "X2", "X3", etc.
		public int    precio;        // costo en monedas
		public float  multiplicador; // 2, 3, 4, 5, 8, 10
		public Button boton;
		public TextMeshProUGUI textoPrecio; // opcional: muestra el precio en el botón
	}

	[Header("Items de la tienda")]
	public ItemTienda[] items = new ItemTienda[]
	{
		new ItemTienda { nombre = "X2",  multiplicador = 2f  },
		new ItemTienda { nombre = "X3",  multiplicador = 3f  },
		new ItemTienda { nombre = "X4",  multiplicador = 4f  },
		new ItemTienda { nombre = "X5",  multiplicador = 5f  },
		new ItemTienda { nombre = "X8",  multiplicador = 8f  },
		new ItemTienda { nombre = "X10", multiplicador = 10f },
	};

	[Header("UI")]
	public TextMeshProUGUI textoMonedasTienda;
	public TextMeshProUGUI textoFeedback; // mensaje de "Comprado", "Sin monedas", etc.

	void Start()
	{
		ActualizarUI();

		for (int i = 0; i < items.Length; i++)
		{
			int indice = i; // captura local para el closure
			items[i].boton?.onClick.AddListener(() => IntentarComprar(indice));

			if (items[i].textoPrecio != null)
				items[i].textoPrecio.text = $"{items[i].precio} monedas";
		}
	}

	void ActualizarUI()
	{
		if (textoMonedasTienda != null && PlayerStats.instancia != null)
			textoMonedasTienda.text = "Monedas: " + PlayerStats.instancia.monedas;

		// Resaltar el item actualmente seleccionado
		float actual = MultiplierData.multiplicadorActual;
		foreach (var item in items)
		{
			if (item.boton == null) continue;
			var colors = item.boton.colors;
			colors.normalColor = (item.multiplicador == actual)
				? new Color(0.3f, 1f, 0.3f) // verde = activo
				: Color.white;
			item.boton.colors = colors;
		}
	}

	void IntentarComprar(int indice)
	{
		if (PlayerStats.instancia == null) return;

		ItemTienda item = items[indice];
		int monedas     = PlayerStats.instancia.monedas;

		if (monedas < item.precio)
		{
			MostrarFeedback($"¡Monedas insuficientes! Necesitas {item.precio - monedas} más.", Color.red);
			return;
		}

		// Descontar monedas
		PlayerStats.instancia.AgregarMonedas(-item.precio);

		// Guardar multiplicador en memoria de sesión
		MultiplierData.multiplicadorActual = item.multiplicador;

		MostrarFeedback($"✔ {item.nombre} activado por esta sesión.", Color.green);
		ActualizarUI();
	}

	void MostrarFeedback(string mensaje, Color color)
	{
		if (textoFeedback == null) return;
		textoFeedback.text  = mensaje;
		textoFeedback.color = color;
		CancelInvoke(nameof(LimpiarFeedback));
		Invoke(nameof(LimpiarFeedback), 2.5f);
	}

	void LimpiarFeedback()
	{
		if (textoFeedback != null) textoFeedback.text = "";
	}
}