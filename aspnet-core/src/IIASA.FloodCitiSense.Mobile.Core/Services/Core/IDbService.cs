using LiteDB;
using System.Collections.Generic;

namespace IIASA.FloodCitiSense.Mobile.Core.Services.Core
{
    public interface IDbService<T>
    {
        T CreateItem(T item);

        T UpdateItem(T item);

        T DeleteItem(T item);
        bool DeleteItem(BsonValue item);

        IEnumerable<T> ReadAllItems();
    }
}
