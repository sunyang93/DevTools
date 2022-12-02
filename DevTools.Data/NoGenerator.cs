using HashidsNet;

namespace DevTools.Data
{
    public static class NoGenerator
    {
        public static string Generate(long id,string salt="", int minLength = 12)
        {
            var hashids = new Hashids(salt, minLength);
            var hash = hashids.EncodeLong(id);
            return hash;
        }
    }
}
