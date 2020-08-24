namespace System
{
    public static class String
    {
        public static string Tabs(this string data, int size)
        {
            var tabs = new string(' ', size);
            return $"{tabs}{data}";
        }
    }
}
