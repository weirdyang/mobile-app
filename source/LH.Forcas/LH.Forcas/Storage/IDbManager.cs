using LiteDB;

namespace LH.Forcas.Storage
{
    public interface IDbManager
    {
        void Initialize();

        LiteDatabase GetDatabase();
    }
}