using System.Text;

namespace GoblinGames
{
    public static class Extension
    {
        public static string Spacing(this string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int length = value.Length;
            for (int i = 0; i < length; i++)
            {
                if (i != 0 && char.IsUpper(value[i]))
                {
                    stringBuilder.Append(" ");
                }
                stringBuilder.Append(value[i]);
            }

            return stringBuilder.ToString();
        }
    }
}
