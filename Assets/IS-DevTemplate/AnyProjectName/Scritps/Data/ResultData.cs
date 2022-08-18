[System.Serializable]
public struct ResultData
{
    public int Point;

    public int PointToWin; 

    public ResultType ResultType;

    public ResultData(int point, int pointToWin, ResultType resultType)
    {
        Point = point;
        PointToWin = pointToWin;
        ResultType = resultType;
    }
}
