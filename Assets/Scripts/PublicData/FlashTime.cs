public class FlashTime
{
    public float Start { private set; get; }
    public float Interval { private set; get; }

    //コンストラクタ
    public FlashTime(float _start, float _interval)
    {
        Start = _start;
        Interval = _interval;
    }
}