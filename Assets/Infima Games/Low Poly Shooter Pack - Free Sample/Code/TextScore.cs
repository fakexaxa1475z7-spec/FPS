using System.Globalization;
using InfimaGames.LowPolyShooterPack.Interface;

public class TextScore : ElementText
{
    protected override void Tick()
    {
        if (ScoreManager.Instance == null)
            return;

        textMesh.text = "Score : " +
            ScoreManager.Instance.GetScore().ToString(CultureInfo.InvariantCulture);
    }
}