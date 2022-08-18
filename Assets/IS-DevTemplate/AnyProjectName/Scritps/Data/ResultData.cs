[System.Serializable]
public struct ResultData
{
    public int Point;

    public ResultType ResultType;

    public ResultData(int point, ResultType resultType)
    {
        Point = point;
        ResultType = resultType;
    }
}
