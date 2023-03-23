namespace Nico.Util
{
    public interface IMetaDataContainer
    {
        void AddData<T>(IMetaData metaData) where T : IMetaData;
    }
}