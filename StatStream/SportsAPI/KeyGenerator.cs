namespace SportsAPI
{
    public static class KeyGenerator
    {
        public static string GetKey(int id)
        {
            return $"{id}";
        }
        public static string GetKey(int id, int season)
        {
            return $"{id}, {season}";
        }
    }
}
