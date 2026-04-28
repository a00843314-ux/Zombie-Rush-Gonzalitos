using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideo : MonoBehaviour
{
	[Header("Configuracion")]
	public VideoPlayer videoPlayer;
	public string      nombreEscenaMenu = "Menu";

	private bool videoPausado = false;

	void Start()
	{
		if (videoPlayer == null)
			videoPlayer = GetComponent<VideoPlayer>();

		videoPlayer.loopPointReached += AlTerminarVideo;
	}

	void Update()
	{
		if (videoPausado && Input.anyKeyDown)
			CargarMenu();
	}

	void AlTerminarVideo(VideoPlayer vp)
	{
		videoPausado = true;
		vp.Pause();
	}

	void CargarMenu()
	{
		SceneManager.LoadScene(nombreEscenaMenu);
	}
}